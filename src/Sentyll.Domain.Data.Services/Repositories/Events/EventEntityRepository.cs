using Sentyll.Domain.Data.Abstractions.Entities.Events;
using Sentyll.Domain.Data.Services.Abstractions.Contracts.Repositories.Events;
using Sentyll.Domain.Data.Services.Extensions;
using Sentyll.Domain.Data.Services.Repositories.Base;

namespace Sentyll.Domain.Data.Services.Repositories.Events;

/// <inheritdoc cref="IEventEntityRepository"/>
internal sealed class EventEntityRepository(
    SentyllContext ctx
) : EntityRepository<EventEntity>(ctx), IEventEntityRepository
{

    /// <inheritdoc />
    public async Task<Result<PaginationResult<EventEntity>>> GetFilteredPaginatedEventsAsync(
        Expression<Func<EventEntity, bool>> filterFunc,
        Expression<Func<EventEntity, object>> orderFunc,
        IPaginationOptions options,
        CancellationToken cancellationToken = default)
        => await DbSet
            .Include(entity => entity.EventCategory)
            .AsSplitQuery()
            .Where(filterFunc)
            .Sort(orderFunc, options.OrderAsc)
            .PaginateAsync(options, cancellationToken)
            .ConfigureAwait(false);

}