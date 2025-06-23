using Sentyll.Domain.Data.Abstractions.Entities.Events;
using Sentyll.Domain.Data.Services.Abstractions.Contracts.Repositories.Base;

namespace Sentyll.Domain.Data.Services.Abstractions.Contracts.Repositories.HealthChecks;

public interface IHealthCheckEventEntityRepository : IEntityRepository<HealthCheckEventEntity>
{

    Task<Result<HealthCheckEventEntity>> GetHealthCheckEventAsync(
        Guid healthCheckId,
        Guid eventId,
        CancellationToken cancellationToken = default
    );
    
    Task<Result> AssignHealthCheckEventAsync(
        Guid healthCheckId,
        Guid eventId,
        CancellationToken cancellationToken = default
    );
    
    Task<Result<List<EventEntity>>> GetMessagingEventsAsync(
        Guid healthCheckId,
        CancellationToken cancellationToken = default
    );
    
}