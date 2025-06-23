namespace Sentyll.Domain.Common.Abstractions.Extensions.CSharpFunctionalExtensions;

public static class RouteByExtensions
{
    
    public static async Task<Result<TZ>> RouteBy<T, TZ>(
        this Task<Result<T>> awaitableResult, 
        Func<T, bool> predicate, 
        Func<T, Result<TZ>> truePredicate, 
        Func<T, Result<TZ>> falsePredicate)
    {
        var result = await awaitableResult.ConfigureAwait(false);
        if (!result.IsSuccess)
        {
            return Result.Failure<TZ>(result.Error);
        }

        return predicate(result.Value)
            ? result.Bind(truePredicate)
            : result.Bind(falsePredicate);
    }
    
    public static async Task<Result<TZ>> RouteBy<T, TZ>(
        this Task<Result<T>> awaitableResult, 
        Func<T, bool> predicate, 
        Func<T, Task<Result<TZ>>> truePredicate, 
        Func<T, Result<TZ>>  falsePredicate)
    {
        var result = await awaitableResult.ConfigureAwait(false);
        if (!result.IsSuccess)
        {
            return Result.Failure<TZ>(result.Error);
        }

        return predicate(result.Value)
            ? await result.Bind(truePredicate).ConfigureAwait(false)
            : result.Bind(falsePredicate);
    }
    
    public static async Task<Result<TZ>> RouteBy<T, TZ>(
        this Task<Result<T>> awaitableResult, 
        Func<T, bool> predicate, 
        Func<T, Result<TZ>> truePredicate, 
        Func<T, Task<Result<TZ>>>  falsePredicate)
    {
        var result = await awaitableResult.ConfigureAwait(false);
        if (!result.IsSuccess)
        {
            return Result.Failure<TZ>(result.Error);
        }

        return predicate(result.Value)
            ? result.Bind(truePredicate)
            : await result.Bind(falsePredicate).ConfigureAwait(false);
    }
}