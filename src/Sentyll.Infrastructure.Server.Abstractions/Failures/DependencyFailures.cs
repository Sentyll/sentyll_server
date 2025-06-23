namespace Sentyll.Infrastructure.Server.Abstractions.Failures;

public static class DependencyFailures
{
    private const string Code = "DPDNC";
    
    public static readonly Failure ConfigurationNotFound = new(Code, "0001", "Target Configuration was not found or registered properly");
}