using System.Net;

namespace Sentyll.Infrastructure.HealthChecks.Uris.Core.Constants;

internal static class UriConstants
{
    
    public const string HttpClientName = "HEALTH_CHECK::URI::V1";
    
    public static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(10);

    public const int MinSuccessStatusCode = 200;
    public const int MaxSuccessStatusCode = 299;
    
    public static string InValidStatusCodeMessage(int min, int max, HttpStatusCode statusCode)
        => $"Discover endpoint is not responding with code in {min}...{max} range, the current status is {statusCode}";

    public static string ExpectedContentMisMatchMessage(string expectedContent)
        => $"The expected value '{expectedContent}' was not found in the response body.";
    
}