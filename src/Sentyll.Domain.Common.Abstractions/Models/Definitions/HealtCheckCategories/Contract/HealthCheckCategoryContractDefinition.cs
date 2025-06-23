using System.Text.Json.Serialization;
using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;

namespace Sentyll.Domain.Common.Abstractions.Models.Definitions.HealtCheckCategories.Contract;

public sealed class HealthCheckCategoryContractDefinition : IValidatable
{

    [JsonPropertyName("schema")]
    public string Schema { get; set; }
    
    [JsonPropertyName("version")]
    public int Version { get; set; }
    
    [JsonPropertyName("categories")]
    public List<HealthCheckCategoryEntryContractDefinition> Categories { get; set; }

    public Result Validate()
    {
        return Result
            .FailureIf(!string.IsNullOrWhiteSpace(Schema), "schema is required")
            .Ensure(() => Version == default, "Version is required")
            .Bind(() =>
            {
                foreach (var category in Categories)
                {
                    var categoryValidationResult = category.Validate();
                    if (categoryValidationResult.IsFailure) return categoryValidationResult;
                }
                
                return Result.Success();
            });
    }
}