using System.Text.Json.Serialization;
using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;
using Sentyll.Domain.Common.Abstractions.Enums;

namespace Sentyll.Domain.Common.Abstractions.Models.Definitions.Events.Messaging.Payload;

public class MessagingEventOverViewPayloadDefinition : IValidatable
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("type")]
    public MessagingEventType Type { get; set; }

    [JsonPropertyName("name")] 
    public string Name { get; set; }

    [JsonPropertyName("description")] 
    public string? Description { get; set; }
    
    /// <summary>
    /// A list of tags that can be used to filter sets of health checks. Optional.
    /// </summary>
    [JsonPropertyName("tags")]
    public string[] Tags { get; set; } = [];
    
    [JsonPropertyName("isEnabled")] 
    public bool IsEnabled { get; set; } = false;

    public virtual Result Validate()
    {
        return Result
            .FailureIf(Id == default, "id is required")
            .Ensure(() => Type != default, "type is required")
            .Ensure(() => !string.IsNullOrWhiteSpace(Name), "name is required");
    }
}