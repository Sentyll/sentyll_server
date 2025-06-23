namespace Sentyll.Infrastructure.HealthChecks.Azure.ApplicationInsights.Core.Constants;

internal static class ApiConstants
{
    
    public const string HttpClientName = "HEALTH_CHECK::APPLICATION_INSIGHTS::V1";
    
    // from https://docs.microsoft.com/en-us/azure/azure-monitor/app/ip-addresses#outgoing-ports
    public static readonly string[] AppInsightsUrls =
    [
        "https://dc.applicationinsights.azure.com",
        "https://dc.applicationinsights.microsoft.com",
        "https://dc.services.visualstudio.com"
    ];

    public static readonly string HealthCheckFailureMessage =
        $"Could not find application insights resource. Searched resources: {string.Join(", ", AppInsightsUrls)}";

    public static string AppInsightsApiRoute(string instrumentationKey)
        => $"/api/profiles/{instrumentationKey}/appId";

}