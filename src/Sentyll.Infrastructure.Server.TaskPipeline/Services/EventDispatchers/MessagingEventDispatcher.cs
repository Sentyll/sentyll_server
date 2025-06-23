using System.Collections.Immutable;
using Sentyll.Domain.Common.Abstractions.Enums;
using Sentyll.Domain.Common.Abstractions.Models.Definitions.HealthChecks.Payload;
using Sentyll.Domain.Data.Abstractions.Entities.Events;
using Sentyll.Infrastructure.Server.TaskPipeline.Abstraction.Contracts.Services.EventProcessors;
using Sentyll.Infrastructure.Server.TaskPipeline.Abstraction.Models.Queues.Events;
using Microsoft.Extensions.Logging;
using Sentyll.Infrastructure.Server.TaskPipeline.Core.Contracts.Services.EventDispatchers;
using Sentyll.Infrastructure.Server.TaskPipeline.Core.Contracts.Services.EventResolvers;

namespace Sentyll.Infrastructure.Server.TaskPipeline.Services.EventDispatchers;

internal sealed class MessagingEventDispatcher(
    IEventDescriptorProvider eventDescriptorProvider,
    IEnumerable<IMessagingEventProcessor> eventProcessors,
    ILogger<MessagingEventDispatcher> logger
    ) : IEventDispatcher
{
    
    public async Task<Result> HandleAsync(HealthCheckJobExecutionEvent jobExecutionEvent, CancellationToken cancellationToken = default)
        => await Result
            .Success()
            .BindIf(!eventProcessors.Any(), () =>
            {
                logger.LogWarning("No messaging event processors are available to consume health check execution queue events.");
                return Result.Failure("No messaging event Processors registered");
            })
            .Bind(() => jobExecutionEvent.JobDefinition.Bind<HealthCheckOverViewPayloadDefinition>())
            .BindZip(definition => eventDescriptorProvider.ResolveMessagingProcessorsAsync(definition.Id, cancellationToken))
            .BindTry(zipResult => InvokeEventProcessorsAsync(zipResult.First, zipResult.Second, jobExecutionEvent, cancellationToken))
            .ConfigureAwait(false);

    private async Task<Result> InvokeEventProcessorsAsync(
        HealthCheckOverViewPayloadDefinition payload,
        ImmutableList<EventEntity> events,
        HealthCheckJobExecutionEvent jobExecutionEvent,
        CancellationToken cancellationToken = default)
        => await Result
            .Try(async () =>
            {
                foreach (var @event in events)
                {
                    //NOTE: dispatch failures are delegated, not returned to the main execution flow.
                    await eventProcessors
                        .FirstOrDefault(eventProc => eventProc.Type == (MessagingEventType)@event.EventCategory.TypeValue)
                        .AsMaybe()
                        .Execute(eventProc => InvokeEventProcessorAsync(@event, eventProc, payload, jobExecutionEvent, cancellationToken))
                        .ConfigureAwait(false);
                }

                return Result.Success();
            })
            .OnFailureCompensate(async (err) => await CompensateForProcessingFailureAsync(err))
            .ConfigureAwait(false);
    
    private async Task InvokeEventProcessorAsync(
        EventEntity eventEntity,
        IMessagingEventProcessor eventProcessor,
        HealthCheckOverViewPayloadDefinition definition,
        HealthCheckJobExecutionEvent jobExecutionEvent,
        CancellationToken cancellationToken = default)
        => await Result
            .Try(
                () => eventProcessor.ProcessAsync(new(
                        eventEntity.Id,
                        definition, 
                        jobExecutionEvent.JobResult, 
                        jobExecutionEvent.QueuedOn
                    ), cancellationToken),
                (exception) => exception
            )
            .OnFailureCompensate(async (exception) => await CompensateForDispatchFailureAsync(eventProcessor.GetType().Name, exception))
            .ConfigureAwait(false);
    
    private Task<Result> CompensateForDispatchFailureAsync(string processorName, Exception exception)
    {
        logger.LogError(exception, "{processorName} experienced an unhandled exception during invocation.", processorName);
        
        //TODO: implement compensate logic
        // - log somewhere to inform user that some dispatchers are having issues?
        
        return Task.FromResult(Result.Success());
    }
    
    private Task<Result> CompensateForProcessingFailureAsync(string exception)
    {
        logger.LogError("Something went wrong while processing health check execution [{exception}]", exception);
        
        //TODO: implement compensate logic
        // - log somewhere to inform user that some dispatchers are having issues?
        
        return Task.FromResult(Result.Success());
    }
}