using System.Text.Json.Serialization;
using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;
using Sentyll.Domain.Common.Abstractions.Enums;

namespace Sentyll.Domain.Common.Abstractions.Models.Definitions.HealthChecks.Payload;

public class HealthCheckOverViewPayloadDefinition : IValidatable
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("type")]
    public HealthCheckType Type { get; set; }

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

    public virtual Result Validate() 
        => Result
            .FailureIf(Id == null, "id is required")
            .Ensure(() => Type != null, "type is required")
            .Ensure(() => !string.IsNullOrWhiteSpace(Name), "name is required")
            .Ensure(() => Tags != null, "tags is required")
            .Bind(() => Scheduler.Validate());
    
}