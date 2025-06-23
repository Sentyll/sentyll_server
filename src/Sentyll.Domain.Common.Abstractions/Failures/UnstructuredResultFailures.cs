namespace Sentyll.Domain.Common.Abstractions.Failures;

public static class UnstructuredResultFailures
{
    private const string Code = "UNSTR";
    
    public static readonly Failure EmptySerializationResult = new(Code, "0001", "Serialization resulted in a empty string.");
    public static readonly Failure DeserializedToNull = new(Code, "0002", "Deserialization resulted in a null response.");
    public static readonly Failure KeyNotFound = new(Code, "0003", "Target key was not found");
    public static readonly Failure NoResults = new(Code, "0004","No results were found");
}