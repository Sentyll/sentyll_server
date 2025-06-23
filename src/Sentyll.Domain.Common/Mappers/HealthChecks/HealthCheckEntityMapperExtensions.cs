using Sentyll.Domain.Common.Mappers.Pagination;
using Sentyll.Domain.Data.Abstractions.Entities.HealthChecks;
using Sentyll.Domain.Common.Models.ApiResult.HealthChecks;

namespace Sentyll.Domain.Common.Mappers.HealthChecks;

public static class HealthCheckEntityMapperExtensions
{
    public static Result<PaginationResult<HealthCheckEntityResult>> ToPaginatedHealthCheckEntityResult(
        this PaginationResult<HealthCheckEntity> result
    ) => result.ToPaginationResult(entity => new HealthCheckEntityResult(
        entity.Id,
        entity.Name,
        entity.Description,
        entity.Type,
        entity.Tags
    ));
}