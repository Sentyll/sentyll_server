using Sentyll.Core.Services.Abstractions.Contracts.Services.Crud.HealthChecks;
using Sentyll.Core.Services.Extensions;
using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;
using Sentyll.Domain.Common.Abstractions.Enums;
using Sentyll.Domain.Common.Abstractions.Models.Definitions.HealthChecks.Payload;
using Sentyll.Domain.Common.Abstractions.Models.Results;
using Sentyll.Domain.Common.Mappers.Definition;
using Sentyll.Domain.Common.Mappers.HealthChecks;
using Sentyll.Domain.Common.Models.ApiRequests.Events;
using Sentyll.Domain.Common.Models.ApiRequests.HealthChecks;
using Sentyll.Domain.Common.Models.ApiResult.HealthChecks;
using Sentyll.Domain.Data.Abstractions.Entities.HealthChecks;
using Sentyll.Domain.Data.Services.Abstractions.Contracts.Repositories.HealthChecks;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Services;

namespace Sentyll.Core.Services.Services.Crud.HealthChecks;

internal sealed class HealthChecksCrudService(
    IHealthCheckEntityRepository healthCheckEntityRepository,
    IHealthCheckRegistrationService healthCheckRegistrationService
    ) : IHealthChecksCrudService
{
    
    public async Task<Result<PaginationResult<HealthCheckEntityResult>>> GetPaginatedEventsAsync(
        GetPaginatedHealthChecksRequest request,
        CancellationToken cancellationToken = default)
    {
        Expression<Func<HealthCheckEntity, object>> orderFunc = request.OrderBy switch
        {
            GetPaginatedEventsRequest.OrderByIsEnabled => unit => unit.IsEnabled,
            _ => unit => unit.Name
        };

        var filter = PredicateBuilder
            .New<HealthCheckEntity>(_ => true)
            .AndIf(!string.IsNullOrWhiteSpace(request.SearchText), () =>
            {
                var loweredSearchText = request.SearchText!.ToLower();
                return x => x.Name.ToLower().Contains(loweredSearchText);
            });
        
        return await healthCheckEntityRepository
            .GetPaginatedAsync(filter, orderFunc, request, cancellationToken)
            .Bind(entity => entity.ToPaginatedHealthCheckEntityResult())
            .ConfigureAwait(false);
    }

    public async Task<Result> CreateAndQueueAsync<T>(
        HealthCheckRestContentPayloadDefinition<T> definition,
        HealthCheckType type,
        CancellationToken cancellationToken = default)
        where T : IValidatable, new()
        => await definition
            .ToHealthCheckEntity(type)
            .Bind(healthCheckEntity => healthCheckEntityRepository.AddAsync(healthCheckEntity, cancellationToken))
            .Bind(trackedHealthCheckEntity => definition.ToHealthCheckPayloadDefinition(trackedHealthCheckEntity.Id, type))
            .BindIf(
                payloadDefinition => payloadDefinition.IsEnabled, 
                (payloadDefinition) => healthCheckRegistrationService.ScheduleHealthCheckAsync(payloadDefinition, cancellationToken)
            )
            .ConfigureAwait(false);

}