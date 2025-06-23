namespace Sentyll.Infrastructure.Server.Scheduler.Core.Models.Options;

/// <summary>
/// TODO: Rewrite to be used on Server Settings store?
/// </summary>
public class SchedulerOptions
{
    
    internal static int ActiveThreads;
    
    internal int MaxConcurrency { get; private set; } = 0;
    
    internal string InstanceIdentifier { get; private set; }
    
    internal TimeSpan TimeOutChecker { get; private set; } = TimeSpan.FromMinutes(1);

    internal bool CancelMissedJobsOnReset { get; private set; } = false;

    internal static Action<int> NotifyThreadCountFunc { get; private set; } = (_) => { };

    internal Action<DateTime?> NotifyNextOccurenceFunc { get; private set; } = (_) => { };
    
    internal Action<bool> NotifyHostRunningFunc { get; private set; } = (_) => { };
    
    internal Action<string> HostExceptionMessageFunc { get; private set; } = (_) => { };
    
    public void SetMaxConcurrency(int maxConcurrency)
    {
        MaxConcurrency = maxConcurrency;
    }
    
    public void SetInstanceIdentifier(string instanceIdentifier)
    {
        InstanceIdentifier = instanceIdentifier;
    }
    
    public void SetTimeOutJobChecker(TimeSpan timeSpan)
    {
        TimeOutChecker = timeSpan < TimeSpan.FromSeconds(30) 
            ? TimeSpan.FromSeconds(30) 
            : timeSpan;
    }
    
    public void CancelMissedJobsOnApplicationRestart()
    {
        CancelMissedJobsOnReset = true;
    }
    
    internal void SubscribeToHostEvents(
        Action<int> notifyThreadCountFunc,
        Action<DateTime?> notifyNextOccurenceFunc,
        Action<bool> notifyHostRunningFunc,
        Action<string> hostExceptionMessageFunc
    )
    {
        NotifyThreadCountFunc = notifyThreadCountFunc;
        NotifyNextOccurenceFunc = notifyNextOccurenceFunc;
        NotifyHostRunningFunc = notifyHostRunningFunc;
        HostExceptionMessageFunc = hostExceptionMessageFunc;
    }
}