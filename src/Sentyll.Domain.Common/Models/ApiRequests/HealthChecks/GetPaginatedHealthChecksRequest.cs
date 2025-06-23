using System.Text.Json.Serialization;
using Sentyll.Domain.Common.Abstractions.Attributes;
using Sentyll.Domain.Data.Services.Abstractions.Models.Request.Base;

namespace Sentyll.Domain.Common.Models.ApiRequests.HealthChecks;

public sealed record GetPaginatedHealthChecksRequest : BasePaginationRequest
{
    
    public const string OrderByName = "name";
    public const string OrderByIsEnabled = "isEnabled";

    [JsonPropertyName("orderBy")]
    [OrderBy(OrderByName, OrderByIsEnabled)]
    public override string OrderBy { get; set; } = OrderByName;
    
    [JsonPropertyName("isEnabled")]
    public bool? IsEnabled { get; set; }
    
}