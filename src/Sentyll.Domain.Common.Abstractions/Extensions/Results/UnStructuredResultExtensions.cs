using Sentyll.Domain.Common.Abstractions.Contracts.Models.Results;
using Sentyll.Domain.Common.Abstractions.Failures;

namespace Sentyll.Domain.Common.Abstractions.Extensions.Results;

public static class UnStructuredResultExtensions
{

    public static Result<List<T>> Bind<T>(this IEnumerable<IUnstructuredResult> results) where T : new()
    {
        var response = new List<T>();
        
        foreach (var result in results)
        {
            var bindResult = result.Bind<T>();
            if (bindResult.IsFailure)
            {
                return Result.Failure<List<T>>(bindResult.Error);
            }

            response.Add(bindResult.Value);
        }

        return response;
    }
    
    public static bool Contains(
        this IUnstructuredResult result,
        string key,
        object comparerValue)
    {
        if (!result.TryGetValue(key, out object? value))
        {
            return false;
        }

        return value?.ToString() == comparerValue.ToString();
    }
    
    public static Result<IUnstructuredResult> Filter(
        this IEnumerable<IUnstructuredResult> results,
        string key,
        object comparerValue)
    {
        foreach (var result in results)
        {
            if (!result.TryGetValue(key, out object? value))
            {
                return Result.Failure<IUnstructuredResult>(UnstructuredResultFailures.KeyNotFound);
            }

            if (value == comparerValue)
            {
                return Result.Success(result);
            }
        }

        return Result.Failure<IUnstructuredResult>(UnstructuredResultFailures.NoResults);
    }
    
}