using Sentyll.Domain.Common.Abstractions.Enums;
using Sentyll.Domain.Common.Abstractions.Models.Definitions.HealthChecks.Payload;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Jobs;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Sentyll.Infrastructure.HealthChecks.Azure.ApplicationInsights.Core.Constants;
using Sentyll.Infrastructure.HealthChecks.Azure.ApplicationInsights.Core.Models.Definitions;

namespace Sentyll.Infrastructure.HealthChecks.Azure.ApplicationInsights.HealthChecks;

internal sealed class AzureApplicationInsightsV1HealthCheck(
    IHealthCheckJobDependencyManager dependencyManager,
    IHttpClientFactory clientFactory
    ) : HealthCheckJob<AzureApplicationInsightsV1HealthCheck, AzureApplicationInsightsV1Parameters>(
        dependencyManager, 
        HealthCheckType.AZURE_APPLICATIONINSIGHTS_V1
    )
{
    
    public override async Task<HealthCheckResult> CheckAsync(
        HealthCheckPayloadDefinition<AzureApplicationInsightsV1Parameters> jobContext, 
        CancellationToken cancellationToken)
    {
        try
        {
            bool resourceExists = await ResourceExists(jobContext.HealthCheck.InstrumentationKey,cancellationToken)
                .ConfigureAwait(false);
            
            return resourceExists 
                ? HealthCheckResult.Healthy() 
                : new HealthCheckResult(jobContext.Scheduler.FailureStatus, description: ApiConstants.HealthCheckFailureMessage);
        }
        catch (Exception ex)
        {
            return new HealthCheckResult(jobContext.Scheduler.FailureStatus, exception: ex);
        }
    }
    
    private async Task<bool> ResourceExists(string instrumentationKey, CancellationToken cancellationToken)
    {
        using var httpClient = clientFactory.CreateClient(ApiConstants.HttpClientName);

        string path = ApiConstants.AppInsightsApiRoute(instrumentationKey);
        int index = 0;
        var exceptions = new List<Exception>();
        while (index < ApiConstants.AppInsightsUrls.Length)
        {
            try
            {
                var uri = new Uri(ApiConstants.AppInsightsUrls[index++] + path);
                using var response = await httpClient
                    .GetAsync(uri, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                    .ConfigureAwait(false);
                
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                exceptions.Add(e);
            }
        }

        // All endpoints threw exceptions
        if (exceptions.Count == ApiConstants.AppInsightsUrls.Length)
        {
            throw new AggregateException(exceptions.ToArray());
        }

        // No success responses were returned and at least one endpoint returned an unsuccessful response
        return false;
    }
    
}