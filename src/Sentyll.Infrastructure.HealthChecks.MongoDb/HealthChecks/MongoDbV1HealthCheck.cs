using Sentyll.Domain.Common.Abstractions.Enums;
using Sentyll.Domain.Common.Abstractions.Models.Definitions.HealthChecks.Payload;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Jobs;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Bson;
using MongoDB.Driver;
using Sentyll.Infrastructure.HealthChecks.MongoDb.Core.Constants;
using Sentyll.Infrastructure.HealthChecks.MongoDb.Core.Models.Definitions;

namespace Sentyll.Infrastructure.HealthChecks.MongoDb.HealthChecks;

internal sealed class MongoDbV1HealthCheck(
    IHealthCheckJobDependencyManager dependencyManager
    ) : HealthCheckJob<MongoDbV1HealthCheck, MongoDbV1Parameters>(
        dependencyManager, 
        HealthCheckType.MONGODB_V1
    )
{

    private static readonly Lazy<BsonDocumentCommand<BsonDocument>> Command =
        new(() => new(BsonDocument.Parse(MdbConstants.DefaultPingCommand))
        );

    public override async Task<HealthCheckResult> CheckAsync(
        HealthCheckPayloadDefinition<MongoDbV1Parameters> jobContext, 
        CancellationToken cancellationToken)
    {
        try
        {
            var client = new MongoClient(jobContext.HealthCheck.ConnectionString);
            
            if (!string.IsNullOrEmpty(jobContext.HealthCheck.DatabaseName))
            {
                // For most operations where it is possible, the MongoDB driver itself will retry exactly once
                // to cover switches in the primary and temporary short term network outages.
                // Due to the RunCommand being a lower level function, according to the spec (https://github.com/mongodb/specifications/blob/master/source/run-command/run-command.rst#retryability)
                // for it, it is not retryable and this extends to the ping.
                for (int attempt = 1; attempt <= MdbConstants.MaxPingAttempts; attempt++)

                {
                    try
                    {
                        await client
                            .GetDatabase(jobContext.HealthCheck.DatabaseName)
                            .RunCommandAsync(Command.Value, cancellationToken: cancellationToken)
                            .ConfigureAwait(false);
                        
                        break;
                    }
                    catch (OperationCanceledException)
                    {
                        throw;
                    }
                    catch (Exception)
                    {
                        if (MdbConstants.MaxPingAttempts == attempt)
                        {
                            throw;
                        }

                        cancellationToken.ThrowIfCancellationRequested();
                    }
                }
            }
            else
            {
                using var cursor = await client
                    .ListDatabaseNamesAsync(cancellationToken)
                    .ConfigureAwait(false);
                
                await cursor
                    .FirstOrDefaultAsync(cancellationToken)
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