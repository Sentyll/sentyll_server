using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Services.Notifiers;
using Microsoft.Extensions.Logging;

namespace Sentyll.Infrastructure.Server.Scheduler.Services.Notifiers;

internal sealed class HostStateChangeLoggerNotifier(
    ILogger<HostStateChangeLoggerNotifier> logger
) : IHostStateChangeNotifier
{
    public Task NotifyActiveThreads(int activeThreads)
    {
        logger.LogInformation("Current Scheduler Host active threads : {activeThreads}", activeThreads);
        return Task.CompletedTask;
    }

    public Task NotifyHostStatus(bool active)
    {
        logger.LogInformation("Current Scheduler Host Active State : {active}", active);
        return Task.CompletedTask;
    }

    public Task NotifyHostException(string exceptionMessage)
    {
        logger.LogError("Scheduler host encountered an exception : {exceptionMessage}", exceptionMessage);
        return Task.CompletedTask;
    }
}