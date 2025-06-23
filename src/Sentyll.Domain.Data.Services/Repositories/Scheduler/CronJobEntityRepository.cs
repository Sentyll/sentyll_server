using Sentyll.Domain.Data.Abstractions.Entities.Scheduler;
using Sentyll.Domain.Data.Services.Abstractions.Contracts.Repositories.Scheduler;
using Sentyll.Domain.Data.Services.Repositories.Base;

namespace Sentyll.Domain.Data.Services.Repositories.Scheduler;

internal sealed class CronJobEntityRepository(
    SentyllContext ctx
) : EntityRepository<CronJobEntity>(ctx), ICronJobEntityRepository
{

    public async Task<Result<List<CronJobEntity>>> GetAsNoTrackingListUnsafeAsync(
        string[] expressions,
        CancellationToken cancellationToken = default)
        => await DbSet
            .AsNoTracking()
            .Where(x => expressions.Contains(x.Expression))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

    public async Task<Result<List<CronJobEntity>>> GetMemoryJobSeedsListUnsafeAsync(CancellationToken cancellationToken = default)
        => await DbSet
            .Where(x => !string.IsNullOrEmpty(x.InitIdentifier) && x.InitIdentifier.StartsWith("MemoryJob_Seed"))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

    public async Task<Result<List<string?>>> GetDistinctExpressionsListUnsafeAsync(CancellationToken cancellationToken = default)
        => await DbSet
            .AsNoTracking()
            .Select(x => x.Expression)
            .Distinct()
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

}