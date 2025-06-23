using System.Net;
using Sentyll.Core.Services.Abstractions.Contracts.Services.Crud.Events;
using Sentyll.Domain.Common.Abstractions.Models.Results;
using Sentyll.Domain.Common.Models.ApiRequests.Events;
using Sentyll.Domain.Common.Models.ApiResult.Events;
using Sentyll.UI.Core.Extensions;
using Sentyll.UI.Controllers.Base;

namespace Sentyll.UI.Controllers.V1;

/// <summary>
/// 
/// </summary>
[ApiVersion(1.0)]
[Route("api/Events")]
public class EventsController(
    IEventsCrudService eventsCrudService
    ) : ApiController
{

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType<PaginationResult<MessagingEventResult>>((int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetPaginatedEventsAsync(
        [FromQuery] GetPaginatedEventsRequest request)
        => await eventsCrudService
            .GetPaginatedEventsAsync(request, CancellationToken.None)
            .OkOrFailureAsync()
            .ConfigureAwait(false);
    
}