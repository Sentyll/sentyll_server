using System.Text.Json.Serialization;
using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;

namespace Sentyll.Domain.Common.Abstractions.Models.Definitions.HealthChecks.Payload;

public sealed class HealthCheckPayloadDefinition<THealthCheckPayload> 
    : HealthCheckOverViewPayloadDefinition
    where THealthCheckPayload : IValidatable
{
    
    [JsonPropertyName("healthCheck")]
    public THealthCheckPayload HealthCheck { get; set; }

    public override Result Validate()
    {
        return base
            .Validate()
            .Bind(() => HealthCheck.Validate());
    }
}