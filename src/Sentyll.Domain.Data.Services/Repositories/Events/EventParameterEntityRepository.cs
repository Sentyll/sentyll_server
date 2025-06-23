using Sentyll.Domain.Data.Abstractions.Entities.Events;
using Sentyll.Domain.Data.Services.Abstractions.Contracts.Repositories.Events;
using Sentyll.Domain.Data.Services.Repositories.Base;

namespace Sentyll.Domain.Data.Services.Repositories.Events;

internal sealed class EventParameterEntityRepository(
    SentyllContext ctx
) : EntityRepository<EventParameterEntity>(ctx), IEventParameterEntityRepository
{
    public async Task<Result<List<EventParameterEntity>>> GetEventParameters(
        Guid eventId,
        CancellationToken cancellationToken = default)
        => await DbSet
            .Include(eventParam => eventParam.Configuration)
            .AsSplitQuery()
            .Where(eventParams => eventParams.EventId == eventId)
            .ToListAsync(cancellationToken)
            .ToResultAsync()
            .Ensure(eventParams => eventParams.Any(), "No event parameters were found for requested event.")
            .ConfigureAwait(false);
}