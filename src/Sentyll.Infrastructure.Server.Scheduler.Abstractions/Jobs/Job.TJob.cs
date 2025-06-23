using Sentyll.Domain.Common.Abstractions.Enums.Scheduler;
using Microsoft.Extensions.DependencyInjection;
using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Delegates;
using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Models.Context;

namespace Sentyll.Infrastructure.Server.Scheduler.Abstractions.Jobs;

public abstract class Job<TJob> : Job
    where TJob : Job<TJob>
{
    
    /// <inheritdoc />
    public override AsyncSchedulerJobInvocationDelegate InvokeFunc
        => async (serviceProvider, context, cancellationToken) =>
        {
            var service = ActivatorUtilities.CreateInstance<TJob>(serviceProvider);
            await service.ExecuteAsync(context, cancellationToken);
        };

    /// <inheritdoc />
    protected Job(
        string jobIdentifier, 
        string? cronSchedule = null, 
        SchedulerJobPriority? priority = null) : base(jobIdentifier, cronSchedule, priority)
    {
    }

    public abstract Task ExecuteAsync(SchedulerFunctionContext functionContext, CancellationToken cancellationToken);
    
}