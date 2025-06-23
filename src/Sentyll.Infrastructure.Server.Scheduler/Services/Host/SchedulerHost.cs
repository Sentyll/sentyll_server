using Sentyll.Infrastructure.Server.Abstractions.Contracts.Services;
using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Services.Jobs;
using Microsoft.Extensions.Logging;
using Sentyll.Infrastructure.Server.Scheduler.Core.Contracts.Services.Host;
using Sentyll.Infrastructure.Server.Scheduler.Core.Models.Cancellation;
using Sentyll.Infrastructure.Server.Scheduler.Core.Models.Options;
using Sentyll.Infrastructure.Server.Scheduler.Services.Jobs;

namespace Sentyll.Infrastructure.Server.Scheduler.Services.Host;

internal partial class SchedulerHost : ISchedulerHost
{
    
    private readonly SchedulerOptions _schedulerOptions;
    private readonly ISystemClock _clock;
    private readonly ILogger<SchedulerHost> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IJobProviderManager _jobProviderManager;
    
    private readonly SchedulerHostTaskScheduler _schedulerHostTaskScheduler;
    private readonly SchedulerHostRestartThrottleManager _schedulerHostRestartThrottle;
    private readonly SemaphoreSlim _semaphoreSlim;
    
    private DateTime? NextPlannedOccurrence { get; set; }
    
    private SafeCancellationTokenSource JobCheckerCts { get; set; }
    
    private SafeCancellationTokenSource JobDelayAwaiterCts { get; set; }
    
    private SafeCancellationTokenSource JobTimeoutCheckerCts { get; set; }
    
    private SafeCancellationTokenSource JobTimeoutDelayAwaiterCts { get; set; }

    public SchedulerHost(
        SchedulerOptions schedulerOptions, 
        ISystemClock clock, 
        ILogger<SchedulerHost> logger, 
        IServiceProvider serviceProvider, 
        ILoggerFactory loggerFactory, 
        IJobProviderManager jobProviderManager
        )
    {
        _schedulerOptions = schedulerOptions ?? throw new ArgumentNullException(nameof(schedulerOptions));
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _jobProviderManager = jobProviderManager ?? throw new ArgumentNullException(nameof(jobProviderManager));
        
        _schedulerHostTaskScheduler = new SchedulerHostTaskScheduler(loggerFactory, schedulerOptions.MaxConcurrency);
        _schedulerHostRestartThrottle = new SchedulerHostRestartThrottleManager(SoftNotifyDelayChange);
        _semaphoreSlim = new SemaphoreSlim(1, 1);
    }
    
    public void Start()
    {
        _logger.LogInformation("Scheduler Host Start requested");
        
        Stop();
        Run();
        _schedulerOptions.NotifyHostRunningFunc(true);
        
        _logger.LogInformation("Scheduler Host Start completed successfully");
    }
    
    public void Stop()
    {
        _logger.LogInformation("Scheduler Host Stop requested");
        
        if (JobTimeoutCheckerTokenActive)
        {
            _logger.LogInformation("Job timeout checker cancellation token source is not disposed, Cancelling now");
            JobTimeoutCheckerCts?.Cancel();
        }

        if (JobCheckerTokenActive)
        {
            _logger.LogInformation("Job checker cancellation token source is not disposed, Cancelling now");
            JobCheckerCts?.Cancel();
        }

        _logger.LogInformation("Releasing Job Cancellation tokens stored in Job Cancellation token manager");
        JobCancellationTokenManager.ClearJobCancellationTokens();
        
        NextPlannedOccurrence = null;
        
        _schedulerOptions.NotifyNextOccurenceFunc(NextPlannedOccurrence);
        _schedulerOptions.NotifyHostRunningFunc(false);
        
        _logger.LogInformation("Scheduler Host Stop completed successfully");
    }
    
    public bool IsRunning()
    {
        return JobCheckerTokenActive;
    }
    
    public void Restart()
    {
        _logger.LogInformation("Scheduler Host Restart requested");
        
        if (JobDelayAwaiterCts?.IsDisposed == false)
        {
            _logger.LogInformation("Job delay awaiter cancellation token source is not disposed, Cancelling now");
            JobDelayAwaiterCts.Cancel();
        }

        _schedulerOptions.NotifyHostRunningFunc(IsRunning());
        
        _logger.LogInformation("Scheduler Host Restart completed successfully");
    }
    
    public void RestartIfNeeded(DateTime nextPlannedOccurrence)
    {
        var restartRequired = NextPlannedOccurrence == null ||
                              (NextPlannedOccurrence.Value - nextPlannedOccurrence).TotalSeconds >= 1;
        
        _logger.LogInformation("Scheduler Host restart Throttle requires restart [{restartRequired}] based on current next planned occurence : {NextPlannedOccurrence}",
            restartRequired, NextPlannedOccurrence);
        
        if (restartRequired)
        {
            _schedulerHostRestartThrottle.RequestRestart();
        }
    }
}