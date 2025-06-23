using Sentyll.Domain.Common.Abstractions.Enums;
using Sentyll.Domain.Common.Abstractions.Extensions.Definitions;
using Sentyll.Domain.Common.Abstractions.Models.Definitions.HealthChecks.Payload;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Jobs;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Sentyll.Infrastructure.HealthChecks.Azure.Compute.FunctionApp.Core.Constants;
using Sentyll.Infrastructure.HealthChecks.Azure.Compute.FunctionApp.Core.Models.Definitions;

namespace Sentyll.Infrastructure.HealthChecks.Azure.Compute.FunctionApp.HealthChecks;

internal sealed class AzureFunctionAppHttpEndpointV1HealthCheck(
    IHealthCheckJobDependencyManager dependencyManager,
    IHttpClientFactory httpClientFactory
    ) : HealthCheckJob<AzureFunctionAppHttpEndpointV1HealthCheck, AzureFunctionAppHttpEndpointV1Parameters>(
        dependencyManager, 
        HealthCheckType.AZURE_COMPUTE_FUNCTIONAPP_ENDPOINT_V1
    )
{
    
    public override async Task<HealthCheckResult> CheckAsync(
        HealthCheckPayloadDefinition<AzureFunctionAppHttpEndpointV1Parameters> jobContext, 
        CancellationToken cancellationToken)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient(AfaConstants.HttpClientName);
            httpClient.DefaultRequestHeaders.Add("x-functions-key", jobContext.HealthCheck.XFunctionKey);
            
            var headers = jobContext.HealthCheck.Headers.ExtractKeyValueTags();
            foreach (var header in headers)
            {
                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
            
            using var response = await httpClient
                .GetAsync(jobContext.HealthCheck.Uri, cancellationToken)
                .ConfigureAwait(false);
            
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content
                    .ReadAsStringAsync(cancellationToken)
                    .ConfigureAwait(false);
                
                return new HealthCheckResult(jobContext.Scheduler.FailureStatus, description: AfaConstants.RequestFailedMessage(response.StatusCode,content));
            }

            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return new HealthCheckResult(jobContext.Scheduler.FailureStatus, exception: ex);
        }
    }
    
}