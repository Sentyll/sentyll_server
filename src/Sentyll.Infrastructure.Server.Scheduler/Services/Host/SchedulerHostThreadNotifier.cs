using Sentyll.Infrastructure.Server.Scheduler.Core.Models.Options;

namespace Sentyll.Infrastructure.Server.Scheduler.Services.Host;

internal static class SchedulerHostThreadNotifier
{
    
    private static Timer _debounceTimer;
    private static int _latestCount = -1;
    private static int _lastNotified = -1;

    internal static void NotifySafely(int count)
    {
        _latestCount = count;

        _debounceTimer?.Dispose();
        _debounceTimer = new Timer(_ =>
        {
            // Always notify if count is 0 (reset signal)
            if (_latestCount != 0 && _latestCount == _lastNotified)
            {
                return;
            }
            
            _lastNotified = _latestCount;

            try
            {
                SchedulerOptions.NotifyThreadCountFunc(count);
            }
            catch
            {
                //TODO: Log here
            }
        }, null, 100, Timeout.Infinite);
    }
}