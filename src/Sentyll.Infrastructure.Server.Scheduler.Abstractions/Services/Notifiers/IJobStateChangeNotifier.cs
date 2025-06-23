using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Models.Dto;

namespace Sentyll.Infrastructure.Server.Scheduler.Abstractions.Services.Notifiers;

public interface IJobStateChangeNotifier
{
    Task NotifyAddCronJobAsync(CronJobDto cronJob);
    Task NotifyUpdateCronJobAsync(CronJobDto cronJob);
    Task NotifyRemoveCronJobAsync(Guid id);
    Task NotifyAddTimerJobAsync(TimerJobDto timerJob);
    Task NotifyUpdateTimerJobAsync(TimerJobDto timerJob);
    Task NotifyRemoveTimerJobAsync(Guid id);
    Task NotifyUpdateNextOccurrence(DateTime? nextOccurrence);
    Task NotifyAddCronOccurrenceAsync(Guid jobId, JobOccurrenceDto occurrence);
    Task NotifyUpdateCronOccurrenceAsync(Guid jobId, JobOccurrenceDto occurrence);
    Task NotifyCanceledJobAsync(Guid id);
}