namespace Sentyll.Infrastructure.Server.Scheduler.Abstractions.Services.Notifiers;

public interface IHostStateChangeNotifier
{
    Task NotifyActiveThreads(int activeThreads);
    Task NotifyHostStatus(bool active);
    Task NotifyHostException(string exceptionMessage);
}