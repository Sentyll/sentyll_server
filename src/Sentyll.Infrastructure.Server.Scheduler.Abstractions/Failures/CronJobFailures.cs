using Sentyll.Domain.Common.Abstractions.Failures;

namespace Sentyll.Infrastructure.Server.Scheduler.Abstractions.Failures;

public static class CronJobFailures
{
    private const string Code = "CRONJ";
    
    public static readonly Failure FunctionNull = new Failure(Code, "0001", "Job Function cannot be null.");
    public static readonly Failure FunctionNotFound = new Failure(Code, "0002", "Job Function does not exist or was not registered during startup.");
}