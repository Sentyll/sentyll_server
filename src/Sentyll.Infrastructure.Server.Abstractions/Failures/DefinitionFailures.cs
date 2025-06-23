namespace Sentyll.Infrastructure.Server.Abstractions.Failures;

public static class DefinitionFailures
{
    private const string Code = "DEFNI";
    
    public static readonly Failure NotFound = new(Code, "0001", "Target Definition was not found");
}