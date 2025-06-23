using System.Text.Json.Serialization;
using Sentyll.Infrastructure.HealthChecks.OpenIdConnectServer.Core.Constants;

namespace Sentyll.Infrastructure.HealthChecks.OpenIdConnectServer.Core.Models.Dto;

internal sealed class DiscoveryEndpointResponse
{
    [JsonPropertyName(OidcConstants.Issuer)]
    public string Issuer { get; set; } = null!;

    [JsonPropertyName(OidcConstants.AuthorizationEndpoint)]
    public string AuthorizationEndpoint { get; set; } = null!;

    [JsonPropertyName(OidcConstants.JwksUri)]
    public string JwksUri { get; set; } = null!;

    [JsonPropertyName(OidcConstants.ResponseTypesSupported)]
    public string[] ResponseTypesSupported { get; set; } = null!;

    [JsonPropertyName(OidcConstants.SubjectTypesSupported)]
    public string[] SubjectTypesSupported { get; set; } = null!;

    [JsonPropertyName(OidcConstants.AlgorithmsSupported)]
    public string[] SigningAlgorithmsSupported { get; set; } = null!;
}
