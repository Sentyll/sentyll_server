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

internal sealed class AzureServiceBusSubscriptionV1HealthCheck(
        IHealthCheckJobDependencyManager dependencyManager
    ) : HealthCheckJob<AzureServiceBusSubscriptionV1HealthCheck, AzureServiceBusSubscriptionV1Parameters>(
        dependencyManager,
        HealthCheckType.AZURE_SERVICEBUS_SUBSCRIPTION_V1
    )
{
    
    public override async Task<HealthCheckResult> CheckAsync(
        HealthCheckPayloadDefinition<AzureServiceBusSubscriptionV1Parameters> jobContext, 
        CancellationToken cancellationToken)
    {
        try
        {
            if (jobContext.HealthCheck.UsePeekMode)
            {
                await CheckWithReceiverAsync(jobContext, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                await CheckWithManagementAsync(jobContext, cancellationToken).ConfigureAwait(false);
            }

            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return new HealthCheckResult(jobContext.Scheduler.FailureStatus, exception: ex);
        }
    }
    
    private async Task CheckWithReceiverAsync(
        HealthCheckPayloadDefinition<AzureServiceBusSubscriptionV1Parameters> jobContext,
        CancellationToken cancellationToken = default)
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
                _ => client.CreateReceiver(jobContext.HealthCheck.TopicName, jobContext.HealthCheck.SubscriptionName))
            .ConfigureAwait(false);

        await receiver
            .PeekMessageAsync(cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    private async Task CheckWithManagementAsync(
        HealthCheckPayloadDefinition<AzureServiceBusSubscriptionV1Parameters> jobContext,
        CancellationToken cancellationToken = default)
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
            .GetSubscriptionRuntimePropertiesAsync(jobContext.HealthCheck.TopicName, jobContext.HealthCheck.SubscriptionName, cancellationToken)
            .ConfigureAwait(false);
    }
    
}
