using System.Net;

namespace Sentyll.Infrastructure.HealthChecks.OpenIdConnectServer.Core.Constants;

internal static class OidcConstants
{
    public const string Issuer = "issuer";

    public const string AuthorizationEndpoint = "authorization_endpoint";

    public const string JwksUri = "jwks_uri";

    public const string ResponseTypesSupported = "response_types_supported";

    public const string SubjectTypesSupported = "subject_types_supported";

    public const string AlgorithmsSupported = "id_token_signing_alg_values_supported";

    public static string[] RequiredResponseTypes => ["code", "id_token"];

    public static string[] RequiredCombinedResponseTypes => ["token id_token", "id_token"];

    public static string[] RequiredSubjectTypes => ["pairwise", "public"];

    public static string[] RequiredAlgorithms => ["RS256"];

    public static string RequestFailedMessage(HttpStatusCode statusCode, string content)
        => $"Discover endpoint is not responding with 200 OK, the current status is {statusCode} and the content {content}";

    public static string DeserializeFailedMessage
        => "Could not deserialize to discovery endpoint response!";
    
    public static string GetMissingValueExceptionMessage(string value) =>
        $"Invalid discovery response - '{value}' must be set!";

    public static string GetMissingRequiredValuesExceptionMessage(string value, string[] requiredValues) =>
        $"Invalid discovery response - '{value}' must be one of the following values: {string.Join(",", requiredValues)}!";

    public static string GetMissingRequiredAllValuesExceptionMessage(string value, string[] requiredValues) =>
        $"Invalid discovery response - '{value}' must contain the following values: {string.Join(",", requiredValues)}!";
    
    public const string HttpClientName = "HEALTH_CHECK::OPENIDCONNECTSERVER::V1";
}