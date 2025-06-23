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

internal sealed class AzureServiceBusTopicV1HealthCheck(
        IHealthCheckJobDependencyManager dependencyManager
    ) : HealthCheckJob<AzureServiceBusTopicV1HealthCheck, AzureServiceBusTopicV1Parameters>(
        dependencyManager,
        HealthCheckType.AZURE_SERVICEBUS_TOPIC_V1
    )
{
    
    public override async Task<HealthCheckResult> CheckAsync(
        HealthCheckPayloadDefinition<AzureServiceBusTopicV1Parameters> jobContext, 
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

            _ = await managementClient
                .GetTopicRuntimePropertiesAsync(jobContext.HealthCheck.TopicName, cancellationToken)
                .ConfigureAwait(false);

            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return new HealthCheckResult(jobContext.Scheduler.FailureStatus, exception: ex);
        }
    }
    
}
