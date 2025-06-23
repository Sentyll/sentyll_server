using Sentyll.Domain.Common.Abstractions.Enums.Scheduler;

namespace Sentyll.Infrastructure.Server.Scheduler.Abstractions.Models.Context;

public sealed class InternalFunctionContext
{
    public string FunctionName { get; set; }
    
    public Guid JobId { get; set; }
    
    public SchedulerJobType Type { get; set; }
    
    public int Retries { get; set; }
    
    public int RetryCount { get; set; }
    
    public SchedulerJobStatus Status { get; set; }
    
    public long ElapsedTime { get; set; }
    
    public string? ExceptionDetails { get; set; }

    public int[]? RetryIntervals { get; set; }
}