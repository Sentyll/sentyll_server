using System.Net;
using Sentyll.Core.Services.Abstractions.Contracts.Services.Crud.HealthChecks;
using Sentyll.Domain.Common.Abstractions.Models.Results;
using Sentyll.Domain.Common.Models.ApiRequests.HealthChecks;
using Sentyll.Domain.Common.Models.ApiResult.HealthChecks;
using Sentyll.Infrastructure.Server.Abstractions.Contracts.Services;
using Sentyll.UI.Core.Extensions;
using Sentyll.UI.Controllers.Base;

namespace Sentyll.UI.Controllers.V1;

/// <summary>
/// 
/// </summary>
[ApiVersion(1.0)]
[Route("api/[controller]")]
public class HealthChecksController(
    IDefinitionService definitionService,
    IHealthCheckAssignmentCrudService healthCheckAssignmentCrudService,
    IHealthChecksCrudService healthChecksCrudService
    ) : ApiController
{
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType<PaginationResult<HealthCheckEntityResult>>((int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetPaginatedHealthChecksAsync(
        [FromQuery] GetPaginatedHealthChecksRequest request)
        => await healthChecksCrudService
            .GetPaginatedEventsAsync(request, CancellationToken.None)
            .OkOrFailureAsync()
            .ConfigureAwait(false);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="healthCheckId"></param>
    /// <param name="eventId"></param>
    /// <returns></returns>
    [HttpPut("{healthCheckId:guid}/Events/{eventId:guid}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> AssignEventToHealthCheckAsync(
        [FromRoute] Guid healthCheckId,
        [FromRoute] Guid eventId)
        => await healthCheckAssignmentCrudService
            .AssignEventAsync(healthCheckId, eventId, CancellationToken.None)
            .OkOrFailureAsync()
            .ConfigureAwait(false);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="healthCheckId"></param>
    /// <param name="eventId"></param>
    /// <returns></returns>
    [HttpDelete("{healthCheckId:guid}/Events/{eventId:guid}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteHealthCheckEventAsync(
        [FromRoute] Guid healthCheckId,
        [FromRoute] Guid eventId)
        => await healthCheckAssignmentCrudService
            .DeleteEventAsync(healthCheckId, eventId, CancellationToken.None)
            .OkOrFailureAsync()
            .ConfigureAwait(false);
    
}