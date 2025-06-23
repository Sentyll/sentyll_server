using System.Text.Json.Serialization;
using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;
using Sentyll.Domain.Common.Abstractions.Enums;

namespace Sentyll.Domain.Common.Abstractions.Models.Definitions.HealthChecks.Contract;

public class HealthCheckMetaContractDefinition : IValidatable
{
    [JsonPropertyName("name")] 
    public string Name { get; set; }
    
    [JsonPropertyName("icon")] 
    public string Icon { get; set; }
    
    [JsonPropertyName("healthCheckType")] 
    public HealthCheckType Type { get; set; }
    
    [JsonPropertyName("healthCheckCategory")] 
    public int HealthCheckCategory { get; set; }

    public Result Validate()
    {
        return Result
            .FailureIf(string.IsNullOrWhiteSpace(Name), "name is required")
            .Ensure(() => !string.IsNullOrWhiteSpace(Icon), "icon is required")
            .Ensure(() => Type != default, "healthCheckType is required")
            .Ensure(() => HealthCheckCategory != default, "healthCheckCategory is required");
    }
}