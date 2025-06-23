using System.Collections.Concurrent;
using Sentyll.Domain.Common.Abstractions.Enums.Scheduler;
using Microsoft.Extensions.Logging;
using Sentyll.Infrastructure.Server.Scheduler.Core.Models.Options;
using Sentyll.Infrastructure.Server.Scheduler.Core.Structs;

namespace Sentyll.Infrastructure.Server.Scheduler.Services.Host;

internal sealed class SchedulerHostTaskScheduler : TaskScheduler, IDisposable
{
    
    private const string DefaultThreadNameFormat = "Scheduler thread ({0})";
    
    /// <summary>Cancellation token used for disposal.</summary>
    private readonly CancellationTokenSource _disposeCancellation;

    /// <summary>Whether we're processing tasks on the current thread.</summary>
    private static readonly ThreadLocal<bool> TaskProcessingThread = new();

    /// <summary>The collection of tasks to be executed on our custom threads.</summary>
    private readonly BlockingCollection<Task> _blockingTaskQueue;

    private readonly ConcurrentDictionary<int, TaskWithPriority> _taskDict;

    private readonly ILogger<SchedulerHostTaskScheduler> _logger;
    
    public SchedulerHostTaskScheduler(
        ILoggerFactory loggerFactory,
        int threadCount)
    {
        _logger = loggerFactory.CreateLogger<SchedulerHostTaskScheduler>();
        
        _disposeCancellation = new();
        _blockingTaskQueue = new();
        _taskDict = new();
        
        CreateAndStartThreads(threadCount);
    }

    protected override void QueueTask(Task task)
    {
        if (_disposeCancellation.IsCancellationRequested)
        {
            throw new ObjectDisposedException(nameof(SchedulerHostTaskScheduler), "Cannot queue tasks after the scheduler is disposed.");
        }

        if (task.CreationOptions == TaskCreationOptions.HideScheduler)
        {
            _taskDict.TryAdd(task.Id, new TaskWithPriority(task, SchedulerJobPriority.Normal));
        }
        else
        {
            _blockingTaskQueue.Add(task);
        }
    }

    public void SetQueuedTaskPriority(int taskId, SchedulerJobPriority schedulerJobPriority)
    {
        if (_taskDict.TryGetValue(taskId, out var priority))
        {
            _taskDict[taskId] = new TaskWithPriority(priority.Task, schedulerJobPriority);
        }
    }

    public void ExecutePriorityTasks()
    {
        TaskWithPriority[] tasksSnapshot;
        lock (_taskDict) 
        {
            tasksSnapshot = _taskDict
                .Select(x => x.Value)
                .OrderBy(x => x.Priority)
                .ToArray();
        }

        foreach (var task in tasksSnapshot)
        {
            try
            {
                _blockingTaskQueue.Add(task.Task);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Failed to queue task {Id}: {Message}", task.Task.Id, ex.Message);
            }
        }

        _taskDict.Clear();
    }

    protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
    {
        return TaskProcessingThread.Value && TryExecuteTask(task);
    }

    protected override IEnumerable<Task> GetScheduledTasks()
    {
        return _blockingTaskQueue.ToList();
    }

    public void Dispose()
    {
        _disposeCancellation.Cancel();
    }
    
    private void CreateAndStartThreads(int concurrencyLevel)
    {
        var threads = new Thread[concurrencyLevel];
        for (var i = 0; i < concurrencyLevel; i++)
        {
            threads[i] = new Thread(ThreadBasedDispatchLoop)
            {
                IsBackground = true,
                Name = string.Format(DefaultThreadNameFormat, i)
            };
        }

        foreach (var thread in threads)
        {
            thread.Start();
        }
    }

    private void ThreadBasedDispatchLoop()
    {
        TaskProcessingThread.Value = true;
        try
        {
            while (true)
            {
                try
                {
                    foreach (var task in _blockingTaskQueue.GetConsumingEnumerable(_disposeCancellation.Token))
                    {
                        SchedulerHostThreadNotifier.NotifySafely(Interlocked.Increment(ref SchedulerOptions.ActiveThreads));
                        
                        try
                        {
                            TryExecuteTask(task);
                        }
                        finally
                        {
                            SchedulerHostThreadNotifier.NotifySafely(Interlocked.Decrement(ref SchedulerOptions.ActiveThreads));
                        }
                    }
                }
                catch (ThreadAbortException)
                {
                    if (!Environment.HasShutdownStarted && !AppDomain.CurrentDomain.IsFinalizingForUnload())
                    {
                        Thread.ResetAbort();
                    }
                }
            }
        }
        catch (OperationCanceledException)
        {
        }
        finally
        {
            TaskProcessingThread.Value = false;
        }
    }
}