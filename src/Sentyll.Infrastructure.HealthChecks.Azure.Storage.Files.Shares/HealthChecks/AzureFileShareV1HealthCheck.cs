using Azure.Storage.Files.Shares;
using Sentyll.Domain.Common.Abstractions.Enums;
using Sentyll.Domain.Common.Abstractions.Models.Definitions.HealthChecks.Payload;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Constants.Storage.Cache;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Jobs;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Services;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Storage.Cache;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Sentyll.Infrastructure.HealthChecks.Azure.Storage.Files.Shares.Core.Models.Definitions;

namespace Sentyll.Infrastructure.HealthChecks.Azure.Storage.Files.Shares.HealthChecks;

internal sealed class AzureFileShareV1HealthCheck(
    IHealthCheckJobDependencyManager dependencyManager
    ) : HealthCheckJob<AzureFileShareV1HealthCheck, AzureFileShareV1Parameters>(
        dependencyManager,
        HealthCheckType.AZURE_STORAGE_FILES_SHARES_V1
    )
{
    
    public override async Task<HealthCheckResult> CheckAsync(
        HealthCheckPayloadDefinition<AzureFileShareV1Parameters> jobContext, 
        CancellationToken cancellationToken)
    {
        try
        {
            var shareServiceClient = new ShareServiceClient(jobContext.HealthCheck.ConnectionString);
            
            // Note: ShareServiceClient does not support TokenCredentials as of writing, so only SAS tokens and
            // Account keys may be used to authenticate. However, like the health checks for Azure Blob Storage and
            // Azure Queue Storage, the AzureFileShareHealthCheck similarly enumerates the shares to probe service health.
            await shareServiceClient
                .GetSharesAsync(cancellationToken: cancellationToken)
                .AsPages(pageSizeHint: 1)
                .GetAsyncEnumerator(cancellationToken)
                .MoveNextAsync()
                .ConfigureAwait(false);

            if (!string.IsNullOrEmpty(jobContext.HealthCheck.ShareName))
            {
                var shareClient = HealthChecksClientCache.GetOrAdd(
                    ClientCacheKeys.AzureFileShare(jobContext.HealthCheck.ShareName),
                    _ => shareServiceClient.GetShareClient(jobContext.HealthCheck.ShareName)
                );
                
                await shareClient
                    .GetPropertiesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }

            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return new HealthCheckResult(jobContext.Scheduler.FailureStatus, exception: ex);
        }
    }
    
}