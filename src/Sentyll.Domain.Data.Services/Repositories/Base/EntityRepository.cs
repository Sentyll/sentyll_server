using Sentyll.Domain.Data.Services.Abstractions.Contracts.Repositories.Base;
using Sentyll.Domain.Data.Services.Extensions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Sentyll.Domain.Data.Services.Repositories.Base;

/// <inheritdoc />
internal abstract class EntityRepository<TEntity>(
    SentyllContext ctx
    ) : IEntityRepository<TEntity> where TEntity : IdentityEntity
{

    protected readonly DbSet<TEntity> DbSet = ctx.Set<TEntity>();

    #region GET

    /// <inheritdoc />
    public virtual async Task<Maybe<TEntity>> GetFirstOrDefaultAsync(
        Guid id, 
        CancellationToken cancellationToken = default) 
        => await DbSet
            .Where(entity => entity.Id == id)
            .FirstOrDefaultAsync(cancellationToken)
            .AsMaybe()
            .ConfigureAwait(false);
    
    /// <inheritdoc />
    public virtual async Task<Result<TEntity>> GetAsync(
        Guid id, 
        CancellationToken cancellationToken = default) 
        => await GetFirstOrDefaultAsync(id, cancellationToken)
            .ToResult(EfCoreFailures.NotFound.ToString())
            .ConfigureAwait(false);
    
    /// <inheritdoc />
    public virtual async Task<Result<List<TEntity>>> GetListUnsafeAsync(
        CancellationToken cancellationToken = default) 
        => await DbSet
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

    /// <inheritdoc />
    public virtual async Task<Result<List<TEntity>>> GetListUnsafeAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken = default)
        => await DbSet
            .Where(entity => ids.Contains(entity.Id))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    
    /// <inheritdoc />
    public virtual async Task<Result<List<TEntity>>> GetListUnsafeAsync(
        Expression<Func<TEntity, bool>> filterFunc,
        Expression<Func<TEntity, object>> orderFunc,
        bool isAsc = true,
        CancellationToken cancellationToken = default) 
        => await DbSet
            .Where(filterFunc)
            .Sort(orderFunc, isAsc)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    
    /// <inheritdoc />
    public virtual async Task<Result<PaginationResult<TEntity>>> GetPaginatedAsync(
        IPaginationOptions options,
        CancellationToken cancellationToken = default
    ) => await DbSet
        .PaginateAsync(options, cancellationToken)
        .ConfigureAwait(false);
    
    /// <inheritdoc />
    public virtual async Task<Result<PaginationResult<TEntity>>> GetPaginatedAsync(
        Expression<Func<TEntity, bool>> filterFunc,
        Expression<Func<TEntity, object>> orderFunc,
        IPaginationOptions options,
        CancellationToken cancellationToken = default
    ) => await DbSet
        .Where(filterFunc)
        .Sort(orderFunc, options.OrderAsc)
        .PaginateAsync(options, cancellationToken)
        .ConfigureAwait(false);

    #endregion
    
    #region ADD

    /// <inheritdoc />
    public EntityEntry<TEntity> TrackAdd(TEntity entity)
        => DbSet.Add(entity);
    
    /// <inheritdoc />
    public ValueTask<EntityEntry<TEntity>> TrackAddAsync(TEntity entity, CancellationToken cancellationToken)
        => DbSet.AddAsync(entity, cancellationToken);
    
    /// <inheritdoc />
    public virtual async Task<Result<TEntity>> AddAsync(
        TEntity entity,
        CancellationToken cancellationToken)
        => await TrackAddAsync(entity, cancellationToken)
            .ToResultAsync()
            .Bind((entityEntry) =>
            {
                return SaveChangesAsync(cancellationToken)
                    .EnsureInsertedAsync()
                    .Map(() => entityEntry);
            })
            .ConfigureAwait(false);
    
    /// <inheritdoc />
    public void TrackAddRange(IEnumerable<TEntity> entities)
        => DbSet.AddRange(entities);

    /// <inheritdoc />
    public Task TrackAddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
        => DbSet.AddRangeAsync(entities, cancellationToken);
    
    /// <inheritdoc />
    public virtual async Task<Result> AddRangeAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken)
        => await TrackAddRangeAsync(entities, cancellationToken)
            .ToResultAsync()
            .Bind(() => SaveChangesAsync(cancellationToken).EnsureInsertedAsync())
            .ConfigureAwait(false);

    #endregion

    #region UPDATE

    /// <inheritdoc />
    public EntityEntry<TEntity> TrackUpdate(TEntity entity)
        => DbSet.Update(entity);
    
    /// <inheritdoc />
    public virtual async Task<Result<TEntity>> UpdateAsync(
        TEntity entity,
        CancellationToken cancellationToken)
        => await TrackUpdate(entity)
            .ToResult()
            .Bind((entityEntry) =>
            {
                return SaveChangesAsync(cancellationToken)
                    .EnsureUpdatedAsync()
                    .Map(() => entityEntry);
            })
            .ConfigureAwait(false);
    
    /// <inheritdoc />
    public void TrackUpdateRange(IEnumerable<TEntity> entities)
        => DbSet.UpdateRange(entities);

    /// <inheritdoc />
    public virtual async Task<Result> UpdateRangeAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken)
        => await Result.Success()
            .Tap(() => TrackUpdateRange(entities))
            .Bind(() => SaveChangesAsync(cancellationToken).EnsureUpdatedAsync())
            .ConfigureAwait(false);

    #endregion

    #region DELETE

    /// <inheritdoc />
    public EntityEntry<TEntity> TrackRemove(TEntity entity)
        => DbSet.Remove(entity);
    
    /// <inheritdoc />
    public virtual async Task<Result<TEntity>> RemoveAsync(
        TEntity entity,
        CancellationToken cancellationToken)
        => await TrackRemove(entity)
            .ToResult()
            .Bind((entityEntry) =>
            {
                return SaveChangesAsync(cancellationToken)
                    .EnsureDeletedAsync()
                    .Map(() => entityEntry);
            })
            .ConfigureAwait(false);
    
    /// <inheritdoc />
    public void TrackRemoveRange(IEnumerable<TEntity> entities)
        => DbSet.RemoveRange(entities);
    
    /// <inheritdoc />
    public virtual async Task<Result> RemoveRangeAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken)
        => await Result.Success()
            .Tap(() => TrackRemoveRange(entities))
            .Bind(() => SaveChangesAsync(cancellationToken).EnsureDeletedAsync())
            .ConfigureAwait(false);

    #endregion

    #region OTHER

    /// <inheritdoc />
    public virtual async Task<Result<bool>> AnyAsync(
        Guid id, 
        CancellationToken cancellationToken = default)
        => await DbSet
            .Where(x => x.Id == id)
            .AnyAsync(cancellationToken)
            .ConfigureAwait(false);

    #endregion

    /// <inheritdoc />
    public void DetachChangeTrackerEntries()
    {
        foreach (var entry in ctx.ChangeTracker.Entries())
        {
            entry.State = EntityState.Detached;
        }
    }
    
    /// <inheritdoc />
    public virtual async Task<Result> CommitAsync(CancellationToken cancellationToken)
        => await SaveChangesAsync(cancellationToken).EnsureChangesDetectedAsync();

    private async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        => await ctx.SaveChangesAsync(cancellationToken);

}