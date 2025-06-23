using Sentyll.Domain.Common.Abstractions.Enums;
using Sentyll.Domain.Data.Abstractions.Entities.Events;
using Sentyll.Domain.Data.Services.Abstractions.Contracts.Repositories.HealthChecks;
using Sentyll.Domain.Data.Services.Repositories.Base;

namespace Sentyll.Domain.Data.Services.Repositories.HealthChecks;

internal sealed class HealthCheckEventEntityRepository(
    SentyllContext ctx
) : EntityRepository<HealthCheckEventEntity>(ctx), IHealthCheckEventEntityRepository
{


    public async Task<Result<HealthCheckEventEntity>> GetHealthCheckEventAsync(
        Guid healthCheckId,
        Guid eventId,
        CancellationToken cancellationToken = default)
        => await DbSet
            .Where(hcEvent => hcEvent.HealthCheckId == healthCheckId && hcEvent.EventId == eventId)
            .FirstOrDefaultAsync(cancellationToken)
            .AsMaybe()
            .ToResult("Health Check event does not exist")
            .ConfigureAwait(false);
    
    public async Task<Result> AssignHealthCheckEventAsync(
        Guid healthCheckId,
        Guid eventId,
        CancellationToken cancellationToken = default)
        => await AddAsync(new HealthCheckEventEntity()
            {
                HealthCheckId = healthCheckId,
                EventId = eventId
            }, cancellationToken)
            .ConfigureAwait(false);
    
    public async Task<Result<List<EventEntity>>> GetMessagingEventsAsync(
        Guid healthCheckId,
        CancellationToken cancellationToken = default) 
        => await DbSet
            .Include(hcEvent => hcEvent.Event)
            .ThenInclude(@event => @event.EventCategory)
            .AsSplitQuery()
            .Where(hcEvent => hcEvent.Event.EventCategory.Type == nameof(MessagingEventType))
            .Where(hcEvent => hcEvent.HealthCheckId == healthCheckId)
            .Select(hcEvent => hcEvent.Event)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    
}