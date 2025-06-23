using System.Text.Json.Serialization;
using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;
using Sentyll.Domain.Common.Abstractions.Models.Definitions.Base;

namespace Sentyll.Domain.Common.Abstractions.Models.Definitions.Events.Messaging.Contract;

public sealed class MessagingEventRestContentMetaContractDefinition<TMessageEventContractDefinition> 
    : IValidatable
    where TMessageEventContractDefinition : IValidatable
{
    [JsonPropertyName("id")]
    public FormInputContractDefinition Id { get; set; }
    
    [JsonPropertyName("type")]
    public FormInputContractDefinition Type { get; set; }
    
    [JsonPropertyName("name")]
    public FormInputContractDefinition Name { get; set; }
    
    [JsonPropertyName("description")]
    public FormInputContractDefinition Description { get; set; }
    
    [JsonPropertyName("tags")]
    public FormInputContractDefinition Tags { get; set; }

    [JsonPropertyName("message")]
    public TMessageEventContractDefinition MessagingEvent { get; set; }

    public Result Validate()
    {
        return Id.Validate()
            .Bind(() => Type.Validate())
            .Bind(() => Name.Validate())
            .Bind(() => Description.Validate())
            .Bind(() => Tags.Validate())
            .Bind(() => MessagingEvent.Validate());
    }
}