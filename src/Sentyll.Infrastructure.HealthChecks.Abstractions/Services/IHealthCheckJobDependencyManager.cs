using Sentyll.Domain.Common.Abstractions.Contracts.Queues;
using Sentyll.Infrastructure.Server.TaskPipeline.Abstraction.Models.Queues.Events;
using Microsoft.Extensions.Logging;

namespace Sentyll.Infrastructure.HealthChecks.Abstractions.Services;

public interface IHealthCheckJobDependencyManager
{
    public ILoggerFactory LoggerFactory { get; }
    public IUnboundedInMemoryQueue<HealthCheckJobExecutionEvent> HealthCheckExecutionQueue { get; }
}