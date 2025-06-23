using Sentyll.Infrastructure.Server.TaskPipeline.Abstraction.Models.Queues.Events;

namespace Sentyll.Infrastructure.Server.TaskPipeline.Core.Contracts.Services.EventDispatchers;

internal interface IEventDispatcher
{
    public Task<Result> HandleAsync(
        HealthCheckJobExecutionEvent jobExecutionEvent,
        CancellationToken cancellationToken = default
    );
}