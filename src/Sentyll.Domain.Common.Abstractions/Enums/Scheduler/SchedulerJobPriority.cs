namespace Sentyll.Domain.Common.Abstractions.Enums.Scheduler;

public enum SchedulerJobPriority
{
    /// <summary>
    /// Long running tasks are executed in a separate thread.
    /// Using Default TaskScheduler.
    /// </summary>
    LongRunning,
    /// <summary>
    /// High Priority Tasks are executed first.
    /// Using SentyllTaskScheduler
    /// </summary>
    High,
    /// <summary>
    /// Normal Priority Tasks are executed after high priority tasks.
    /// Using SentyllTaskScheduler
    /// </summary>
    Normal,
    /// <summary>
    /// Low Priority Tasks are executed last.
    /// Using SentyllTaskScheduler
    /// </summary>
    Low
}