using System.Text.Json.Serialization;
using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;
using Sentyll.Domain.Common.Abstractions.Enums;

namespace Sentyll.Domain.Common.Abstractions.Models.Definitions.Events.Messaging.Contract;

public class MessagingEventMetaContractDefinition : IValidatable
{
    [JsonPropertyName("name")] 
    public string Name { get; set; }
    
    [JsonPropertyName("icon")] 
    public string Icon { get; set; }
    
    [JsonPropertyName("notificationType")] 
    public MessagingEventType Type { get; set; }
    
    public Result Validate()
    {
        return Result
            .FailureIf(string.IsNullOrWhiteSpace(Name), "name is required")
            .Ensure(() => !string.IsNullOrWhiteSpace(Icon), "icon is required")
            .Ensure(() => Type != default, "notificationType is required");
    }
}