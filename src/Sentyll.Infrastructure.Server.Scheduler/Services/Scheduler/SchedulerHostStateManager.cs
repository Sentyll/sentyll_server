using Sentyll.Domain.Data.Abstractions.Entities.Scheduler;
using Sentyll.Domain.Data.Services.Abstractions.Contracts.Repositories.Scheduler;
using Sentyll.Infrastructure.Server.Abstractions.Contracts.Services;
using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Extensions;
using Sentyll.Infrastructure.Server.Scheduler.Core.Contracts.Services.Host;
using Sentyll.Infrastructure.Server.Scheduler.Core.Models.Options;

namespace Sentyll.Infrastructure.Server.Scheduler.Services.Scheduler;

/// <summary>
/// TODO: simplify this service, there is too much logic being written inline in some of these functions, abstract and simplify it.
/// 
/// </summary>
/// <param name="schedulerHost"></param>
/// <param name="systemClock"></param>
/// <param name="schedulerOptions"></param>
/// <param name="notificationHubSender"></param>
/// <param name="cronJobEntityRepository"></param>
/// <param name="timerJobEntityRepository"></param>
/// <param name="cronJobOccurrenceEntityRepository"></param>
internal sealed class SchedulerHostStateManager(
    ISchedulerHost schedulerHost,
    ISystemClock systemClock,
    SchedulerOptions schedulerOptions,
    INotificationHub notificationHubSender,
    ICronJobEntityRepository cronJobEntityRepository,
    ITimerJobEntityRepository timerJobEntityRepository,
    ICronJobOccurrenceEntityRepository cronJobOccurrenceEntityRepository)
    : ISchedulerHostStateManager
{
    
    private readonly string _lockHolder = schedulerOptions.InstanceIdentifier;

    public async Task<Result<T>> GetOccurrenceRequestAsync<T>(
        Guid jobId,
        SchedulerJobType type,
        CancellationToken cancellationToken = default)
        => type == SchedulerJobType.CronExpression
            ? await cronJobOccurrenceEntityRepository
                .GetRequestAsync(jobId, cancellationToken)
                .Bind((request) => request.ReadSchedulerJobRequest<T>())
            : await timerJobEntityRepository
                .GetRequestAsync(jobId, cancellationToken)
                .Bind((request) => request.ReadSchedulerJobRequest<T>());

    public async Task<Result> DeleteJobAsync(Guid jobId, SchedulerJobType type,
        CancellationToken cancellationToken = default)
        => (type == SchedulerJobType.CronExpression)
            ? await cronJobEntityRepository
                .GetAsync(jobId, cancellationToken)
                .Bind((cronJob) => cronJobEntityRepository.RemoveAsync(cronJob, cancellationToken))
                .Tap(async (cronJob) => await notificationHubSender.NotifyRemoveCronJobAsync(cronJob.Id))
                .Tap(() => schedulerHost.Restart())
            : await timerJobEntityRepository
                .GetAsync(jobId, cancellationToken)
                .Bind((timerJob) => timerJobEntityRepository.RemoveAsync(timerJob, cancellationToken))
                .Tap((timerJob) => notificationHubSender.NotifyRemoveTimerJobAsync(timerJob.Id))
                .Tap((timerJob) => schedulerHost.RestartIfNeeded(timerJob.ExecutionTime));

    public async Task<Result<InternalFunctionContext[]>> GetTimedOutFunctionsAsync(CancellationToken cancellationToken = default)
        => await RetrieveTimedOuTimerJobsAsync(cancellationToken)
            .BindZip((timerJobs) => RetrieveTimedOutCronJobsAsync(cancellationToken))
            .RouteBy(
                (functionContexts) => functionContexts.First.Length == 0 && functionContexts.Second.Length == 0,
                (functionContexts) => Result.Success(Array.Empty<InternalFunctionContext>()),
                (functionContexts) => functionContexts.First.Concat(functionContexts.Second).ToArray()
            )
            .ConfigureAwait(false);

    public async Task<Result> SyncJobsWithPersistantStorageAsync(
        IList<(string, string)> cronExpressions,
        CancellationToken cancellationToken = default) 
        => await cronJobEntityRepository
            .GetMemoryJobSeedsListUnsafeAsync(cancellationToken)
            .Map((seededJobs) =>
            {
                var existingFunctions = new HashSet<Guid>();
                var newCronJobs = new List<CronJobEntity>();

                foreach (var (function, expression) in cronExpressions)
                {
                    if ((seededJobs.FirstOrDefault(x => x.Function == function) is { } existingCronJob))
                    {
                        existingFunctions.Add(existingCronJob.Id);

                        if (existingCronJob.Expression == expression)
                            continue;

                        existingCronJob.Expression = expression;
                        existingCronJob.UpdatedAt = systemClock.UtcNow;
                    }
                    else
                    {
                        newCronJobs.Add(new CronJobEntity
                        {
                            Id = Guid.NewGuid(),
                            Function = function,
                            Expression = expression,
                            InitIdentifier = $"MemoryJob_Seeded_{Guid.NewGuid()}",
                            CreatedAt = systemClock.UtcNow,
                            UpdatedAt = systemClock.UtcNow
                        });
                    }
                }

                return (seededJobs, newCronJobs, existingFunctions);
            })
            .Tap((mutationResult) =>
            {
                var nonExistingCronJobs = mutationResult.seededJobs
                    .Where(x => !mutationResult.existingFunctions.Contains(x.Id))
                    .ToList();

                if (nonExistingCronJobs.Any())
                {
                    cronJobEntityRepository.TrackRemoveRange(nonExistingCronJobs);
                }
            })
            .Tap(async (mutationResult) =>
            {
                if (mutationResult.newCronJobs.Count != 0)
                {
                    await cronJobEntityRepository.TrackAddRangeAsync(mutationResult.newCronJobs, cancellationToken);
                }
            })
            .Bind((mutationResult) => cronJobEntityRepository.CommitAsync(cancellationToken))
            .ConfigureAwait(false);

    public async Task<Result> ReleaseOrCancelAllLockedJobsAsync(
        bool terminateExpiredJobs,
        CancellationToken cancellationToken = default)
        => await timerJobEntityRepository
            .GetLockedUnsafeListAsync(_lockHolder, cancellationToken)
            .Map((timerJobs) =>
            {
                foreach (var timeJob in timerJobs)
                {
                    if (timeJob.Status == SchedulerJobStatus.Inprogress ||
                        (terminateExpiredJobs && DateTime.Compare(timeJob.ExecutionTime, systemClock.UtcNow) == 0))
                    {
                        timeJob.Status = SchedulerJobStatus.Cancelled;
                        timeJob.LockedAt = systemClock.UtcNow;
                        timeJob.LockHolder = _lockHolder;
                    }
                    else
                    {
                        timeJob.Status = SchedulerJobStatus.Idle;
                        timeJob.LockedAt = null;
                        timeJob.LockHolder = null;
                    }
                }

                return timerJobs;
            })
            .Tap((timerJobs) => timerJobEntityRepository.TrackUpdateRange(timerJobs))
            .Bind((timerJobs) => cronJobOccurrenceEntityRepository.GetLockedAsNoTrackingUnsafeListAsync(_lockHolder, cancellationToken))
            .Map((cronJobOccurrences) =>
            {
                foreach (var cronJobOccurrence in cronJobOccurrences)
                {
                    if (cronJobOccurrence.Status == SchedulerJobStatus.Inprogress ||
                        (terminateExpiredJobs && DateTime.Compare(cronJobOccurrence.ExecutionTime, systemClock.UtcNow) > 0))
                    {
                        cronJobOccurrence.Status = SchedulerJobStatus.Cancelled;
                        cronJobOccurrence.LockedAt = systemClock.UtcNow;
                        cronJobOccurrence.LockHolder = _lockHolder;
                    }
                    else
                    {
                        cronJobOccurrence.Status = SchedulerJobStatus.Idle;
                        cronJobOccurrence.LockedAt = null;
                        cronJobOccurrence.LockHolder = null;
                    }
                }

                return cronJobOccurrences;
            })
            .Tap((cronJobOccurrences) => cronJobOccurrenceEntityRepository.TrackUpdateRange(cronJobOccurrences))
            .Bind((cronJobOccurrences) => cronJobOccurrenceEntityRepository.CommitAsync(cancellationToken))
            .ConfigureAwait(false);
    
    public Task<Result> UpdateJobRetriesAsync(InternalFunctionContext context, CancellationToken cancellationToken = default) 
        => UpdateJobsAsync([context],
            cronJobOccurrence =>
            {
                cronJobOccurrence.RetryCount = context.RetryCount;
            },
            timeJob =>
            {
                timeJob.RetryCount = context.RetryCount;
            },
            null, cancellationToken);
    
    public Task<Result> SetJobsInprogressAsync(
        InternalFunctionContext[] resources,
        CancellationToken cancellationToken = default) 
        => UpdateJobsAsync(resources,
            cronOccurrence =>
            {
                cronOccurrence.Status = SchedulerJobStatus.Inprogress;
            },
            timeJob =>
            {
                timeJob.Status = SchedulerJobStatus.Inprogress;
            }, null, cancellationToken);

    public Task<Result> ReleaseLockedJobsAsync(InternalFunctionContext[] resources,
        CancellationToken cancellationToken = default) 
        => UpdateJobsAsync(resources,
            cronOccurrence =>
            {
                cronOccurrence.LockHolder = null;
                cronOccurrence.Status = SchedulerJobStatus.Idle;
                cronOccurrence.LockedAt = null;
            },
            timeJob =>
            {
                timeJob.LockHolder = null;
                timeJob.Status = SchedulerJobStatus.Idle;
                timeJob.LockedAt = null;
            }, null, cancellationToken);
    
    public Task<Result> SetJobsStatusAsync(InternalFunctionContext context, CancellationToken cancellationToken = default) 
        => UpdateJobsAsync([context],
            cronOccurrence =>
            {
                cronOccurrence.Status = context.Status;
                cronOccurrence.ExecutedAt = systemClock.UtcNow;

                if (!string.IsNullOrEmpty(context.ExceptionDetails))
                    cronOccurrence.Exception = context.ExceptionDetails;

                if (context.ElapsedTime > 0)
                    cronOccurrence.ElapsedTime = context.ElapsedTime;

                if (context.RetryCount > 0)
                    cronOccurrence.RetryCount = context.RetryCount;
            },
            timeJob =>
            {
                if (!string.IsNullOrEmpty(context.ExceptionDetails))
                    timeJob.Exception = context.ExceptionDetails;

                if (context.ElapsedTime > 0)
                    timeJob.ElapsedTime = context.ElapsedTime;

                if (context.RetryCount > 0)
                    timeJob.RetryCount = context.RetryCount;

                timeJob.Status = context.Status;
                timeJob.ExecutedAt = systemClock.UtcNow;
            },
            null, cancellationToken);
    
    private async Task<Result<InternalFunctionContext[]>> RetrieveTimedOutCronJobsAsync(
        CancellationToken cancellationToken)
        => await cronJobOccurrenceEntityRepository
            .GetTimedOutCronJobsOccurrencesAsync(systemClock.UtcNow.AddSeconds(1), cancellationToken)
            .RouteBy(
                (cronJobOccurrences) => cronJobOccurrences.Any(),
                async (cronJobOccurrences) =>
                {
                    foreach (var occurrence in cronJobOccurrences)
                    {
                        occurrence.Status = SchedulerJobStatus.Inprogress;
                        occurrence.LockHolder = _lockHolder;
                    }

                    return await cronJobOccurrenceEntityRepository
                        .UpdateRangeAsync(cronJobOccurrences, cancellationToken)
                        .Tap(async () =>
                        {
                            foreach (var updatedOccurrence in cronJobOccurrences)
                            {
                                await notificationHubSender
                                    .NotifyUpdateCronOccurrenceAsync(updatedOccurrence.CronJobId,
                                        updatedOccurrence.MapToJobOccurrenceDto())
                                    .ConfigureAwait(false);
                            }
                        })
                        .Map(() => cronJobOccurrences.MapToInternalFunctionContexts());
                },
                (cronJobOccurrences) => Array.Empty<InternalFunctionContext>()
            )
            .ConfigureAwait(false);

    private async Task<Result<InternalFunctionContext[]>> RetrieveTimedOuTimerJobsAsync(
        CancellationToken cancellationToken)
        => await timerJobEntityRepository
            .GetTimedOutUnsafeListAsync(systemClock.UtcNow, cancellationToken)
            .RouteBy(
                (timerJobs) => timerJobs.Any(),
                async (timerJobs) =>
                {
                    foreach (var timerJob in timerJobs)
                    {
                        timerJob.Status = SchedulerJobStatus.Inprogress;
                        timerJob.LockHolder = _lockHolder;
                    }

                    return await timerJobEntityRepository
                        .UpdateRangeAsync(timerJobs, cancellationToken)
                        .Map(() => timerJobs.MapToInternalFunctionContexts());
                },
                (timerJobs) => Array.Empty<InternalFunctionContext>())
            .ConfigureAwait(false);
    
    private TimeSpan CalculateMinTimeRemaining(
        IGrouping<DateTime, string>? minCronJob, 
        DateTime minTimeJob = default)
    {
        var now = systemClock.UtcNow;
        return minCronJob != null && minTimeJob != default
            ? (minCronJob.Key < minTimeJob ? minCronJob.Key : minTimeJob) - now
            : minCronJob != null
                ? minCronJob.Key - now
                : minTimeJob != default
                    ? minTimeJob - now
                    : Timeout.InfiniteTimeSpan;
    }
    
    private async Task<Result> UpdateJobsAsync(
        InternalFunctionContext[] resources,
        Action<CronJobOccurrenceEntity> cronOccurrenceUpdateAction,
        Action<TimerJobEntity> timeUpdateAction,
        Action<CronJobEntity>? cronUpdateAction,
        CancellationToken cancellationToken = default)
    {
        foreach (var resourceType in resources.GroupBy(x => x.Type))
        {
            var jobIds = resourceType.Select(x => x.JobId).ToArray();
            var updateResult = Result.Success();
            
            if (resourceType.Key == SchedulerJobType.CronExpression)
            {
                updateResult = (cronUpdateAction != null)
                    ? await UpdateCronJobsAsync(jobIds, cronUpdateAction, cancellationToken).ConfigureAwait(false)
                    : await UpdateCronJobOccurrencesAsync(jobIds, cronOccurrenceUpdateAction, cancellationToken).ConfigureAwait(false);
            }
            else if (resourceType.Key == SchedulerJobType.Timer)
            {
                updateResult = await UpdateTimerJobAsync(jobIds, timeUpdateAction, cancellationToken).ConfigureAwait(false);
            }

            if (updateResult.IsFailure)
            {
                return updateResult;
            }
        }

        return await timerJobEntityRepository
            .CommitAsync(cancellationToken)
            .Tap(() => timerJobEntityRepository.DetachChangeTrackerEntries())
            .ConfigureAwait(false);
    }

    private async Task<Result> UpdateCronJobsAsync(
        Guid[] cronJobIds,
        Action<CronJobEntity> cronUpdateAction,
        CancellationToken cancellationToken = default)
        => await cronJobEntityRepository
            .GetListUnsafeAsync(cronJobIds, cancellationToken)
            .Map(async (cronJobs) =>
            {
                foreach (var cronJob in cronJobs)
                {
                    cronUpdateAction(cronJob);
                    await notificationHubSender.NotifyUpdateCronJobAsync(cronJob.MapToCronJobDto());
                }

                return cronJobs;
            })
            .Tap((cronJobs) => cronJobEntityRepository.TrackUpdateRange(cronJobs))
            .ConfigureAwait(false);
    
    private async Task<Result> UpdateCronJobOccurrencesAsync(
        Guid[] cronOccurrenceIds,
        Action<CronJobOccurrenceEntity> cronOccurrenceUpdateAction,
        CancellationToken cancellationToken = default)
        => await cronJobOccurrenceEntityRepository
            .GetListUnsafeAsync(cronOccurrenceIds, cancellationToken)
            .Map(async (cronOccurrences) =>
            {
                foreach (var cronOccurrence in cronOccurrences)
                {
                    cronOccurrenceUpdateAction(cronOccurrence);
                    await notificationHubSender.NotifyUpdateCronOccurrenceAsync(cronOccurrence.CronJobId, cronOccurrence.MapToJobOccurrenceDto());
                }

                return cronOccurrences;
            })
            .Tap((cronOccurrences) => cronJobOccurrenceEntityRepository.TrackUpdateRange(cronOccurrences))
            .ConfigureAwait(false);
    
    private async Task<Result> UpdateTimerJobAsync(
        Guid[] timeJobIds,
        Action<TimerJobEntity> timeUpdateAction,
        CancellationToken cancellationToken)
        => await timerJobEntityRepository
            .GetListUnsafeAsync(timeJobIds, cancellationToken)
            .Map(async (timerJobs) =>
            {
                foreach (var timeJob in timerJobs)
                {
                    timeUpdateAction(timeJob);
                    await notificationHubSender.NotifyUpdateTimerJobAsync(timeJob.MapToTimerJobDto());
                }

                return timerJobs;
            })
            .Tap((timerJobs) => timerJobEntityRepository.TrackUpdateRange(timerJobs))
            .ConfigureAwait(false);
    
    private async Task<Result<InternalFunctionContext[]>> RetrieveNexTimerJobEntitysAsync(
        DateTime minDate,
        CancellationToken cancellationToken = default) 
        => await timerJobEntityRepository
            .GetNextUnsafeListAsync(
                _lockHolder, 
                new DateTime(minDate.Ticks - (minDate.Ticks % TimeSpan.TicksPerSecond), DateTimeKind.Utc), 
                cancellationToken
            )
            .Map((timerJobs) =>
            {
                var now = systemClock.UtcNow;

                foreach (var timeJob in timerJobs)
                {
                    timeJob.Status = SchedulerJobStatus.Queued;
                    timeJob.LockHolder = _lockHolder;
                    timeJob.LockedAt = now;
                }

                return timerJobs;
            })
            .BindIf((timerJobs) => (timerJobs.Count > 0),
                async (timerJobs) =>
                {
                    return await timerJobEntityRepository
                        .CommitAsync(cancellationToken)
                        .Map(() => timerJobs)
                        .ConfigureAwait(false);
                }
            )
            .TapIf((timerJobs) => (timerJobs.Count > 0),
                async (timerJobs) =>
                {
                    foreach (var functionCtx in timerJobs)
                    {
                        await notificationHubSender.NotifyUpdateTimerJobAsync(functionCtx.MapToTimerJobDto());
                    }
                })
            .TapIf((timerJobs) => (timerJobs.Count > 0), () => timerJobEntityRepository.DetachChangeTrackerEntries())
            .Map((timerJobs) => timerJobs.MapToInternalFunctionContexts())
            .ConfigureAwait(false);
    
    private async Task<Maybe<IGrouping<DateTime, string>>> GetEarliesCronJobEntityGroupAsync(
        CancellationToken cancellationToken = default)
    {
        var now = systemClock.UtcNow;
        var expressionsResult = await cronJobEntityRepository
            .GetDistinctExpressionsListUnsafeAsync(cancellationToken)
            .ConfigureAwait(false);

        if (expressionsResult.IsFailure)
        {
            return Maybe<IGrouping<DateTime, string>>.None;
        }
        
        return expressionsResult.Value
            .Select(expr => new
            {
                Expression = expr,
                Next = CronSchedule.TryGetNextOccurrence(expr, now)
            })
            .Where(x => x.Next.IsSuccess)
            .GroupBy(x => x.Next!.Value)
            .OrderBy(x => x.Key)
            .FirstOrDefault()
            .AsMaybe()
            .Map(grouping =>
            {
                return grouping
                    .Select(x => x.Expression)
                    .GroupBy(_ => grouping.Key)
                    .First(); //use FirstOrDefault
            });
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    

    public async Task<Result<(TimeSpan TimeRemaining, InternalFunctionContext[] Functions)>> GetNextJobsAsync(
        CancellationToken cancellationToken = default)
    {
        var minCronGroup = await GetEarliesCronJobEntityGroupAsync(cancellationToken)
            .ConfigureAwait(false);
        
        var minTimeJob = await timerJobEntityRepository
            .GetEarliestExecutionTimeAsync(systemClock.UtcNow, cancellationToken)
            .ConfigureAwait(false);

        var minTimeRemaining = CalculateMinTimeRemaining(
            minCronGroup.GetValueOrDefault(),
            minTimeJob.GetValueOrDefault()
        );

        if (minTimeRemaining == Timeout.InfiniteTimeSpan)
            return (Timeout.InfiniteTimeSpan, []);

        return await RetrieveEligibleJobsAsync(
                minCronGroup.GetValueOrDefault(),
                minTimeJob.GetValueOrDefault(),
                cancellationToken
            )
            .Map((nextJobs) => (minTimeRemaining, nextJobs))
            .ConfigureAwait(false);
    }

    private async Task<Result<InternalFunctionContext[]>> RetrieveEligibleJobsAsync(
        IGrouping<DateTime, string>? minCronJob, 
        DateTime minTimeJob,
        CancellationToken cancellationToken = default)
    {
        var hasValidCronJob = minCronJob != null;
        var hasValidTimeJob = minTimeJob != default;

        var areCloseInTime = hasValidCronJob && hasValidTimeJob && Math.Abs((minTimeJob - minCronJob!.Key).TotalSeconds) == 0;
        if (areCloseInTime)
        {
            return  await RetrieveNexCronJobEntitysAsync(minCronJob!.ToArray(), cancellationToken)
                .BindZip((cronJobs) => RetrieveNexTimerJobEntitysAsync(minTimeJob, cancellationToken))
                .Map((zipResult) => zipResult.First.Union(zipResult.Second).ToArray())
                .ConfigureAwait(false);
        }

        if (!hasValidCronJob)
            return await RetrieveNexTimerJobEntitysAsync(minTimeJob, cancellationToken).ConfigureAwait(false);

        if (!hasValidTimeJob)
            return await RetrieveNexCronJobEntitysAsync(minCronJob!.ToArray(), cancellationToken).ConfigureAwait(false);

        return minTimeJob < minCronJob!.Key
            ? await RetrieveNexTimerJobEntitysAsync(minTimeJob, cancellationToken).ConfigureAwait(false)
            : await RetrieveNexCronJobEntitysAsync(minCronJob.ToArray(), cancellationToken).ConfigureAwait(false);
    }

    private async Task<Result<InternalFunctionContext[]>> RetrieveNexCronJobEntitysAsync(
        string[] expressions,
        CancellationToken cancellationToken = default)
    {
        var now = systemClock.UtcNow;

        return await cronJobEntityRepository
            .GetAsNoTrackingListUnsafeAsync(expressions, cancellationToken)
            .BindZip(async (cronJobs) =>
            {
                var cronJobIdSet = cronJobs.Select(t => t.Id).ToArray();

                return await cronJobOccurrenceEntityRepository
                    .GetNextOccurrencesForJobsAsync(cronJobIdSet, _lockHolder, cancellationToken)
                    .ConfigureAwait(false);
            })
            .Map(async (jobsZipResult) =>
            {
                var result = new List<InternalFunctionContext>();

                foreach (var cronJob in jobsZipResult.First)
                {
                    var nextOccurrence = CronSchedule.GetNextOccurrence(cronJob.Expression, now);

                    var existing = jobsZipResult.Second.FirstOrDefault(x =>
                        x.CronJobId == cronJob.Id &&
                        x.ExecutionTime == nextOccurrence.Value);

                    if (existing != null)
                    {
                        existing.Status = SchedulerJobStatus.Queued;
                        existing.LockHolder = _lockHolder;
                        existing.LockedAt = now;

                        result.Add(cronJob.MapToInternalFunctionContext(existing));

                        await notificationHubSender.NotifyUpdateCronOccurrenceAsync(cronJob.Id, existing.MapToJobOccurrenceDto());
                    }
                    else
                    {
                        var newOccurrence = new CronJobOccurrenceEntity()
                        {
                            Id = Guid.NewGuid(),
                            Status = SchedulerJobStatus.Queued,
                            ExecutionTime = nextOccurrence.Value,
                            LockedAt = now,
                            LockHolder = _lockHolder,
                            CronJobId = cronJob.Id
                        };

                        await cronJobOccurrenceEntityRepository.TrackAddAsync(newOccurrence, cancellationToken);

                        result.Add(new InternalFunctionContext
                        {
                            FunctionName = cronJob.Function,
                            JobId = newOccurrence.Id,
                            Type = SchedulerJobType.CronExpression,
                            Retries = cronJob.Retries,
                            RetryIntervals = cronJob.RetryIntervals
                        });

                        await notificationHubSender.NotifyAddCronOccurrenceAsync(cronJob.Id, newOccurrence.MapToJobOccurrenceDto());
                    }
                }

                return result;
            })
            .TapIf(ctxResult => ctxResult.Count > 0, () => cronJobOccurrenceEntityRepository.CommitAsync(cancellationToken))
            .Tap(() =>  cronJobOccurrenceEntityRepository.DetachChangeTrackerEntries())
            .Map(ctxResult => ctxResult.ToArray())
            .ConfigureAwait(false);
    }
    
}