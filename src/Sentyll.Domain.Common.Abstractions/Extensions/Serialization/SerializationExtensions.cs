using System.Text.Json;
using Sentyll.Domain.Common.Abstractions.Failures;

namespace Sentyll.Domain.Common.Abstractions.Extensions.Serialization;

public static class SerializationExtensions
{
    public static Result<T> Deserialize<T>(this string content)
        => Maybe
            .From(JsonSerializer.Deserialize<T>(content))
            .ToResult(JsonFailures.Null.ToString());
    
    public static Result<IList<T>> DeserializeCollection<T>(this IList<string> contents)
    {
        var result = new List<T>();
        foreach (var content in contents)
        {
            var deserializedContent = Deserialize<T>(content);
            if (deserializedContent.IsFailure)
            {
                return Result.Failure<IList<T>>(deserializedContent.Error);
            }
            
            result.Add(deserializedContent.Value);
        }

        return result;
    }
}