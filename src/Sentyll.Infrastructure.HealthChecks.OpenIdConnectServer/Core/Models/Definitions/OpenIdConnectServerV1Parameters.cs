using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;

namespace Sentyll.Infrastructure.HealthChecks.OpenIdConnectServer.Core.Models.Definitions;

public sealed class OpenIdConnectServerV1Parameters : IValidatable
{
    /// <summary>
    /// The uri of the OpenID Connect server to check.
    /// </summary>
    [JsonPropertyName("idSvrUri")] 
    public Uri IdSvrUri { get; set; }
    
    /// <summary>
    /// OpenID Connect server discover configuration segment.
    /// </summary>
    [JsonPropertyName("discoveryConfigSegment")] 
    public string DiscoveryConfigSegment { get; set; }
    
    /// <summary>
    /// Set to true if the health check shall validate the existence of code, id_token, and the id_token token in the response type values.
    /// </summary>
    [JsonPropertyName("dynamicOpenIdProvider")] 
    public bool IsDynamicOpenIdProvider { get; set; }

    public Result Validate() 
        => Result
            .FailureIf(IdSvrUri == default, "idSvrUri is required")
            .Ensure(() => !string.IsNullOrWhiteSpace(DiscoveryConfigSegment), "discoveryConfigSegment is required")
            .Ensure(() => IsDynamicOpenIdProvider != default, "dynamicOpenIdProvider is required");
}