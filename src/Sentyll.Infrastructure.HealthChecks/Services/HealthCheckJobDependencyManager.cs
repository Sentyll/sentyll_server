using Sentyll.Domain.Common.Abstractions.Contracts.Queues;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Services;
using Sentyll.Infrastructure.Server.TaskPipeline.Abstraction.Models.Queues.Events;
using Microsoft.Extensions.Logging;

namespace Sentyll.Infrastructure.HealthChecks.Services;

internal sealed class HealthCheckJobDependencyManager(
    ILoggerFactory loggerFactory,
    IUnboundedInMemoryQueue<HealthCheckJobExecutionEvent> healthCheckJobExecutionQueue
    ) : IHealthCheckJobDependencyManager
{
    public ILoggerFactory LoggerFactory => loggerFactory;
    public IUnboundedInMemoryQueue<HealthCheckJobExecutionEvent> HealthCheckExecutionQueue => healthCheckJobExecutionQueue;
}