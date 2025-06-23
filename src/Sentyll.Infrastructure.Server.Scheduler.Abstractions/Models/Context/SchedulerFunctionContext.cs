using Sentyll.Domain.Common.Abstractions.Enums.Scheduler;

namespace Sentyll.Infrastructure.Server.Scheduler.Abstractions.Models.Context;

public sealed record SchedulerFunctionContext<TRequest> : SchedulerFunctionContext
{
    public SchedulerFunctionContext(SchedulerFunctionContext context, TRequest request) : base
    (
        context.Id,
        context.Type,
        context.RetryCount,
        context.IsDue,
        context.DeleteAsync,
        context.CancelJob
    )
    {
        Request = request;
    }
    
    public TRequest Request { get; }
    
}

public record SchedulerFunctionContext(
    Guid Id, 
    SchedulerJobType Type, 
    int RetryCount,
    bool IsDue, 
    Func<Task> DeleteAsync, 
    Action CancelJob
);