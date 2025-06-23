using System.Linq.Expressions;
using Sentyll.Domain.Data.Abstractions.Entities.Events;
using Sentyll.Domain.Data.Services.Abstractions.Contracts.Repositories.Base;

namespace Sentyll.Domain.Data.Services.Abstractions.Contracts.Repositories.Events;

/// <summary>
/// 
/// </summary>
public interface IEventEntityRepository : IEntityRepository<EventEntity>
{
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="filterFunc"></param>
    /// <param name="orderFunc"></param>
    /// <param name="options"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Result<PaginationResult<EventEntity>>> GetFilteredPaginatedEventsAsync(
        Expression<Func<EventEntity, bool>> filterFunc,
        Expression<Func<EventEntity, object>> orderFunc,
        IPaginationOptions options,
        CancellationToken cancellationToken = default
    );
}