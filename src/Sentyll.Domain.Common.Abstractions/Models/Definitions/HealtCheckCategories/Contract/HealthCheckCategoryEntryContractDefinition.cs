using System.Text.Json.Serialization;
using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;

namespace Sentyll.Domain.Common.Abstractions.Models.Definitions.HealtCheckCategories.Contract;

public sealed class HealthCheckCategoryEntryContractDefinition : IValidatable
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("slug")]
    public string Slug { get; set; }
    
    [JsonPropertyName("displayName")]
    public string DisplayName { get; set; }
    
    [JsonPropertyName("sortOrder")]
    public int SortOrder { get; set; }

    public Result Validate()
    {
        return Result
            .FailureIf(Id == default || Id == null, "id is required")
            .Ensure(() => !string.IsNullOrWhiteSpace(Slug), "slug is required")
            .Ensure(() => !string.IsNullOrWhiteSpace(DisplayName), "slug is required")
            .Ensure(() => SortOrder == default, "sortOrder is required");
    }
}