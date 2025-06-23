using Sentyll.Domain.Common.Abstractions.Enums.Scheduler;
using Sentyll.Domain.Common.Abstractions.Extensions.CSharpFunctionalExtensions;
using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Services.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Delegates;
using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Models.Context;
using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Services.Scheduler;

namespace Sentyll.Infrastructure.Server.Scheduler.Abstractions.Jobs;

public abstract class Job<TJob, TRequest> : Job
    where TJob : Job<TJob, TRequest>
{
    
    /// <inheritdoc />
    public override AsyncSchedulerJobInvocationDelegate InvokeFunc
        => async (serviceProvider, context, cancellationToken) =>
        {
            var jobStoreManager = serviceProvider.GetRequiredService<ISchedulerHostStateManager>();
            
            var job = ActivatorUtilities.CreateInstance<TJob>(serviceProvider);
            
            var request = await jobStoreManager.GetOccurrenceRequestAsync<TRequest>(context.Id, context.Type);
            
            var genericContext = new SchedulerFunctionContext<TRequest>(context, request.GetValueOrThrow());
            
            await job.ExecuteAsync(genericContext, cancellationToken);
        };

    /// <inheritdoc />
    public override (string, Type)? RequestType => (JobIdentifier, typeof(TRequest));
    
    /// <inheritdoc />
    protected Job(
        string jobIdentifier, 
        string? cronSchedule = null, 
        SchedulerJobPriority? priority = null) : base(jobIdentifier, cronSchedule, priority)
    {
    }

    public abstract Task ExecuteAsync(SchedulerFunctionContext<TRequest> functionContext, CancellationToken cancellationToken);
    
}