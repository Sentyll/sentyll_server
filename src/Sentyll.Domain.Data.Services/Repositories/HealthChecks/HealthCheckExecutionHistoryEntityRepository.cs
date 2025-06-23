using Sentyll.Domain.Data.Services.Abstractions.Contracts.Repositories.HealthChecks;
using Sentyll.Domain.Data.Services.Repositories.Base;

namespace Sentyll.Domain.Data.Services.Repositories.HealthChecks;

internal sealed class HealthCheckExecutionHistoryEntityRepository(
    SentyllContext ctx
) : EntityRepository<HealthCheckExecutionHistoryEntity>(ctx), IHealthCheckExecutionHistoryEntityRepository
{

}