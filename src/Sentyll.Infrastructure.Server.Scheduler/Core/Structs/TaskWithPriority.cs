using Sentyll.Domain.Common.Abstractions.Enums.Scheduler;

namespace Sentyll.Infrastructure.Server.Scheduler.Core.Structs;

internal readonly struct TaskWithPriority
{
    
    public readonly SchedulerJobPriority Priority;
    public readonly Task Task;

    public TaskWithPriority(Task task, SchedulerJobPriority priority)
    {
        Task = task;
        Priority = priority;
    }
}