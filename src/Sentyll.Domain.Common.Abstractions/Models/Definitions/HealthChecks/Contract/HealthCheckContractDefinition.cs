using System.Text.Json.Serialization;
using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;
using Sentyll.Domain.Common.Abstractions.Models.Definitions.HealthChecks.Payload;

namespace Sentyll.Domain.Common.Abstractions.Models.Definitions.HealthChecks.Contract;

public sealed class HealthCheckContractDefinition<TRestContentContract, TRestContentMetaContract> 
    : IValidatable
    where TRestContentContract : IValidatable
    where TRestContentMetaContract : IValidatable
{
    [JsonPropertyName("schema")]
    public string Schema { get; set; }
    
    [JsonPropertyName("version")]
    public int Version { get; set; }

    [JsonPropertyName("meta")]
    public HealthCheckMetaContractDefinition MetaContract { get; set; }

    [JsonPropertyName("restContent")]
    public HealthCheckPayloadDefinition<TRestContentContract> RestContent { get; set; }

    [JsonPropertyName("restContentMeta")]
    public HealthCheckRestContentMetaContractDefinition<TRestContentMetaContract> RestContentMeta { get; set; }

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