using Sentyll.Domain.Common.Abstractions.Failures;

namespace Sentyll.Infrastructure.Server.Scheduler.Abstractions.Failures;

public static class TimerJobFailures
{
    private const string Code = "TIMRJ";
    
    public static readonly Failure ExecutionNull = new Failure(Code, "0001", "ExecutionTime cannot be null");
}