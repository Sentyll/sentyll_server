using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Models.Dto;
using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Services.Notifiers;
using Microsoft.Extensions.Logging;

namespace Sentyll.Infrastructure.Server.Scheduler.Services.Notifiers;

internal sealed class NotificationHub(
    IEnumerable<IHostStateChangeNotifier> hostStateChangeNotifiers,
    IEnumerable<IJobStateChangeNotifier> jobStateChangeNotifiers,
    ILogger<NotificationHub> logger
    ) : INotificationHub
{
    
    public async Task NotifyActiveThreads(int activeThreads)
        => await TryNotifyAsync(hostStateChangeNotifiers, notifier => notifier.NotifyActiveThreads(activeThreads));

    public async Task NotifyHostStatus(bool active)
        => await TryNotifyAsync(hostStateChangeNotifiers, notifier => notifier.NotifyHostStatus(active));

    public async Task NotifyHostException(string exceptionMessage)
        => await TryNotifyAsync(hostStateChangeNotifiers, notifier => notifier.NotifyHostException(exceptionMessage));

    public async Task NotifyAddCronJobAsync(CronJobDto cronJob)
        => await TryNotifyAsync(jobStateChangeNotifiers, notifier => notifier.NotifyAddCronJobAsync(cronJob));

    public async Task NotifyUpdateCronJobAsync(CronJobDto cronJob)
        => await TryNotifyAsync(jobStateChangeNotifiers, notifier => notifier.NotifyUpdateCronJobAsync(cronJob));

    public async Task NotifyRemoveCronJobAsync(Guid id)
        => await TryNotifyAsync(jobStateChangeNotifiers, notifier => notifier.NotifyRemoveCronJobAsync(id));

    public async Task NotifyAddTimerJobAsync(TimerJobDto timerJob)
        => await TryNotifyAsync(jobStateChangeNotifiers, notifier => notifier.NotifyAddTimerJobAsync(timerJob));

    public async Task NotifyUpdateTimerJobAsync(TimerJobDto timerJob)
        => await TryNotifyAsync(jobStateChangeNotifiers, notifier => notifier.NotifyUpdateTimerJobAsync(timerJob));

    public async Task NotifyRemoveTimerJobAsync(Guid id)
        => await TryNotifyAsync(jobStateChangeNotifiers, notifier => notifier.NotifyRemoveTimerJobAsync(id));

    public async Task NotifyUpdateNextOccurrence(DateTime? nextOccurrence)
        => await TryNotifyAsync(jobStateChangeNotifiers, notifier => notifier.NotifyUpdateNextOccurrence(nextOccurrence));

    public async Task NotifyAddCronOccurrenceAsync(Guid jobId, JobOccurrenceDto occurrence)
        => await TryNotifyAsync(jobStateChangeNotifiers, notifier => notifier.NotifyAddCronOccurrenceAsync(jobId, occurrence));

    public async Task NotifyUpdateCronOccurrenceAsync(Guid jobId, JobOccurrenceDto occurrence)
        => await TryNotifyAsync(jobStateChangeNotifiers, notifier => notifier.NotifyUpdateCronOccurrenceAsync(jobId, occurrence));

    public async Task NotifyCanceledJobAsync(Guid id)
        => await TryNotifyAsync(jobStateChangeNotifiers, notifier => notifier.NotifyCanceledJobAsync(id));

    private async Task TryNotifyAsync<TNotifier>(
        IEnumerable<TNotifier> notifiers,
        Func<TNotifier, Task> notifyAction)
    {
        foreach (var notifier in notifiers)
        {
            try
            {
                await notifyAction(notifier);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Notifier failed to during invocation");
            }
        }
    }
    
}