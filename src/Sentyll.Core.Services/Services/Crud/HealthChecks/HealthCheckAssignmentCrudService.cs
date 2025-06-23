using Sentyll.Core.Services.Abstractions.Contracts.Services.Crud.HealthChecks;
using Sentyll.Domain.Data.Services.Abstractions.Contracts.Repositories.Events;
using Sentyll.Domain.Data.Services.Abstractions.Contracts.Repositories.HealthChecks;

namespace Sentyll.Core.Services.Services.Crud.HealthChecks;

internal sealed class HealthCheckAssignmentCrudService(
    IEventEntityRepository eventEntityRepository,
    IHealthCheckEntityRepository healthCheckEntityRepository,
    IHealthCheckEventEntityRepository healthCheckEventEntityRepository
    ) : IHealthCheckAssignmentCrudService
{

    public async Task<Result> AssignEventAsync(
        Guid healthCheckId,
        Guid eventId,
        CancellationToken cancellationToken = default)
        => await healthCheckEntityRepository
            .AnyAsync(healthCheckId, cancellationToken)
            .Ensure(healthCheckExists => healthCheckExists, "Target health check does not exist")
            .Bind(_ => eventEntityRepository.AnyAsync(eventId, cancellationToken))
            .Ensure(eventExists => eventExists, "Target event does not exist")
            .Bind(eventExists => healthCheckEventEntityRepository.AssignHealthCheckEventAsync(healthCheckId, eventId, cancellationToken))
            .ConfigureAwait(false);

    public async Task<Result> DeleteEventAsync(
        Guid healthCheckId,
        Guid eventId,
        CancellationToken cancellationToken = default)
        => await healthCheckEventEntityRepository
            .GetHealthCheckEventAsync(healthCheckId, eventId, cancellationToken)
            .Bind(hcEvent => healthCheckEventEntityRepository.RemoveAsync(hcEvent, cancellationToken))
            .ConfigureAwait(false);

}