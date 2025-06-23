using Sentyll.Domain.Data.Abstractions.Entities.Scheduler;
using Sentyll.Domain.Data.Services.Abstractions.Contracts.Repositories.Scheduler;
using Sentyll.Infrastructure.Server.Abstractions.Contracts.Services;
using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Failures;
using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Services.Jobs;
using Sentyll.Infrastructure.Server.Scheduler.Core.Contracts.Services.Host;
using Sentyll.Infrastructure.Server.Scheduler.Core.Models.Dto.CronJob;

namespace Sentyll.Infrastructure.Server.Scheduler.Services.Scheduler;

internal sealed class SchedulerJobStateManager(
    ISchedulerHost schedulerHost,
    ISystemClock systemClock,
    INotificationHub notificationHubSender,
    IJobProviderManager jobProviderManager,
    ICronJobEntityRepository cronJobEntityRepository,
    ITimerJobEntityRepository timerJobEntityRepository,
    ICronJobOccurrenceEntityRepository cronJobOccurrenceEntityRepository)
    : ISchedulerJobStateManager
{

    public async Task<Result> AddCronJobAsync(CronJobEntity entity, CancellationToken cancellationToken = default)
        => await CronSchedule
            .TryGetNextOccurrence(entity.Expression, systemClock.UtcNow)
            .Map((nextOccurrence) =>
            {
                entity.CreatedAt = systemClock.UtcNow;
                entity.UpdatedAt = systemClock.UtcNow;

                return (nextOccurrence, entity);
            })
            .Bind((mapResult) =>
            {
                return cronJobEntityRepository
                    .AddAsync(mapResult.entity, cancellationToken)
                    .Map((_) => mapResult);
            })
            .Tap((mapResult) => schedulerHost.RestartIfNeeded(mapResult.nextOccurrence))
            .Tap(async (mapResult) => await notificationHubSender.NotifyAddCronJobAsync(mapResult.entity.MapToCronJobDto()))
            .ConfigureAwait(false);

    public async Task<Result> AddTimerJobAsync(TimerJobEntity entity, CancellationToken cancellationToken = default)
        => await Result
            .FailureIf(entity.ExecutionTime == default, TimerJobFailures.ExecutionNull)
            .Map(() =>
            {
                entity.CreatedAt = systemClock.UtcNow;
                entity.UpdatedAt = systemClock.UtcNow;
                entity.ExecutionTime = entity.ExecutionTime.ToUniversalTime();

                return entity;
            })
            .Bind((timerJob) =>
            {
                return timerJobEntityRepository
                    .AddAsync(timerJob, cancellationToken)
                    .Map((_) => timerJob);
            })
            .Tap((timerJob) => schedulerHost.RestartIfNeeded(timerJob.ExecutionTime))
            .Tap(async (timerJob) => await notificationHubSender.NotifyAddTimerJobAsync(timerJob.MapToTimerJobDto()))
            .ConfigureAwait(false);

    public async Task<Result> UpdateCronJobAsync(CronJobEntity entity, CancellationToken cancellationToken = default)
        => await cronJobEntityRepository
            .GetAsync(entity.Id, cancellationToken)
            .Map(job =>
            {
                job.Expression = entity.Expression;
                job.Request = entity.Request;
                job.Function = entity.Function;
                job.UpdatedAt = systemClock.UtcNow;

                return new UpdateCronJobFlowDto(job);
            })
            .Bind(flowDto =>
            {
                return ValidateAndGetNextOccurrenceJob(flowDto.Job)
                    .Map(nextOccurrence => flowDto with { NextOccurrence = nextOccurrence });
            })
            .Bind((flowDto) =>
            {
                return cronJobOccurrenceEntityRepository
                    .GetListUnsafeAsync(flowDto.Job.Id, SchedulerJobStatus.Queued, cancellationToken)
                    .Map(nextOccurrences => flowDto with { QueuedOccurences = nextOccurrences });
            })
            .BindIf((flowDto) => flowDto.QueuedOccurences.Count > 0, (flowDto) =>
            {
                cronJobOccurrenceEntityRepository.TrackRemoveRange(flowDto.QueuedOccurences);
                return flowDto with { ForceRestart = true };
            })
            .Tap((flowDto) => cronJobEntityRepository.TrackUpdate(flowDto.Job))
            .Bind((flowDto) =>
            {
                return cronJobEntityRepository
                    .CommitAsync(cancellationToken)
                    .Map(() => flowDto);
            })
            .TapIf((flowDto) => flowDto.ForceRestart, (flowDto) => schedulerHost.RestartIfNeeded(flowDto.NextOccurrence))
            .Tap(async (flowDto) => await notificationHubSender.NotifyUpdateCronJobAsync(flowDto.Job.MapToCronJobDto()))
            .ConfigureAwait(false);

    public async Task<Result> UpdateTimerJobAsync(TimerJobEntity timerJob, CancellationToken cancellationToken = default)
        => await timerJobEntityRepository
            .GetAsync(timerJob.Id, cancellationToken)
            .Map((fetchedJob) =>
            {
                fetchedJob.UpdatedAt = systemClock.UtcNow;
                fetchedJob.ExecutionTime = timerJob.ExecutionTime.ToUniversalTime();

                return fetchedJob;
            })
            .Bind((fetchedJob) => timerJobEntityRepository.UpdateAsync(fetchedJob, cancellationToken))
            .TapIf((fetchedJob) => fetchedJob.Status == SchedulerJobStatus.Queued, () => schedulerHost.RestartIfNeeded(timerJob.ExecutionTime))
            .Tap(async (fetchedJob) => await notificationHubSender.NotifyUpdateTimerJobAsync(fetchedJob.MapToTimerJobDto()))
            .ConfigureAwait(false);

    public async Task<Result> DeleteCronJobAsync(Guid id, CancellationToken cancellationToken = default)
        => await cronJobEntityRepository
            .GetAsync(id, cancellationToken)
            .Bind((cronJob) => cronJobEntityRepository.RemoveAsync(cronJob, cancellationToken))
            .Bind((cronJob) => cronJobOccurrenceEntityRepository.AnyAsync(cronJob.Id, SchedulerJobStatus.Queued, cancellationToken))
            .TapIf((occurrenceExists) => occurrenceExists, () => schedulerHost.Restart())
            .Tap(async () => await notificationHubSender.NotifyRemoveCronJobAsync(id))
            .ConfigureAwait(false);

    public async Task<Result> DeleteTimerJobAsync(Guid id, CancellationToken cancellationToken = default)
        => await timerJobEntityRepository
            .GetAsync(id, cancellationToken)
            .Bind((timerJob) => timerJobEntityRepository.RemoveAsync(timerJob, cancellationToken))
            .TapIf((timerJob) => timerJob.Status == SchedulerJobStatus.Queued, () => schedulerHost.Restart())
            .Tap(async () => await notificationHubSender.NotifyRemoveTimerJobAsync(id))
            .ConfigureAwait(false);
    
    private Result<DateTime> ValidateAndGetNextOccurrenceJob(CronJobEntity entity)
        => Result
            .FailureIf(string.IsNullOrWhiteSpace(entity.Function), CronJobFailures.FunctionNull)
            .Ensure(() => jobProviderManager.JobExists(entity.Function!), CronJobFailures.FunctionNotFound)
            .Bind(() => CronSchedule.GetNextOccurrence(entity.Expression, systemClock.UtcNow));
    
}