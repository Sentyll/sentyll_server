using System.Net;

namespace Sentyll.Infrastructure.HealthChecks.Azure.Compute.FunctionApp.Core.Constants;

internal static class AfaConstants
{
    
    public const string HttpClientName = "HEALTH_CHECK::AZURE::COMPUTE::FUNCTIONAPP::V1";
    
    public static string RequestFailedMessage(HttpStatusCode statusCode, string content)
        => $"endpoint is not responding with 200 OK, the current status is {statusCode} and the content {content}";
    
}