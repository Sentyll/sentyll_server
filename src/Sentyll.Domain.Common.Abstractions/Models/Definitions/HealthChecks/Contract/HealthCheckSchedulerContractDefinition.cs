using System.Text.Json.Serialization;
using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;
using Sentyll.Domain.Common.Abstractions.Models.Definitions.Base;

namespace Sentyll.Domain.Common.Abstractions.Models.Definitions.HealthChecks.Contract;

public sealed class HealthCheckSchedulerContractDefinition : IValidatable
{
    [JsonPropertyName("schedule")]
    public FormInputContractDefinition Schedule { get; set; }
    
    [JsonPropertyName("timeout")]
    public FormInputContractDefinition Timeout { get; set; }
    
    [JsonPropertyName("failureStatus")]
    public FormInputContractDefinition FailureStatus { get; set; }

    public Result Validate()
    {
        return Schedule.Validate()
            .Bind(() => Timeout.Validate())
            .Bind(() => FailureStatus.Validate());
    }
}