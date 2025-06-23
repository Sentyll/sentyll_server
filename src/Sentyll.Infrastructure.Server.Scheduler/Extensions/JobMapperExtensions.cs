using Sentyll.Domain.Data.Abstractions.Entities.Scheduler;
using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Models.Dto;

namespace Sentyll.Infrastructure.Server.Scheduler.Extensions;

internal static class JobMapperExtensions
{
    public static CronJobDto MapToCronJobDto(this CronJobEntity entity)
        => new(
            entity.Id,
            entity.Function,
            entity.CreatedAt,
            entity.UpdatedAt,
            entity.Expression,
            string.Empty,
            entity.Description ?? string.Empty,
            entity.Retries,
            entity.RetryIntervals,
            entity.InitIdentifier ?? string.Empty
        );

    public static TimerJobDto MapToTimerJobDto(this TimerJobEntity entity)
        => new(
            entity.Id,
            entity.Function,
            entity.CreatedAt,
            entity.UpdatedAt,
            entity.Description,
            entity.Status,
            entity.LockHolder,
            entity.ExecutionTime,
            entity.LockedAt,
            entity.ExecutedAt,
            string.Empty,
            entity.Exception,
            entity.ElapsedTime,
            entity.Retries,
            entity.RetryCount,
            entity.RetryIntervals,
            entity.InitIdentifier ?? string.Empty
        );

    public static TimerJobDto MapToTimerJobDto(this InternalFunctionContext context)
        => new(
            context.JobId,
            context.FunctionName,
            null,
            null,
            string.Empty,
            context.Status,
            string.Empty,
            null,
            null,
            null,
            string.Empty,
            context.ExceptionDetails,
            context.ElapsedTime,
            context.Retries,
            context.RetryCount,
            context.RetryIntervals,
            string.Empty
        );

    public static JobOccurrenceDto MapToJobOccurrenceDto(this CronJobOccurrenceEntity entity)
        => new(
            entity.Id,
            entity.Status,
            entity.LockHolder,
            entity.ExecutionTime,
            entity.CronJobId,
            entity.LockedAt,
            entity.ExecutedAt,
            entity.Exception,
            entity.ElapsedTime,
            entity.RetryCount
        );
    
    public static InternalFunctionContext MapToInternalFunctionContext(this CronJobOccurrenceEntity cronJobOccurrence)
        => new ()
        {
            FunctionName = cronJobOccurrence.CronJob.Function!,
            JobId = cronJobOccurrence.Id,
            Type = SchedulerJobType.CronExpression,
            Retries = cronJobOccurrence.CronJob.Retries,
            RetryIntervals = cronJobOccurrence.CronJob.RetryIntervals
        };

    public static InternalFunctionContext[] MapToInternalFunctionContexts(this List<CronJobOccurrenceEntity> cronJobOccurrences)
        => cronJobOccurrences.Select(MapToInternalFunctionContext).ToArray();
    
    public static InternalFunctionContext MapToInternalFunctionContext(this TimerJobEntity timerJob)
        => new ()
        {
            FunctionName = timerJob.Function!,
            JobId = timerJob.Id,
            Type = SchedulerJobType.Timer,
            Retries = timerJob.Retries,
            RetryIntervals = timerJob.RetryIntervals
        };

    public static InternalFunctionContext[] MapToInternalFunctionContexts(this List<TimerJobEntity> timerJobs)
        => timerJobs.Select(MapToInternalFunctionContext).ToArray();
    
    public static InternalFunctionContext MapToInternalFunctionContext(this CronJobEntity cronJob, CronJobOccurrenceEntity occurrence)
        => new ()
        {
            FunctionName = cronJob.Function!,
            JobId = occurrence.Id,
            Type = SchedulerJobType.CronExpression,
            Retries = cronJob.Retries,
            RetryIntervals = cronJob.RetryIntervals
        };

}