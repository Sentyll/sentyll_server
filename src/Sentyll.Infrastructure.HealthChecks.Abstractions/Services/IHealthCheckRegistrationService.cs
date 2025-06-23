using CSharpFunctionalExtensions;
using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;
using Sentyll.Domain.Common.Abstractions.Models.Definitions.HealthChecks.Payload;

namespace Sentyll.Infrastructure.HealthChecks.Abstractions.Services;

public interface IHealthCheckRegistrationService
{
    
    Task<Result> ScheduleHealthCheckAsync<T>(
        HealthCheckPayloadDefinition<T> payloadDefinition,
        CancellationToken cancellationToken = default
    ) where T : IValidatable, new();
    
}