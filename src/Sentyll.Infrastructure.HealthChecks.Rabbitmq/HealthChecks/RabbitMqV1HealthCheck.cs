using Sentyll.Domain.Common.Abstractions.Enums;
using Sentyll.Domain.Common.Abstractions.Models.Definitions.HealthChecks.Payload;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Constants.Storage.Cache;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Jobs;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Services;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Storage.Cache;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQ.Client;
using Sentyll.Infrastructure.HealthChecks.Rabbitmq.Core.Models.Definitions;

namespace Sentyll.Infrastructure.HealthChecks.Rabbitmq.HealthChecks;

internal sealed class RabbitMqV1HealthCheck(
    IHealthCheckJobDependencyManager dependencyManager
    ) : HealthCheckJob<RabbitMqV1HealthCheck, RabbitMqV1Parameters>(
        dependencyManager, 
        HealthCheckType.RABBITMQ_V1
    )
{
    
    public override async Task<HealthCheckResult> CheckAsync(
        HealthCheckPayloadDefinition<RabbitMqV1Parameters> jobContext, 
        CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = await GetOrSetConnectionAsync(
                jobContext.HealthCheck.ConnectionUri,
                jobContext.HealthCheck.ConnectionTimeout,
                cancellationToken
            );

            await connection.CreateChannelAsync(cancellationToken: cancellationToken);
            
            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return new HealthCheckResult(jobContext.Scheduler.FailureStatus, exception: ex);
        }
    }
    
    private async Task<IConnection> GetOrSetConnectionAsync(
        Uri connectionUri,
        TimeSpan? connectionTimeout,
        CancellationToken cancellationToken = default) 
        => await HealthChecksClientCache.GetOrAddAsync(
            ClientCacheKeys.RabbitMq(connectionUri),
            async (_) =>
            {
                var factory = new ConnectionFactory
                {
                    Uri = connectionUri,
                    AutomaticRecoveryEnabled = true
                };

                if (connectionTimeout.HasValue)
                {
                    factory.RequestedConnectionTimeout = connectionTimeout.Value;
                }
            
                return await factory.CreateConnectionAsync(cancellationToken);
            }
        );
}