using Sentyll.Domain.Common.Abstractions.Models.Definitions.HealthChecks.Payload;
using Sentyll.Infrastructure.Server.TaskPipeline.Abstraction.Contracts.Services.EventProcessors;
using Sentyll.Infrastructure.Server.TaskPipeline.Abstraction.Models.Queues.Events;
using Microsoft.Extensions.Logging;
using Sentyll.Infrastructure.Server.TaskPipeline.Core.Contracts.Services.EventDispatchers;

namespace Sentyll.Infrastructure.Server.TaskPipeline.Services.EventDispatchers;

internal sealed class WebhookEventDispatcher(
    IEnumerable<IWebhookEventProcessor> eventProcessors,
    ILogger<WebhookEventDispatcher> logger
    ) : IEventDispatcher
{

    public async Task<Result> HandleAsync(HealthCheckJobExecutionEvent jobExecutionEvent,
        CancellationToken cancellationToken = default)
        => await Result
            .Success()
            .BindIf(!eventProcessors.Any(), () =>
            {
                logger.LogWarning("No webhook event processors are available to consume health check execution queue events.");
                return Result.Failure("No webhook event Processors");
            })
            .Bind(() => jobExecutionEvent.JobDefinition.Bind<HealthCheckOverViewPayloadDefinition>())
            .BindTry(async (definition) =>
            {
                foreach (var eventProcessor in eventProcessors)
                {
                    //NOTE: dispatch failures are delegated, not returned to the main execution flow.
                    await Result
                        .Try(
                            () => eventProcessor.ProcessAsync(new(Guid.Empty,definition, jobExecutionEvent.JobResult, jobExecutionEvent.QueuedOn), cancellationToken),
                            (exception) => exception
                        )
                        .OnFailureCompensate(async (exception) => await CompensateForDispatchFailureAsync(
                                eventProcessor.GetType().Name,
                                exception
                            )
                            .ConfigureAwait(false));
                }

                return Result.Success();
            })
            .OnFailureCompensate(async (err) => await CompensateForProcessingFailureAsync(err))
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