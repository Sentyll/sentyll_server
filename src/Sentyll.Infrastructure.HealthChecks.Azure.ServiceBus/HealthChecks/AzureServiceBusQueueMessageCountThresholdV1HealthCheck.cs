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

internal sealed class AzureServiceBusQueueMessageCountThresholdV1HealthCheck(
        IHealthCheckJobDependencyManager dependencyManager
    ) : HealthCheckJob<AzureServiceBusQueueMessageCountThresholdV1HealthCheck, AzureServiceBusQueueMessageCountThresholdV1Parameters>(
        dependencyManager,
        HealthCheckType.AZURE_SERVICEBUS_QUEUE_MESSAGECOUNTTHRESHOLD_V1
    )
{

    public override async Task<HealthCheckResult> CheckAsync(
        HealthCheckPayloadDefinition<AzureServiceBusQueueMessageCountThresholdV1Parameters> jobContext,
        CancellationToken cancellationToken)
    {
        try
        {
            var managementClient = HealthChecksClientCache.GetOrAdd(
                ClientCacheKeys.AzureServiceBus(SbConstants.ManagementClient, jobContext.Id),
                _ => ServiceBusClientExt.CreateManagementClient(
                    fullyQualifiedNamespace: jobContext.HealthCheck.FullyQualifiedNamespace,
                    tenantId: jobContext.HealthCheck.TenantId,
                    clientId: jobContext.HealthCheck.ClientId,
                    clientSecret: jobContext.HealthCheck.ClientSecret
                ));
            
            var properties = await managementClient
                .GetQueueRuntimePropertiesAsync(jobContext.HealthCheck.QueueName, cancellationToken)
                .ConfigureAwait(false);

            var activeQueueHealthStatus = CheckHealthStatus(
                properties.Value.ActiveMessageCount,
                SbConstants.NormalQueueType,
                jobContext.HealthCheck.QueueName,
                jobContext.HealthCheck.ActiveMessagesUnhealthyThreshold,
                jobContext.HealthCheck.ActiveMessagesDegradedThreshold);

            if (activeQueueHealthStatus.Status != HealthStatus.Healthy)
            {
                return activeQueueHealthStatus;
            }

            var deadLetterQueueHealthStatus = CheckHealthStatus(
                properties.Value.DeadLetterMessageCount,
                SbConstants.DeadLetterQueueType,
                jobContext.HealthCheck.QueueName,
                jobContext.HealthCheck.DeadLetterMessagesUnhealthyThreshold,
                jobContext.HealthCheck.DeadLetterMessagesDegradedThreshold);

            if (deadLetterQueueHealthStatus.Status != HealthStatus.Healthy)
            {
                return deadLetterQueueHealthStatus;
            }

            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return new HealthCheckResult(jobContext.Scheduler.FailureStatus, exception: ex);
        }
    }

    private HealthCheckResult CheckHealthStatus(
        long messagesCount,
        string queueType,
        string queueName,
        int? unhealthyThreshold = null,
        int? degradedThreshold = null)
    {
        if (unhealthyThreshold is null && degradedThreshold is null)
        {
            return HealthCheckResult.Healthy();
        }

        if (messagesCount >= unhealthyThreshold)
        {
            return HealthCheckResult.Unhealthy(SbConstants.UnHealthyCountFailureMessage(queueType, queueName, unhealthyThreshold, messagesCount));
        }

        if (messagesCount >= degradedThreshold)
        {
            return HealthCheckResult.Degraded(SbConstants.DegradedCountFailureMessage(queueType, queueName, degradedThreshold, messagesCount));
        }

        return HealthCheckResult.Healthy();
    }
}
