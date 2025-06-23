using Sentyll.Domain.Common.Abstractions.Enums.Scheduler;
using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Delegates;

namespace Sentyll.Infrastructure.Server.Scheduler.Abstractions.Jobs;

public abstract class Job
{
    /// <summary>
    /// Required Identifier used to define a target job "worker" that can be used for multiple jobs
    /// </summary>
    public string JobIdentifier { get; }
    
    /// <summary>
    /// Defines a cron schedule the target Job should run against.
    /// </summary>
    /// <remarks>
    /// Leave empty if you want to dynamically load jobs with custom schedules. <br />
    /// If defined in a Job Constructor it will Register the Job for Invocation when Startup completes.
    /// </remarks>
    public string CronSchedule { get; }
    
    public SchedulerJobPriority Priority { get; }
    
    /// <summary>
    /// Defines the Action that will get invoked when the Job is executed.
    /// </summary>
    public abstract AsyncSchedulerJobInvocationDelegate InvokeFunc { get; }
    
    /// <summary>
    /// Contains Job invocation meta-data for the current Job Invocation.
    /// It's also used to pass Custom Data if the Job has a custom Request Object defined.
    /// </summary>
    public virtual (string jobId, Type requesObj)? RequestType => null;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="jobIdentifier">Used to Identify The Job</param>
    /// <param name="cronSchedule">Schedules the Job on startup with the defined cron schedule</param>
    /// <param name="priority"></param>
    protected Job(
        string jobIdentifier, 
        string? cronSchedule = null, 
        SchedulerJobPriority? priority = null
        )
    {
        JobIdentifier = jobIdentifier;
        CronSchedule = string.IsNullOrWhiteSpace(cronSchedule) ? string.Empty : cronSchedule;
        Priority = priority ?? SchedulerJobPriority.Normal;
    }
}