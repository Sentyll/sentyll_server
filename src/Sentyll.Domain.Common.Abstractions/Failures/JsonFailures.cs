namespace Sentyll.Domain.Common.Abstractions.Failures;

public static class JsonFailures
{
    private const string Code = "JSONC";
    
    public static readonly Failure Null = new Failure(Code, "0001", "Deserialization resulted in a null result");
}