using Sentyll.Domain.Common.Abstractions.Enums;
using Sentyll.Domain.Common.Abstractions.Models.Definitions.HealthChecks.Payload;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Jobs;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Sentyll.Infrastructure.HealthChecks.SqlServer.Core.Constants;
using Sentyll.Infrastructure.HealthChecks.SqlServer.Core.Models.Definitions;
using Sentyll.Infrastructure.HealthChecks.SqlServer.Services;

namespace Sentyll.Infrastructure.HealthChecks.SqlServer.HealthChecks;

internal sealed class SqlServerDatabaseV1HealthCheck(
    IHealthCheckJobDependencyManager dependencyManager,
    SqlServerConnectionService sqlServerConnectionService
    ) : HealthCheckJob<SqlServerDatabaseV1HealthCheck, SqlServerDatabaseV1Parameters>(
        dependencyManager,
        HealthCheckType.SQLSERVER_DATABASE_V1
    )
{
    
    public override async Task<HealthCheckResult> CheckAsync(
        HealthCheckPayloadDefinition<SqlServerDatabaseV1Parameters> jobContext, 
        CancellationToken cancellationToken)
    {
        try
        {
            await sqlServerConnectionService
                .ExecuteQueryAsync(jobContext.HealthCheck.ConnectionString, SsConstants.DefaultPingSqlQuery, cancellationToken)
                .ConfigureAwait(false);

            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return new HealthCheckResult(jobContext.Scheduler.FailureStatus, exception: ex);
        }
    }
    
}