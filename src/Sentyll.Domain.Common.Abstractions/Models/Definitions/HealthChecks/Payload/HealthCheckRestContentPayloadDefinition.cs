using System.Text.Json.Serialization;
using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;

namespace Sentyll.Domain.Common.Abstractions.Models.Definitions.HealthChecks.Payload;

public sealed class HealthCheckRestContentPayloadDefinition<THealthCheckPayload> 
    : IValidatable
    where THealthCheckPayload : IValidatable
{
    
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
    
    [JsonPropertyName("scheduler")]
    public HealthCheckSchedulerPayloadDefinition Scheduler { get; set; }
    
    [JsonPropertyName("healthCheck")]
    public THealthCheckPayload HealthCheck { get; set; }

    public Result Validate() 
        => Result
            .FailureIf(string.IsNullOrWhiteSpace(Name), "name is required")
            .Ensure(() => Tags != default, "tags is required")
            .Ensure(() => IsEnabled != default, "isEnabled is required")
            .Bind(() => HealthCheck.Validate());
    
}