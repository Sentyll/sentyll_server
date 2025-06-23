using System.Text.Json;
using Sentyll.Domain.Common.Abstractions.Enums;
using Sentyll.Domain.Common.Abstractions.Models.Definitions.HealthChecks.Payload;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Jobs;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Sentyll.Infrastructure.HealthChecks.SqlServer.Core.Constants;
using Sentyll.Infrastructure.HealthChecks.SqlServer.Core.Models.Definitions;
using Sentyll.Infrastructure.HealthChecks.SqlServer.Services;

namespace Sentyll.Infrastructure.HealthChecks.SqlServer.HealthChecks;

internal sealed class SqlServerDatabaseQueryV1HealthCheck(
    IHealthCheckJobDependencyManager dependencyManager,
    SqlServerConnectionService sqlServerConnectionService
    ) : HealthCheckJob<SqlServerDatabaseQueryV1HealthCheck, SqlServerDatabaseQueryV1Parameters>(
        dependencyManager,
        HealthCheckType.SQLSERVER_DATABASE_QUERY_V1
    )
{
    
    public override async Task<HealthCheckResult> CheckAsync(
        HealthCheckPayloadDefinition<SqlServerDatabaseQueryV1Parameters> jobContext, 
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await sqlServerConnectionService
                .ExecuteQueryAsync(jobContext.HealthCheck.ConnectionString, jobContext.HealthCheck.Query, cancellationToken)
                .ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(jobContext.HealthCheck.ExpectedJson))
            {
                var serializedResult = JsonSerializer.Serialize(result);
                var expectedJsonMatches = string.Equals(serializedResult, jobContext.HealthCheck.ExpectedJson, StringComparison.OrdinalIgnoreCase);
                if (!expectedJsonMatches)
                {
                    return HealthCheckResult.Unhealthy(description: SsConstants.JsonNotEqualMessage(jobContext.HealthCheck.ExpectedJson, serializedResult));
                }
            }

            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return new HealthCheckResult(jobContext.Scheduler.FailureStatus, exception: ex);
        }
    }
    
}