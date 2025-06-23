using Sentyll.Domain.Data.Services.Abstractions.Contracts.Repositories.HealthChecks;
using Sentyll.Domain.Data.Services.Repositories.Base;

namespace Sentyll.Domain.Data.Services.Repositories.HealthChecks;

/// <inheritdoc cref="IHealthCheckEntityRepository"/>
internal sealed class HealthCheckEntityRepository(
    SentyllContext ctx
) : EntityRepository<HealthCheckEntity>(ctx), IHealthCheckEntityRepository
{
    
}