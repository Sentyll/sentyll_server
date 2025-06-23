using System.Text.Json;
using Sentyll.Domain.Common.Abstractions.Enums;
using Sentyll.Domain.Common.Abstractions.Models.Definitions.HealthChecks.Payload;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Jobs;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Sentyll.Infrastructure.HealthChecks.NpgSql.Core.Constants;
using Sentyll.Infrastructure.HealthChecks.NpgSql.Core.Models.Definitions;
using Sentyll.Infrastructure.HealthChecks.NpgSql.Services;

namespace Sentyll.Infrastructure.HealthChecks.NpgSql.HealthChecks;

internal sealed class NpqSqlDatabaseQueryV1HealthCheck(
    IHealthCheckJobDependencyManager dependencyManager,
    NpqSqlCommandService npqSqlCommandService
    ) : HealthCheckJob<NpqSqlDatabaseQueryV1HealthCheck, NpqSqlDatabaseQueryV1Parameters>(
        dependencyManager,
        HealthCheckType.NPQSQL_DATABASE_QUERY_V1
    )
{
    
    public override async Task<HealthCheckResult> CheckAsync(
        HealthCheckPayloadDefinition<NpqSqlDatabaseQueryV1Parameters> jobContext, 
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await npqSqlCommandService
                .ExecuteQueryAsync(jobContext.HealthCheck.ConnectionString, jobContext.HealthCheck.Query, cancellationToken)
                .ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(jobContext.HealthCheck.ExpectedJson))
            {
                var serializedResult = JsonSerializer.Serialize(result);
                var expectedJsonMatches = string.Equals(serializedResult, jobContext.HealthCheck.ExpectedJson, StringComparison.OrdinalIgnoreCase);
                if (!expectedJsonMatches)
                {
                    return HealthCheckResult.Unhealthy(description: NpqSqlConstants.JsonNotEqualMessage(jobContext.HealthCheck.ExpectedJson, serializedResult));
                }
            }
            
            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return new HealthCheckResult(jobContext.Scheduler.FailureStatus, description: ex.Message, exception: ex);
        }
    }
}