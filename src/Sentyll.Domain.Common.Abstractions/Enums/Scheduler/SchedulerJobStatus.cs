namespace Sentyll.Domain.Common.Abstractions.Enums.Scheduler;

public enum SchedulerJobStatus
{
    Idle,
    Queued,
    Inprogress,
    Done,
    DueDone,
    Failed,
    Cancelled
}