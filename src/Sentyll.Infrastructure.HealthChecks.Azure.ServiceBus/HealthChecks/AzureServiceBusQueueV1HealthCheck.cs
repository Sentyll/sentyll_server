using Sentyll.Domain.Common.Abstractions.Enums;
using Sentyll.Domain.Common.Abstractions.Models.Definitions.HealthChecks.Payload;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Constants.Storage.Cache;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Jobs;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Services;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Storage.Cache;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Sentyll.Infrastructure.HealthChecks.Azure.ServiceBus.Core.Constants;
using Sentyll.Infrastructure.HealthChecks.Azure.ServiceBus.Core.Models.Definitions;
using Sentyll.Infrastructure.HealthChecks.Azure.ServiceBus.Extensions;

namespace Sentyll.Infrastructure.HealthChecks.Azure.ServiceBus.HealthChecks;

internal sealed class AzureServiceBusQueueV1HealthCheck(
        IHealthCheckJobDependencyManager dependencyManager
    ) : HealthCheckJob<AzureServiceBusQueueV1HealthCheck, AzureServiceBusQueueV1Parameters>(
        dependencyManager,
        HealthCheckType.AZURE_SERVICEBUS_QUEUE_V1
    )
{

    public override async Task<HealthCheckResult> CheckAsync(
        HealthCheckPayloadDefinition<AzureServiceBusQueueV1Parameters> jobContext, 
        CancellationToken cancellationToken)
    {
        try
        {
            if (jobContext.HealthCheck.UsePeekMode)
            {
                await CheckWithReceiver(jobContext, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                await CheckWithManagement(jobContext, cancellationToken).ConfigureAwait(false);
            }

            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return new HealthCheckResult(jobContext.Scheduler.FailureStatus, exception: ex);
        }
    }
    
    private async Task CheckWithReceiver(
        HealthCheckPayloadDefinition<AzureServiceBusQueueV1Parameters> jobContext, 
        CancellationToken cancellationToken)
    {
        var client = await HealthChecksClientCache
            .GetOrAddAsyncDisposableAsync(
                ClientCacheKeys.AzureServiceBus(SbConstants.DefaultClient, jobContext.Id),
                _ => ServiceBusClientExt.CreateClient(
                    fullyQualifiedNamespace: jobContext.HealthCheck.FullyQualifiedNamespace,
                    tenantId: jobContext.HealthCheck.TenantId,
                    clientId: jobContext.HealthCheck.ClientId,
                    clientSecret: jobContext.HealthCheck.ClientSecret
                )
            )
            .ConfigureAwait(false);
        
        var receiver = await HealthChecksClientCache
            .GetOrAddAsyncDisposableAsync(
                ClientCacheKeys.AzureServiceBus(SbConstants.ReceiverClient, jobContext.Id),
                _ => client.CreateReceiver(jobContext.HealthCheck.QueueName))
            .ConfigureAwait(false);
        
        await receiver
            .PeekMessageAsync(cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    private async Task CheckWithManagement(
        HealthCheckPayloadDefinition<AzureServiceBusQueueV1Parameters> jobContext, 
        CancellationToken cancellationToken)
    {
        var managementClient = HealthChecksClientCache.GetOrAdd(
            ClientCacheKeys.AzureServiceBus(SbConstants.ManagementClient, jobContext.Id),
            _ => ServiceBusClientExt.CreateManagementClient(
                fullyQualifiedNamespace: jobContext.HealthCheck.FullyQualifiedNamespace,
                tenantId: jobContext.HealthCheck.TenantId,
                clientId: jobContext.HealthCheck.ClientId,
                clientSecret: jobContext.HealthCheck.ClientSecret
            )
        );
        
        await managementClient
            .GetQueueRuntimePropertiesAsync(jobContext.HealthCheck.QueueName, cancellationToken)
            .ConfigureAwait(false);
    }
    
}
