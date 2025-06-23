using System.Text.Json.Serialization;
using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;

namespace Sentyll.Domain.Common.Abstractions.Models.Definitions.Events.Messaging.Payload;

public class MessagingEventPayloadDefinition<TMessagingEventPayload> 
    : MessagingEventOverViewPayloadDefinition, IValidatable
    where TMessagingEventPayload : IValidatable
{
    
    [JsonPropertyName("message")]
    public TMessagingEventPayload MessagingEvent { get; set; }

    public virtual Result Validate()
    {
        return base
            .Validate()
            .Bind(() => MessagingEvent.Validate());
    }
}