using Sentyll.Domain.Common.Abstractions.Failures;

namespace Sentyll.Infrastructure.Server.Scheduler.Abstractions.Failures;

public static class SchedulerJobRequestFailures
{
    private const string Code = "SCHJR";
    
    public static readonly Failure NotGZipCompressed = new Failure(Code, "0001", "Job Request bytes are not GZip compressed.");
}