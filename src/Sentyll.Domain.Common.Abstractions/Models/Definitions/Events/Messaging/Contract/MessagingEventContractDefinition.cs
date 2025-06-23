using System.Text.Json.Serialization;
using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;
using Sentyll.Domain.Common.Abstractions.Models.Definitions.Events.Messaging.Payload;

namespace Sentyll.Domain.Common.Abstractions.Models.Definitions.Events.Messaging.Contract;

public sealed class MessagingEventContractDefinition<TRestContentContract, TRestContentMetaContract> 
    : IValidatable
    where TRestContentContract : IValidatable
    where TRestContentMetaContract : IValidatable
{
    [JsonPropertyName("schema")]
    public string Schema { get; set; }
    
    [JsonPropertyName("version")]
    public int Version { get; set; }

    [JsonPropertyName("meta")]
    public MessagingEventMetaContractDefinition MetaContract { get; set; }

    [JsonPropertyName("restContent")]
    public MessagingEventPayloadDefinition<TRestContentContract> RestContent { get; set; }

    [JsonPropertyName("restContentMeta")]
    public MessagingEventRestContentMetaContractDefinition<TRestContentMetaContract> RestContentMeta { get; set; }

    public Result Validate()
    {
        return Result
            .FailureIf(string.IsNullOrWhiteSpace(Schema), "schema is required")
            .Ensure(() => Version != default, "version is required")
            .Bind(() => MetaContract.Validate())
            .Bind(() => RestContent.Validate())
            .Bind(() => RestContentMeta.Validate());
    }
}