using Sentyll.Domain.Common.Abstractions.Enums;
using Sentyll.Domain.Common.Abstractions.Models.Definitions.HealthChecks.Payload;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Jobs;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Sentyll.Infrastructure.HealthChecks.NpgSql.Core.Constants;
using Sentyll.Infrastructure.HealthChecks.NpgSql.Core.Models.Definitions;
using Sentyll.Infrastructure.HealthChecks.NpgSql.Services;

namespace Sentyll.Infrastructure.HealthChecks.NpgSql.HealthChecks;

internal sealed class NpqSqlDatabaseV1HealthCheck(
    IHealthCheckJobDependencyManager dependencyManager,
    NpqSqlCommandService npqSqlCommandService
    ) : HealthCheckJob<NpqSqlDatabaseV1HealthCheck, NpqSqlDatabaseV1Parameters>(
        dependencyManager,
        HealthCheckType.NPQSQL_DATABASE_V1
    )
{
    
    public override async Task<HealthCheckResult> CheckAsync(
        HealthCheckPayloadDefinition<NpqSqlDatabaseV1Parameters> jobContext, 
        CancellationToken cancellationToken)
    {
        try
        {
            await npqSqlCommandService
                .ExecuteQueryAsync(jobContext.HealthCheck.ConnectionString, NpqSqlConstants.DefaultPingSqlQuery, cancellationToken)
                .ConfigureAwait(false);
            
            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return new HealthCheckResult(jobContext.Scheduler.FailureStatus, description: ex.Message, exception: ex);
        }
    }
}