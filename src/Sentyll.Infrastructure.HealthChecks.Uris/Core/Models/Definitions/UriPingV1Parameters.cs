using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;

namespace Sentyll.Infrastructure.HealthChecks.Uris.Core.Models.Definitions;

public sealed class UriPingV1Parameters : IValidatable
{
    
    [JsonPropertyName("uri")]
    public Uri Uri { get; set; }

    [JsonPropertyName("headers")] 
    public string[] Headers { get; set; } = [];
    
    public Result Validate() 
        => Result.FailureIf(Uri == default, "uri is required");
    
}