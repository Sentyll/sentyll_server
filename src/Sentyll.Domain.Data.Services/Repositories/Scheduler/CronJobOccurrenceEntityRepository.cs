using Sentyll.Domain.Common.Abstractions.Enums.Scheduler;
using Sentyll.Domain.Data.Abstractions.Entities.Scheduler;
using Sentyll.Domain.Data.Services.Abstractions.Contracts.Repositories.Scheduler;
using Sentyll.Domain.Data.Services.Repositories.Base;

namespace Sentyll.Domain.Data.Services.Repositories.Scheduler;

internal sealed class CronJobOccurrenceEntityRepository(
    SentyllContext ctx
) : EntityRepository<CronJobOccurrenceEntity>(ctx), ICronJobOccurrenceEntityRepository
{

    public async Task<Result<bool>> AnyAsync(
        Guid cronJobId,
        SchedulerJobStatus status,
        CancellationToken cancellationToken = default)
        => await DbSet
            .Where(x => x.CronJobId == cronJobId)
            .Where(x => x.Status == status)
            .AnyAsync(cancellationToken)
            .ConfigureAwait(false);

    public async Task<Result<List<CronJobOccurrenceEntity>>> GetListUnsafeAsync(
        Guid cronJobId,
        SchedulerJobStatus status,
        CancellationToken cancellationToken = default)
        => await DbSet
            .Where(x => x.CronJobId == cronJobId)
            .Where(x => x.Status == status)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

    public async Task<Result<byte[]>> GetRequestAsync(
        Guid id,
        CancellationToken cancellationToken = default)
        => await DbSet
            .Include(x => x.CronJob)
            .AsSplitQuery()
            .Where(x => x.Id == id)
            .Select(x => x.CronJob.Request)
            .FirstOrDefaultAsync(cancellationToken)
            .AsMaybe()
            .ToResult(SchedulerJobRequestDataFailures.NotFound.ToString())
            .ConfigureAwait(false);

    public async Task<Result<List<CronJobOccurrenceEntity>>> GetTimedOutCronJobsOccurrencesAsync(
        DateTime executedTime,
        CancellationToken cancellationToken = default)
        => await DbSet
            .Include(x => x.CronJob)
            .AsSplitQuery()
            .Where(x =>
                !x.ExecutedAt.HasValue
                && x.Status != SchedulerJobStatus.Inprogress
                && x.Status != SchedulerJobStatus.Cancelled
            )
            .Where(x => x.ExecutionTime < executedTime)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

    public async Task<Result<List<CronJobOccurrenceEntity>>> GetLockedAsNoTrackingUnsafeListAsync(
        string lockHolder,
        CancellationToken cancellationToken = default)
        => await DbSet
            .AsNoTracking()
            .Where(x => x.Status == SchedulerJobStatus.Queued || x.Status == SchedulerJobStatus.Inprogress)
            .Where(x => x.LockHolder == lockHolder)
            .ToListAsync(cancellationToken);

    public async Task<Result<List<CronJobOccurrenceEntity>>> GetNextOccurrencesForJobsAsync(
        Guid[] cronJobIds,
        string lockHolder,
        CancellationToken cancellationToken = default)
        => await DbSet
            .Where(x =>
                cronJobIds.Contains(x.CronJobId) &&
                ((x.LockHolder == null && x.Status == SchedulerJobStatus.Idle) ||
                 (x.LockHolder == lockHolder && x.Status == SchedulerJobStatus.Queued)))
            .ToListAsync(cancellationToken);

}