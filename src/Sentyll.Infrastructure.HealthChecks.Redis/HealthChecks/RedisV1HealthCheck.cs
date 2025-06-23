using Sentyll.Domain.Common.Abstractions.Enums;
using Sentyll.Domain.Common.Abstractions.Models.Definitions.HealthChecks.Payload;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Constants.Storage.Cache;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Jobs;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Services;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Storage.Cache;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Sentyll.Infrastructure.HealthChecks.Redis.Core.Constants;
using Sentyll.Infrastructure.HealthChecks.Redis.Core.Models.Definitions;
using StackExchange.Redis;

namespace Sentyll.Infrastructure.HealthChecks.Redis.HealthChecks;

internal sealed class RedisV1HealthCheck(
    IHealthCheckJobDependencyManager dependencyManager
    ) : HealthCheckJob<RedisV1HealthCheck, RedisV1Parameters>(
        dependencyManager, 
        HealthCheckType.REDIS_V1
    )
{

    public override async Task<HealthCheckResult> CheckAsync(
        HealthCheckPayloadDefinition<RedisV1Parameters> jobContext, 
        CancellationToken cancellationToken)
    {
        var redisConnectionString = jobContext.HealthCheck.ConnectionString;
        
        try
        {
            var connection = await HealthChecksClientCache.GetOrAddAsync(
                ClientCacheKeys.Redis(redisConnectionString),
                async (_) =>
                {
                    var connectionMultiplexerTask = ConnectionMultiplexer.ConnectAsync(redisConnectionString);
                    var connection = await TimeoutAsync(connectionMultiplexerTask, cancellationToken).ConfigureAwait(false);
                    
                    // Dispose new connection which we just created, because we don't need it.
                    await connection.DisposeAsync();
                    
                    return connection;
                }
            );

            foreach (var endPoint in connection.GetEndPoints(configuredOnly: true))
            {
                var server = connection.GetServer(endPoint);

                if (server.ServerType != ServerType.Cluster)
                {
                    await connection
                        .GetDatabase()
                        .PingAsync()
                        .ConfigureAwait(false);
                    
                    await server
                        .PingAsync()
                        .ConfigureAwait(false);
                }
                else
                {
                    var clusterInfo = await server
                        .ExecuteAsync(RdsConstants.DefaultCommand, RdsConstants.DefaultCommandArgs)
                        .ConfigureAwait(false);

                    if (clusterInfo is object && !clusterInfo.IsNull)
                    {
                        if (!clusterInfo.ToString().Contains(RdsConstants.ClusterValidState))
                        {
                            //cluster info is not ok!
                            return new HealthCheckResult(jobContext.Scheduler.FailureStatus, description: RdsConstants.ClusterNotOkMessage(endPoint));
                        }
                    }
                    else
                    {
                        //cluster info cannot be read for this cluster node
                        return new HealthCheckResult(jobContext.Scheduler.FailureStatus, description: RdsConstants.ClusterNullMessage(endPoint));
                    }
                }
            }

            return HealthCheckResult.Healthy();
        }
        catch (OperationCanceledException)
        {
            return new HealthCheckResult(jobContext.Scheduler.FailureStatus, description: RdsConstants.TimeoutMessage);
        }
        catch (Exception ex)
        {
            if (HealthChecksClientCache.TryRemove<ConnectionMultiplexer>(redisConnectionString, out var connection))
            {
                await connection!.DisposeAsync();
            }
            
            return new HealthCheckResult(jobContext.Scheduler.FailureStatus, exception: ex);
        }
    }

    // Remove when https://github.com/StackExchange/StackExchange.Redis/issues/1039 is done
    private static async Task<ConnectionMultiplexer> TimeoutAsync(Task<ConnectionMultiplexer> task, CancellationToken cancellationToken)
    {
        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        
        var completedTask = await Task
            .WhenAny(task, Task.Delay(Timeout.Infinite, timeoutCts.Token))
            .ConfigureAwait(false);

        if (completedTask == task)
        {
            await timeoutCts.CancelAsync();
            return await task.ConfigureAwait(false);
        }

        cancellationToken.ThrowIfCancellationRequested();
        throw new OperationCanceledException();
    }
}