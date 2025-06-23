using System.Collections.Concurrent;
using Azure.Storage.Queues;
using Sentyll.Domain.Common.Abstractions.Enums;
using Sentyll.Domain.Common.Abstractions.Models.Definitions.HealthChecks.Payload;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Constants.Storage.Cache;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Jobs;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Services;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Storage.Cache;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Sentyll.Infrastructure.HealthChecks.Azure.Storage.Queues.Core.Models.Definitions;

namespace Sentyll.Infrastructure.HealthChecks.Azure.Storage.Queues.HealthChecks;

internal sealed class AzureQueueStorageV1HealthCheck(
    IHealthCheckJobDependencyManager dependencyManager
    ) : HealthCheckJob<AzureQueueStorageV1HealthCheck, AzureQueueStorageV1Parameters>(
        dependencyManager,
        HealthCheckType.AZURE_STORAGE_QUEUES_V1
    )
{
    
    private static readonly ConcurrentDictionary<string, QueueClient> QueueClients = new();

    public override async Task<HealthCheckResult> CheckAsync(
        HealthCheckPayloadDefinition<AzureQueueStorageV1Parameters> jobContext, 
        CancellationToken cancellationToken)
    {
        try
        {
            var serviceClient = new QueueServiceClient(jobContext.HealthCheck.ConnectionString);
            
            if (!string.IsNullOrEmpty(jobContext.HealthCheck.QueueName))
            {
                // Note: PoLP (Principle of least privilege)
                // This can be used having at least the role assignment "Storage Queue Data Reader" at container level or at least "Storage Queue Data Reader" at storage account level.
                // See https://learn.microsoft.com/en-us/rest/api/storageservices/get-queue-metadata#authorization.
                var queueClient = HealthChecksClientCache.GetOrAdd(
                        ClientCacheKeys.AzureQueueStorage(jobContext.HealthCheck.QueueName),
                        _ => serviceClient.GetQueueClient(jobContext.HealthCheck.QueueName)
                    );
                
                await queueClient
                    .GetPropertiesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            else
            {
                // Note: QueueServiceClient.GetPropertiesAsync() cannot be used with only the role assignment
                // "Storage Queue Data Contributor," so QueueServiceClient.GetQueuesAsync() is used instead to probe service health.
                // However, QueueClient.GetPropertiesAsync() does have sufficient permissions.
                // Note: PoLP (Principle of least privilege)
                // This can be used having at least "Storage Queue Data Reader" at storage account level.
                // See https://learn.microsoft.com/en-us/rest/api/storageservices/get-queue-metadata#authorization.
                await serviceClient
                    .GetQueuesAsync(cancellationToken: cancellationToken)
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