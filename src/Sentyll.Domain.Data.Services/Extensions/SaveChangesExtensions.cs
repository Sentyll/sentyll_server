namespace Sentyll.Domain.Data.Services.Extensions;

internal static class SaveChangesExtensions
{
    
    public static Result EnsureChangesDetected(this int saveResult) => 
        saveResult > 0 ? 
            Result.Success() : 
            Result.Failure(EfCoreFailures.NoChanges);
    
    public static async Task<Result> EnsureChangesDetectedAsync(this Task<int> awaitableSaveResult) 
        => (await awaitableSaveResult.ConfigureAwait(false)).EnsureChangesDetected();
    
    public static Result EnsureInserted(this int saveResult) => 
        saveResult > 0 ? 
            Result.Success() : 
            Result.Failure(EfCoreFailures.NoInsert);
    
    public static async Task<Result> EnsureInsertedAsync(this Task<int> awaitableSaveResult) 
        => (await awaitableSaveResult.ConfigureAwait(false)).EnsureInserted();

    public static Result EnsureUpdated(this int saveResult) => 
        saveResult > 0 ? 
            Result.Success() : 
            Result.Failure(EfCoreFailures.NoUpdate);
    
    public static async Task<Result> EnsureUpdatedAsync(this Task<int> awaitableSaveResult) 
        => (await awaitableSaveResult.ConfigureAwait(false)).EnsureUpdated();
    
    public static Result EnsureDeleted(this int saveResult) => 
        saveResult > 0 ? 
            Result.Success() : 
            Result.Failure(EfCoreFailures.NoDelete);
    
    public static async Task<Result> EnsureDeletedAsync(this Task<int> awaitableSaveResult) 
        => (await awaitableSaveResult.ConfigureAwait(false)).EnsureDeleted();
}