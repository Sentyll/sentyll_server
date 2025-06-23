using Sentyll.Domain.Common.Abstractions.Contracts.Queues;
using Sentyll.Domain.Common.Abstractions.Models.Definitions.HealthChecks.Payload;
using Sentyll.Infrastructure.Server.TaskPipeline.Abstraction.Models.Queues.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sentyll.Infrastructure.Server.TaskPipeline.Core.Contracts.Services.EventDispatchers;

namespace Sentyll.Infrastructure.Server.TaskPipeline.BackgroundServices;

internal sealed class HealthCheckExecutionQueueConsumer(
    IUnboundedInMemoryQueue<HealthCheckJobExecutionEvent> healthCheckExecutionQueue,
    IServiceProvider serviceProvider,
    ILogger<HealthCheckExecutionQueueConsumer> logger
    ) : BackgroundService
{

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        => await Result
            .Success()
            .TapTry(async () =>
            {
                await foreach (var executionEvent in healthCheckExecutionQueue.ContinuallyReadAllAsync(stoppingToken))
                {
                    await executionEvent.JobDefinition
                        .Bind<HealthCheckOverViewPayloadDefinition>()
                        .Ensure((definition) => definition.Validate())
                        .Bind((definition) => TrackExecutionAsync(definition, executionEvent.JobResult))
                        .TapTry(async () =>
                        {
                            //NOTE:
                            // Since our queue is a singleton instance we need to create a new scope everytime the health check gets consumed
                            // by other scoped services.
                            // TODO: find a better way if this is a performance loss issue.
                            
                            await using var spScope = serviceProvider.CreateAsyncScope();
                            var eventDispatchers = spScope.ServiceProvider.GetRequiredService<IEnumerable<IEventDispatcher>>();
                            
                            foreach (var dispatcher in eventDispatchers)
                            {
                                //NOTE: dispatch failures are delegated, not returned to the main execution flow.
                                await Result
                                    .Try(
                                        () => dispatcher.HandleAsync(executionEvent, stoppingToken),
                                        (exception) => exception
                                    )
                                    .OnFailureCompensate(async (exception) =>
                                        await CompensateForDispatchFailureAsync(dispatcher.GetType().Name, exception));
                            }
                        })
                        .OnFailureCompensate(async (err) => await CompensateForProcessingFailureAsync(err))
                        .ConfigureAwait(false);
                }
            })
            .TapError((err) =>
            {
                //NOTE: We should not ever reach this stage, if we do something happened during Queue reading
                logger.LogError("System experienced a unhandled host exception. Consider restarting :(");
            })
            .ConfigureAwait(false);

    private Task<Result> TrackExecutionAsync(HealthCheckOverViewPayloadDefinition healthCheck, HealthCheckResult jobResult)
    {
        //TODO: implement save changes logic
        // - save to queue to save changes to db or save changes straight when called here.

        return Task.FromResult(Result.Success());
    }

    private Task<Result> CompensateForDispatchFailureAsync(string dispatcherName, Exception exception)
    {
        logger.LogError(exception, "{eventDispatcher} experienced an unhandled exception during invocation.", dispatcherName);
        
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