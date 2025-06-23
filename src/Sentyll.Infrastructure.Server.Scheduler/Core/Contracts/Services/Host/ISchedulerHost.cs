namespace Sentyll.Infrastructure.Server.Scheduler.Core.Contracts.Services.Host;

internal interface ISchedulerHost
{
    void Start();
    
    void Stop();
    
    bool IsRunning();
    
    void Restart();
    
    void RestartIfNeeded(DateTime newOccurrence);
}