namespace Sentyll.Domain.Data.Abstractions.Extensions;

public static class EntityExtensions
{

    public static Result<TEntity> ToResult<TEntity>(this EntityEntry<TEntity> entityEntry) 
        where TEntity : class
        => entityEntry.Entity;
    
    public static async Task<Result<TEntity>> ToResultAsync<TEntity>(this ValueTask<EntityEntry<TEntity>> awaitableEntry) 
        where TEntity : class
        => (await awaitableEntry.ConfigureAwait(false)).ToResult();
    
    public static async Task<Result> ToResultAsync(this Task awaitableEntry)
    {
        await awaitableEntry.ConfigureAwait(false);
        return Result.Success();
    }
    
    public static async Task<Result<T>> ToResultAsync<T>(this Task<T> awaitableEntry)
    {
        var awaitResult = await awaitableEntry.ConfigureAwait(false);
        return Result.Success(awaitResult);
    }
}