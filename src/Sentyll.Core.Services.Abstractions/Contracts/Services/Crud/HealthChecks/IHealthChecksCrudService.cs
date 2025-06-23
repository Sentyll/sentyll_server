using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;
using Sentyll.Domain.Common.Abstractions.Enums;
using Sentyll.Domain.Common.Abstractions.Models.Definitions.HealthChecks.Payload;
using Sentyll.Domain.Common.Abstractions.Models.Results;
using Sentyll.Domain.Common.Models.ApiRequests.HealthChecks;
using Sentyll.Domain.Common.Models.ApiResult.HealthChecks;

namespace Sentyll.Core.Services.Abstractions.Contracts.Services.Crud.HealthChecks;

public interface IHealthChecksCrudService
{
    Task<Result<PaginationResult<HealthCheckEntityResult>>> GetPaginatedEventsAsync(
        GetPaginatedHealthChecksRequest request,
        CancellationToken cancellationToken = default
    );

    Task<Result> CreateAndQueueAsync<T>(
        HealthCheckRestContentPayloadDefinition<T> definition,
        HealthCheckType type,
        CancellationToken cancellationToken = default)
        where T : IValidatable, new();

}