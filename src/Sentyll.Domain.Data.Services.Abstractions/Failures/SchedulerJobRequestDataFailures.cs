namespace Sentyll.Domain.Data.Services.Abstractions.Failures;

public static class SchedulerJobRequestDataFailures
{
    private const string Code = "SCJRD";
    
    public static readonly Failure NotFound = new Failure(Code, "0001", "Target job request was not found.");
}