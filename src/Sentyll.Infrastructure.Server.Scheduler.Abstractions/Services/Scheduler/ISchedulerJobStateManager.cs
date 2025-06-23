using CSharpFunctionalExtensions;
using Sentyll.Domain.Data.Abstractions.Entities.Scheduler;

namespace Sentyll.Infrastructure.Server.Scheduler.Abstractions.Services.Scheduler;

public interface ISchedulerJobStateManager
{
    Task<Result> AddCronJobAsync(
        CronJobEntity entity,
        CancellationToken cancellationToken = default
    );

    Task<Result> AddTimerJobAsync(
        TimerJobEntity entity,
        CancellationToken cancellationToken = default
    );

    Task<Result> UpdateCronJobAsync(
        CronJobEntity entity,
        CancellationToken cancellationToken = default
    );

    Task<Result> UpdateTimerJobAsync(
        TimerJobEntity timerJob,
        CancellationToken cancellationToken = default
    );

    Task<Result> DeleteCronJobAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );

    Task<Result> DeleteTimerJobAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );
}