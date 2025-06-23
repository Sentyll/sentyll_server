using Sentyll.Domain.Common.Abstractions.Enums.Scheduler;
using Sentyll.Domain.Common.Abstractions.Extensions.CSharpFunctionalExtensions;
using Sentyll.Domain.Data.Abstractions.Entities.Scheduler;
using Sentyll.Domain.Data.Services.Abstractions.Contracts.Repositories.Scheduler;
using Sentyll.Domain.Data.Services.Repositories.Base;

namespace Sentyll.Domain.Data.Services.Repositories.Scheduler;

internal sealed class TimerJobEntityRepository(
    SentyllContext ctx
) : EntityRepository<TimerJobEntity>(ctx), ITimerJobEntityRepository
{

    public async Task<Result<byte[]>> GetRequestAsync(
        Guid id,
        CancellationToken cancellationToken = default)
        => await DbSet
            .Where(x => x.Id == id)
            .Select(x => x.Request)
            .FirstOrDefaultAsync(cancellationToken)
            .AsMaybe()
            .ToResult(SchedulerJobRequestDataFailures.NotFound.ToString())
            .ConfigureAwait(false);

    public async Task<Result<List<TimerJobEntity>>> GetTimedOutUnsafeListAsync(
        DateTime executedTime,
        CancellationToken cancellationToken = default)
        => await DbSet
            .Where(x =>
                (x.Status == SchedulerJobStatus.Idle && x.ExecutionTime.AddSeconds(1) < executedTime) ||
                (x.Status == SchedulerJobStatus.Queued && x.ExecutionTime.AddSeconds(3) < executedTime))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

    public async Task<Maybe<DateTime>> GetEarliestExecutionTimeAsync(
        DateTime currentTime,
        CancellationToken cancellationToken = default)
        => await DbSet
            .AsNoTracking()
            .Where(x => x.LockHolder == null
                        && x.Status == SchedulerJobStatus.Idle
                        && x.ExecutionTime > currentTime)
            .OrderBy(x => x.ExecutionTime)
            .Select(x => x.ExecutionTime)
            .FirstOrDefaultAsync(cancellationToken)
            .AsMaybe()
            .ConfigureAwait(false);

    public async Task<Result<List<TimerJobEntity>>> GetLockedUnsafeListAsync(
        string lockHolder,
        CancellationToken cancellationToken = default)
        => await DbSet
            .Where(x => x.Status == SchedulerJobStatus.Queued || x.Status == SchedulerJobStatus.Inprogress)
            .Where(x => x.LockHolder == lockHolder)
            .ToListAsync(cancellationToken);

    public async Task<Result<List<TimerJobEntity>>> GetNextUnsafeListAsync(
        string lockHolder,
        DateTime roundedMinDate,
        CancellationToken cancellationToken = default)
        => await DbSet
            .Where(x =>
                ((x.LockHolder == null && x.Status == SchedulerJobStatus.Idle) ||
                 (x.LockHolder == lockHolder && x.Status == SchedulerJobStatus.Queued)) &&
                x.ExecutionTime >= roundedMinDate &&
                x.ExecutionTime < roundedMinDate.AddSeconds(1))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    
}