using Sentyll.Domain.Data.Services.Abstractions.Contracts.Repositories.HealthChecks;
using Sentyll.Domain.Data.Services.Repositories.Base;

namespace Sentyll.Domain.Data.Services.Repositories.HealthChecks;

internal sealed class HealthCheckParameterEntityRepository(
    SentyllContext ctx
) : EntityRepository<HealthCheckParameterEntity>(ctx), IHealthCheckParameterEntityRepository
{

}