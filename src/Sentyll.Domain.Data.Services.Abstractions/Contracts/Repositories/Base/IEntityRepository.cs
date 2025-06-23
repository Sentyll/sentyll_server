using System.Linq.Expressions;
using Sentyll.Domain.Data.Abstractions.Entities.Base;
using Sentyll.Domain.Data.Abstractions.Failures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Sentyll.Domain.Data.Services.Abstractions.Contracts.Repositories.Base;

public interface IEntityRepository<TEntity> where TEntity : IdentityEntity
{
    /// <summary>
    /// Attempts to find the first target <see cref="TEntity"/> based on the <see cref="id"/> provided
    /// </summary>
    /// <param name="id">Id of the target entity</param>
    /// <param name="cancellationToken">Cancellation token to cancel query</param>
    /// <returns></returns>
    Task<Maybe<TEntity>> GetFirstOrDefaultAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Attempts to find the first target <see cref="TEntity"/> based on the <see cref="id"/> provided
    /// </summary>
    /// <param name="id">Id of the target entity</param>
    /// <param name="cancellationToken">Cancellation token to cancel query</param>
    /// <returns></returns>
    Task<Result<TEntity>> GetAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Returns a non paginated collection of <see cref="TEntity"/>
    /// </summary>
    /// <param name="cancellationToken">Cancellation token to cancel query</param>
    /// <returns></returns>
    Task<Result<List<TEntity>>> GetListUnsafeAsync(
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Returns a non paginated collection of <see cref="TEntity"/> that match the <see cref="ids"/> provided.
    /// </summary>
    /// <param name="ids"></param>
    /// <param name="cancellationToken">Cancellation token to cancel query</param>
    /// <returns></returns>
    Task<Result<List<TEntity>>> GetListUnsafeAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Returns a sorted, non paginated collection of <see cref="TEntity"/> that match the filters provided.
    /// </summary>
    /// <param name="filterFunc"></param>
    /// <param name="orderFunc"></param>
    /// <param name="isAsc"></param>
    /// <param name="cancellationToken">Cancellation token to cancel query</param>
    /// <returns></returns>
    Task<Result<List<TEntity>>> GetListUnsafeAsync(
        Expression<Func<TEntity, bool>> filterFunc,
        Expression<Func<TEntity, object>> orderFunc,
        bool isAsc = true,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Returns a paginated collection of <see cref="TEntity"/> that matches the <see cref="options"/> provided.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="cancellationToken">Cancellation token to cancel query</param>
    /// <returns></returns>
    Task<Result<PaginationResult<TEntity>>> GetPaginatedAsync(
        IPaginationOptions options,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Returns a sorted, paginated collection of <see cref="TEntity"/> that matches the filters provided.
    /// </summary>
    /// <param name="filterFunc"></param>
    /// <param name="orderFunc"></param>
    /// <param name="options"></param>
    /// <param name="cancellationToken">Cancellation token to cancel query</param>
    /// <returns></returns>
    Task<Result<PaginationResult<TEntity>>> GetPaginatedAsync(
        Expression<Func<TEntity, bool>> filterFunc,
        Expression<Func<TEntity, object>> orderFunc,
        IPaginationOptions options,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Starts a transaction to add a single <see cref="TEntity"/>.
    /// </summary>
    /// <remarks>
    /// Allows multiple state changes to happen before anything gets commited to the configured persistant storage. <br />
    /// Needs to be followed with <see cref="CommitAsync"/>
    /// </remarks>
    /// <param name="entity"></param>
    /// <returns></returns>
    EntityEntry<TEntity> TrackAdd(TEntity entity);

    /// <summary>
    /// Starts a transaction to add a single <see cref="TEntity"/>.
    /// </summary>
    /// <remarks>
    /// Allows multiple state changes to happen before anything gets commited to the configured persistant storage. <br />
    /// Needs to be followed with <see cref="CommitAsync"/>
    /// </remarks>
    /// <param name="entity"></param>
    /// <param name="cancellationToken">Cancellation token to cancel the transaction</param>
    /// <returns></returns>
    ValueTask<EntityEntry<TEntity>> TrackAddAsync(
        TEntity entity,
        CancellationToken cancellationToken
    );

    /// <summary>
    /// Attempts to add a single <see cref="TEntity"/>
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken">Cancellation token to cancel the transaction</param>
    /// <returns></returns>
    Task<Result<TEntity>> AddAsync(
        TEntity entity,
        CancellationToken cancellationToken
    );

    /// <summary>
    /// Starts a transaction to add multiple <see cref="TEntity"/>.
    /// </summary>
    /// <remarks>
    /// Allows multiple state changes to happen before anything gets commited to the configured persistant storage. <br />
    /// Needs to be followed with <see cref="CommitAsync"/>
    /// </remarks>
    /// <param name="entities"></param>
    /// <returns></returns>
    void TrackAddRange(IEnumerable<TEntity> entities);

    /// <summary>
    /// Starts a transaction to add multiple <see cref="TEntity"/>.
    /// </summary>
    /// <remarks>
    /// Allows multiple state changes to happen before anything gets commited to the configured persistant storage. <br />
    /// Needs to be followed with <see cref="CommitAsync"/>
    /// </remarks>
    /// <param name="entities"></param>
    /// <param name="cancellationToken">Cancellation token to cancel the transaction</param>
    /// <returns></returns>
    Task TrackAddRangeAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken
    );

    /// <summary>
    /// Attempts to add multiple <see cref="TEntity"/>
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="cancellationToken">Cancellation token to cancel the transaction</param>
    /// <returns></returns>
    Task<Result> AddRangeAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken
    );

    /// <summary>
    /// Starts a transaction to update a single <see cref="TEntity"/>.
    /// </summary>
    /// <remarks>
    /// Allows multiple state changes to happen before anything gets commited to the configured persistant storage. <br />
    /// Needs to be followed with <see cref="CommitAsync"/>
    /// </remarks>
    /// <param name="entity"></param>
    /// <returns></returns>
    EntityEntry<TEntity> TrackUpdate(TEntity entity);

    /// <summary>
    /// Attempts to update a single <see cref="TEntity"/>
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken">Cancellation token to cancel the transaction</param>
    /// <returns></returns>
    Task<Result<TEntity>> UpdateAsync(
        TEntity entity,
        CancellationToken cancellationToken
    );

    /// <summary>
    /// Starts a transaction to update multiple <see cref="TEntity"/>.
    /// </summary>
    /// <remarks>
    /// Allows multiple state changes to happen before anything gets commited to the configured persistant storage. <br />
    /// Needs to be followed with <see cref="CommitAsync"/>
    /// </remarks>
    /// <param name="entities"></param>
    /// <returns></returns>
    void TrackUpdateRange(IEnumerable<TEntity> entities);

    /// <summary>
    /// Attempts to add multiple <see cref="TEntity"/>
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="cancellationToken">Cancellation token to cancel the transaction</param>
    /// <returns></returns>
    Task<Result> UpdateRangeAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken
    );

    /// <summary>
    /// Starts a transaction to remove a single <see cref="TEntity"/>.
    /// </summary>
    /// <remarks>
    /// Allows multiple state changes to happen before anything gets commited to the configured persistant storage. <br />
    /// Needs to be followed with <see cref="CommitAsync"/>
    /// </remarks>
    /// <param name="entity"></param>
    /// <returns></returns>
    EntityEntry<TEntity> TrackRemove(TEntity entity);

    /// <summary>
    /// Attempts to remove a single <see cref="TEntity"/>
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken">Cancellation token to cancel the transaction</param>
    /// <returns></returns>
    Task<Result<TEntity>> RemoveAsync(
        TEntity entity,
        CancellationToken cancellationToken
    );

    /// <summary>
    /// Starts a transaction to remove multiple <see cref="TEntity"/>.
    /// </summary>
    /// <remarks>
    /// Allows multiple state changes to happen before anything gets commited to the configured persistant storage. <br />
    /// Needs to be followed with <see cref="CommitAsync"/>
    /// </remarks>
    /// <param name="entities"></param>
    /// <returns></returns>
    void TrackRemoveRange(IEnumerable<TEntity> entities);

    /// <summary>
    /// Attempts to remove multiple <see cref="TEntity"/>
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="cancellationToken">Cancellation token to cancel the transaction</param>
    /// <returns></returns>
    Task<Result> RemoveRangeAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken
    );

    /// <summary>
    /// Determines if a target <see cref="TEntity"/> exists for the provided <see cref="id"/>
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken">Cancellation token to cancel the query</param>
    /// <returns></returns>
    Task<Result<bool>> AnyAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Retrieves all the <see cref="EntityEntry"/> from the Context Change Tracker and sets their state too <see cref="EntityState.Detached"/>
    /// </summary>
    void DetachChangeTrackerEntries();

    /// <summary>
    /// Attempts to save all state changes to the configured persistant storage provided.
    /// </summary>
    /// <remarks>
    /// If no changes were detected, a <see cref="EfCoreFailures.NoChanges"/> failure will be returned.
    /// </remarks>
    /// <param name="cancellationToken">Cancellation token to cancel the query</param>
    /// <returns></returns>
    Task<Result>CommitAsync(CancellationToken cancellationToken);
}