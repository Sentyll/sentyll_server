namespace Sentyll.Domain.Common.Abstractions.Extensions.CSharpFunctionalExtensions;

public static class ResultExtensions
{
    public static T GetValueOrThrow<T>(this Result<T> result, string? errorMessage = null)
    {
        if (result.IsFailure)
            throw new InvalidOperationException(errorMessage ?? result.Error);

        return result.Value;
    }

    public static Task<Result> ToTask(this Result result)
        => Task.FromResult(result);
    
    public static Task<Result<T>> ToTask<T>(this Result<T> result)
        => Task.FromResult(result);
 
    public static async Task<Result> BindIf<T>(
        this Task<Result<T>> result,
        Func<T, bool> predicate,
        Func<T, Task<Result>> func)
    {
        var awaitResult = await result.ConfigureAwait(false);
        if (!awaitResult.IsSuccess || !predicate(awaitResult.Value))
        {
            return awaitResult;
        }
        
        return await func(awaitResult.Value).ConfigureAwait(false);
    }
    
}