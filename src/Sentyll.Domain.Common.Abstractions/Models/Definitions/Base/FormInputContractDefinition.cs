using System.Text.Json.Serialization;
using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;

namespace Sentyll.Domain.Common.Abstractions.Models.Definitions.Base;

public sealed class FormInputContractDefinition : IValidatable
{
    [JsonPropertyName("formType")]
    public string FormType { get; set; }
    
    [JsonPropertyName("displayName")]
    public string DisplayName { get; set; }
    
    [JsonPropertyName("defaultValue")]
    public object? DefaultValue { get; set; }
    
    [JsonPropertyName("rules")]
    public string[] Rules { get; set; }

    public Result Validate()
    {
        return Result
            .FailureIf(string.IsNullOrWhiteSpace(FormType), "formType is required")
            .Ensure(() => !string.IsNullOrWhiteSpace(DisplayName), "displayName is required")
            .Ensure(() => Rules != default, "rules is required");
    }
}