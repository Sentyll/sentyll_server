using Azure.Storage.Blobs;
using Sentyll.Domain.Common.Abstractions.Enums;
using Sentyll.Domain.Common.Abstractions.Models.Definitions.HealthChecks.Payload;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Constants.Storage.Cache;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Jobs;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Services;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Storage.Cache;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Sentyll.Infrastructure.HealthChecks.Azure.Storage.Blobs.Core.Models.Definitions;

namespace Sentyll.Infrastructure.HealthChecks.Azure.Storage.Blobs.HealthChecks;

internal sealed class AzureBlobStorageV1HealthCheck(
    IHealthCheckJobDependencyManager dependencyManager
    ) : HealthCheckJob<AzureBlobStorageV1HealthCheck, AzureBlobStorageV1Parameters>(
        dependencyManager,
        HealthCheckType.AZURE_STORAGE_BLOB_V1
    )
{
    
    public override async Task<HealthCheckResult> CheckAsync(
        HealthCheckPayloadDefinition<AzureBlobStorageV1Parameters> jobContext, 
        CancellationToken cancellationToken)
    {
        try
        {
            var blobServiceClient = new BlobServiceClient(jobContext.HealthCheck.ConnectionString);
            
            if (!string.IsNullOrEmpty(jobContext.HealthCheck.ContainerName))
            {
                // Note: PoLP (Principle of least privilege)
                // This can be used having at least the role assignment "Storage Blob Data Reader" at container level or at least "Storage Blob Data Reader" at storage account level.
                // See https://docs.microsoft.com/en-us/azure/storage/common/storage-auth-aad-app?tabs=dotnet#configure-permissions-for-access-to-blob-and-queue-data
                var containerClient = HealthChecksClientCache.GetOrAdd(
                    ClientCacheKeys.AzureBlob(jobContext.HealthCheck.ContainerName),
                    _ => blobServiceClient.GetBlobContainerClient(jobContext.HealthCheck.ContainerName)
                );
                
                await containerClient
                    .GetPropertiesAsync(cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
            }
            else
            {
                // Note: BlobServiceClient.GetPropertiesAsync() cannot be used with only the role assignment
                // "Storage Blob Data Contributor," so BlobServiceClient.GetBlobContainersAsync() is used instead to probe service health.
                // However, BlobContainerClient.GetPropertiesAsync() does have sufficient permissions.
                // Note: PoLP (Principle of least privilege)
                // This can be used having at least "Storage Blob Data Reader" at storage account level.
                // See https://docs.microsoft.com/en-us/azure/storage/common/storage-auth-aad-app?tabs=dotnet#configure-permissions-for-access-to-blob-and-queue-data
                await blobServiceClient
                    .GetBlobContainersAsync(cancellationToken: cancellationToken)
                    .AsPages(pageSizeHint: 1)
                    .GetAsyncEnumerator(cancellationToken)
                    .MoveNextAsync()
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