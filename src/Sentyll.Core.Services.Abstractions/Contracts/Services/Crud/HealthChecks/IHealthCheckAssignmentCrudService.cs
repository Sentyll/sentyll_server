namespace Sentyll.Core.Services.Abstractions.Contracts.Services.Crud.HealthChecks;

public interface IHealthCheckAssignmentCrudService
{
    Task<Result> AssignEventAsync(
        Guid healthCheckId,
        Guid eventId,
        CancellationToken cancellationToken = default
    );

    Task<Result> DeleteEventAsync(
        Guid healthCheckId,
        Guid eventId,
        CancellationToken cancellationToken = default
    );
}