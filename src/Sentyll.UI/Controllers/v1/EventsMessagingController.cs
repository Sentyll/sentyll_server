using Sentyll.Core.Services.Abstractions.Contracts.Services.Crud.Events;
using Sentyll.Domain.Common.Abstractions.Enums;
using Sentyll.Domain.Common.Abstractions.Models.Definitions.Events.Messaging.Payload;
using Sentyll.Infrastructure.Events.Messaging.Azure.Communication.Emailer.Core.Models.Definitions;
using Sentyll.Infrastructure.Events.Messaging.MicrosoftTeams.Core.Models.Definitions;
using Sentyll.Infrastructure.Events.Messaging.Slack.Core.Models.Definitions;
using Sentyll.UI.Core.Extensions;
using Sentyll.UI.Controllers.Base;

namespace Sentyll.UI.Controllers.V1;

/// <summary>
/// 
/// </summary>
[ApiVersion(1.0)]
[Route("api/Events/Messaging")]
public class EventsMessagingController(
    IEventsCrudService eventsCrudService
    ) : ApiController
{
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpPut("Slack")]
    public async Task<IActionResult> CreateSlackMessagingEventAsync(
        [FromBody] MessagingEventRestContentPayloadDefinition<SlackV1Parameters> request)
        => await eventsCrudService
            .CreateNewMessagingEventAsync(request, MessagingEventType.SLACK, CancellationToken.None)
            .OkOrFailureAsync()
            .ConfigureAwait(false);
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpPut("Microsoft/Teams")]
    public async Task<IActionResult> CreateMicrosoftTeamsMessagingEventAsync(
        [FromBody] MessagingEventRestContentPayloadDefinition<MicrosoftTeamsV1Parameters> request)
        => await eventsCrudService
            .CreateNewMessagingEventAsync(request, MessagingEventType.MICROSOFT_TEAMS, CancellationToken.None)
            .OkOrFailureAsync()
            .ConfigureAwait(false);
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpPut("Azure/CommunicationServices/Email")]
    public async Task<IActionResult> CreateAzureCommunicationServicesEmailerMessagingEventAsync(
        [FromBody] MessagingEventRestContentPayloadDefinition<AzureCommunicationEmailerV1Parameters> request)
        => await eventsCrudService
            .CreateNewMessagingEventAsync(request, MessagingEventType.AZURE_COMMUNICATIONSERVICESES_EMAIL, CancellationToken.None)
            .OkOrFailureAsync()
            .ConfigureAwait(false);
    
}