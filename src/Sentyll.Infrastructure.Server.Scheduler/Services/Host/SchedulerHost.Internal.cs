using System.Diagnostics;
using System.Text.Json;
using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Delegates;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sentyll.Infrastructure.Server.Scheduler.Core.Contracts.Services.Host;
using Sentyll.Infrastructure.Server.Scheduler.Core.Exceptions;
using Sentyll.Infrastructure.Server.Scheduler.Core.Models.Cancellation;
using Sentyll.Infrastructure.Server.Scheduler.Core.Models.Exceptions;
using Sentyll.Infrastructure.Server.Scheduler.Services.Jobs;

namespace Sentyll.Infrastructure.Server.Scheduler.Services.Host;

internal partial class SchedulerHost
{

    private bool JobCheckerTokenActive => JobCheckerCts?.IsDisposed == false;
    private bool JobTimeoutCheckerTokenActive => JobTimeoutCheckerCts?.IsDisposed == false;
    
    private void CancelJobChecker()
        => JobCheckerCts?.Dispose();
    
    private void CancelJobTimeoutChecker() 
        => JobTimeoutCheckerCts?.Dispose();
    
    private static string SerializeException(Exception ex)
    {
        var rootException = ex.GetRootException();
        var stackTrace = new StackTrace(rootException, true);
        var frame = stackTrace.GetFrame(0);
        
        return JsonSerializer.Serialize(new HostExceptionDetails(ex.Message, frame?.ToString()));
    }
    
    private void SoftNotifyDelayChange()
    {
        if (JobDelayAwaiterCts?.IsDisposed == false)
        {
            JobDelayAwaiterCts.Cancel();
        }
    }
    
    private void ResetJobDelayAwaiterCts()
    {
        JobDelayAwaiterCts?.Dispose();
        JobDelayAwaiterCts = SafeCancellationTokenSource.CreateLinked(JobCheckerCts.Token);
    }
    
    private static async Task DeleteJob(
        InternalFunctionContext context, 
        ISchedulerHostStateManager jobHostStateManager, 
        CancellationToken cancellationToken
        )
    {
        var deleteResult = await jobHostStateManager
            .DeleteJobAsync(context.JobId, context.Type, cancellationToken)
            .ConfigureAwait(false);

        if (deleteResult.IsFailure)
        {
            throw new Exception(deleteResult.Error);
        }
        
        throw new TerminateExecutionException();
    }
    
    private void Run()
    {
        if (!_jobProviderManager.JobsExist())
        {
            _logger.LogInformation("No Jobs are available in registered Job Provider Manager, job checking loops will be ignored.");
            return;
        }
        
        _logger.LogInformation("Jobs found in registered Job Provider Manager, Starting Job checking Loops.");
        
        Task.Run(async () =>
        {
            try
            {
                JobCheckerCts = new SafeCancellationTokenSource();
                JobTimeoutCheckerCts = new SafeCancellationTokenSource();

                var jobTask = JobCheckerTokenActive
                    ? StartJobCheckingLoop()
                    : Task.CompletedTask;

                var timeoutTask = JobTimeoutCheckerTokenActive
                    ? StartTimeoutJobCheckingLoop()
                    : Task.CompletedTask;

                await Task.WhenAll(jobTask, timeoutTask).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _schedulerOptions.HostExceptionMessageFunc(SerializeException(ex));
                _schedulerOptions.NotifyNextOccurenceFunc(null);
            }
            finally
            {
                CancelJobChecker();
                CancelJobTimeoutChecker();
                
                using var scope = _serviceProvider.CreateScope();
                var internalJobManager = scope.ServiceProvider.GetRequiredService<ISchedulerHostStateManager>();
                await internalJobManager.ReleaseOrCancelAllLockedJobsAsync(false);
            }
        });
    }
    
    private async Task StartJobCheckingLoop()
    {
        JobDelayAwaiterCts = SafeCancellationTokenSource.CreateLinked(JobCheckerCts.Token);

        using var scope = _serviceProvider.CreateScope();
        var internalJobManager = scope.ServiceProvider.GetRequiredService<ISchedulerHostStateManager>();

        while (!JobCheckerCts.Token.IsCancellationRequested)
        {
            var functions = Array.Empty<InternalFunctionContext>();
            try
            {
                var nextJobsResult = await internalJobManager
                    .GetNextJobsAsync(JobCheckerCts.Token)
                    .ConfigureAwait(false);

                (var timeRemaining, functions) = nextJobsResult.Value;
                
                if (timeRemaining == Timeout.InfiniteTimeSpan)
                {
                    NextPlannedOccurrence = null;
                    _schedulerOptions.NotifyNextOccurenceFunc(NextPlannedOccurrence);
                    await Task.Delay(Timeout.InfiniteTimeSpan, JobDelayAwaiterCts.Token).ConfigureAwait(false);
                }
                else
                {
                    NextPlannedOccurrence = _clock.UtcNow.Add(timeRemaining);
                    _schedulerOptions.NotifyNextOccurenceFunc(NextPlannedOccurrence);

                    var sleepDuration = timeRemaining > TimeSpan.FromDays(1)
                        ? TimeSpan.FromDays(1)
                        : timeRemaining;

                    if (JobDelayAwaiterCts.IsCancellationRequested)
                    {
                        ResetJobDelayAwaiterCts();
                    }

                    if (sleepDuration > TimeSpan.Zero && sleepDuration < TimeSpan.FromMilliseconds(500))
                    {
                        await Task.Delay(sleepDuration, CancellationToken.None).ConfigureAwait(false);
                    }
                    else if (sleepDuration > TimeSpan.Zero)
                    {
                        await Task.Delay(sleepDuration, JobDelayAwaiterCts.Token).ConfigureAwait(false);
                    }

                    if (functions.Length != 0)
                    {
                        var setResult = await internalJobManager
                            .SetJobsInprogressAsync(functions, JobCheckerCts.Token)
                            .ConfigureAwait(false);

                        if (setResult.IsFailure)
                        {
                            throw new Exception(setResult.Error);
                        }
                    }

                    if (functions.Length != 0)
                    {
                        await OnTimerTick(functions, JobCheckerCts.Token);
                    }

                    if (JobDelayAwaiterCts.IsCancellationRequested)
                    {
                        ResetJobDelayAwaiterCts();
                    }
                }
            }
            catch (Exception) when (JobDelayAwaiterCts.IsCancellationRequested)
            {
                if (functions.Length != 0)
                {
                    var releaseResult = await internalJobManager
                        .ReleaseLockedJobsAsync(functions, CancellationToken.None)
                        .ConfigureAwait(false);

                    if (releaseResult.IsFailure)
                    {
                        throw new Exception(releaseResult.Error);
                    }
                }

                if (JobCheckerCts.IsCancellationRequested)
                {
                    return;
                }

                JobDelayAwaiterCts?.Dispose();
                JobDelayAwaiterCts = SafeCancellationTokenSource.CreateLinked(JobCheckerCts.Token);
            }
            catch (Exception ex)
            {
                _schedulerOptions.HostExceptionMessageFunc(SerializeException(ex));
                _schedulerOptions.NotifyHostRunningFunc(false);
                _schedulerOptions.NotifyNextOccurenceFunc(null);

                JobCheckerCts?.Cancel();
            }
        }
    }

    private async Task StartTimeoutJobCheckingLoop()
    {
        JobTimeoutDelayAwaiterCts = SafeCancellationTokenSource.CreateLinked(JobTimeoutCheckerCts.Token);

        using var scope = _serviceProvider.CreateScope();
        var internalJobManager = scope.ServiceProvider.GetRequiredService<ISchedulerHostStateManager>();

        if (_schedulerOptions.TimeOutChecker == Timeout.InfiniteTimeSpan)
        {
            return;
        }

        var delayAwaiter = _schedulerOptions.TimeOutChecker;
        while (!JobTimeoutCheckerCts.Token.IsCancellationRequested)
        {
            try
            {
                var getResult = await internalJobManager
                    .GetTimedOutFunctionsAsync(JobTimeoutCheckerCts.Token)
                    .ConfigureAwait(false);

                if (getResult.IsFailure)
                {
                    throw new Exception(getResult.Error);
                }
                
                if (getResult.Value.Length == 0)
                {
                    await Task.Delay(delayAwaiter, JobTimeoutDelayAwaiterCts.Token).ConfigureAwait(false);
                    delayAwaiter = _schedulerOptions.TimeOutChecker;
                    continue;
                }

                await OnTimerTick(getResult.Value, JobTimeoutCheckerCts.Token, true);
            }
            catch (Exception) when (JobTimeoutDelayAwaiterCts.IsCancellationRequested)
            {
                if (JobTimeoutCheckerCts.IsCancellationRequested)
                {
                    return;
                }

                delayAwaiter = TimeSpan.FromSeconds(1);

                JobTimeoutDelayAwaiterCts?.Dispose();
                JobTimeoutDelayAwaiterCts = SafeCancellationTokenSource.CreateLinked(JobTimeoutCheckerCts.Token);
            }
        }
    }
    
    private async Task OnTimerTick(
        InternalFunctionContext[] functions,
        CancellationToken cancellationToken = default, 
        bool dueDone = false)
    {
        await _semaphoreSlim.WaitAsync(cancellationToken);

        foreach (var context in functions)
        {
            if (!_jobProviderManager.TryGetJobFunction(context.FunctionName, out var jobItem))
            {
                continue;
            }

            if (jobItem.Priority == SchedulerJobPriority.LongRunning)
            {
                _ = Task.Factory.StartNew(async () =>
                {
                    await ExecuteTaskAsync(context, jobItem.Delegate, dueDone, cancellationToken);
                }, TaskCreationOptions.LongRunning);
            }
            else
            {
                var taskDetails = Task.Factory.StartNew(
                    async () =>
                    {
                        await ExecuteTaskAsync(context, jobItem.Delegate, dueDone, cancellationToken);
                    },
                    cancellationToken,
                    TaskCreationOptions.DenyChildAttach,
                    _schedulerHostTaskScheduler
                ).Unwrap();

                _schedulerHostTaskScheduler.SetQueuedTaskPriority(taskDetails.Id, jobItem.Priority);
            }
        }

        _schedulerHostTaskScheduler.ExecutePriorityTasks();
        _semaphoreSlim.Release();
    }

    private async Task ExecuteTaskAsync(
        InternalFunctionContext context,
        AsyncSchedulerJobInvocationDelegate delegateFunction,
        bool isDue,
        CancellationToken cancellationToken = default)
    {
        var stopWatch = new Stopwatch();

        var scope = _serviceProvider.CreateScope();
        
        var internalJobManager = context.JobId != Guid.Empty 
            ? scope.ServiceProvider.GetRequiredService<ISchedulerHostStateManager>()
            : null;

        var cancellationTokenSource = cancellationToken != CancellationToken.None
            ? CancellationTokenSource.CreateLinkedTokenSource(cancellationToken)
            : new CancellationTokenSource();

        if (context.JobId != Guid.Empty)
        {
            JobCancellationTokenManager.TryAddJobCancellationToken(cancellationTokenSource, context.FunctionName, context.JobId, context.Type, isDue);
        }

        try
        {
            stopWatch.Start();

            async Task ExecuteDelegate(IServiceProvider scopeServiceProvider, CancellationToken scopeCancellationToken = default)
            {
                try
                {
                    await delegateFunction(
                        scopeServiceProvider, 
                        new SchedulerFunctionContext(
                            context.JobId, 
                            context.Type,
                            context.RetryCount, 
                            isDue, 
                            () => DeleteJob(context, internalJobManager, scopeCancellationToken),
                            null),
                        scopeCancellationToken
                    );
                }
                catch (TaskCanceledException)
                {
                    throw;
                }
                catch (TerminateExecutionException)
                {
                    throw;
                }
                catch
                {
                    if (context.JobId == Guid.Empty)
                    {
                        throw;
                    }

                    if (context.RetryCount >= context.Retries)
                    {
                        throw;
                    }

                    var retryInterval = (context.RetryIntervals != null && context.RetryIntervals.Length > 0)
                        ? (context.RetryCount < context.RetryIntervals.Length ? context.RetryIntervals[context.RetryCount] : context.RetryIntervals[^1])
                        : 30;

                    context.RetryCount++;

                    var updateResult = await internalJobManager
                        .UpdateJobRetriesAsync(context, cancellationToken)
                        .ConfigureAwait(false);
                    
                    if (updateResult.IsFailure)
                    {
                        throw new Exception(updateResult.Error);
                    }

                    await Task.Delay(TimeSpan.FromSeconds(retryInterval), scopeCancellationToken);

                    await ExecuteDelegate(scopeServiceProvider, scopeCancellationToken);
                }
            }
                
            await ExecuteDelegate(scope.ServiceProvider, cancellationTokenSource.Token).ConfigureAwait(false);
            
            stopWatch.Stop();
            context.ElapsedTime = stopWatch.ElapsedMilliseconds;
            
            if (context.JobId != Guid.Empty)
            {
                context.Status = isDue ? SchedulerJobStatus.DueDone : SchedulerJobStatus.Done;
                var setResult = await internalJobManager!
                    .SetJobsStatusAsync(context, cancellationToken)
                    .ConfigureAwait(false);
                
                if (setResult.IsFailure)
                {
                    throw new Exception(setResult.Error);
                }
            }
        }
        catch (TerminateExecutionException)
        {
        }
        catch (TaskCanceledException e)
        {
            context.ElapsedTime = stopWatch.ElapsedMilliseconds;
            context.Status = SchedulerJobStatus.Cancelled;

            if (context.JobId != Guid.Empty)
            {
                var setResult = await internalJobManager!
                    .SetJobsStatusAsync(context, cancellationToken)
                    .ConfigureAwait(false);
                
                if (setResult.IsFailure)
                {
                    throw new Exception(setResult.Error);
                }
            }

            var exceptionHandler = scope.ServiceProvider.GetRequiredService<ISchedulerHostExceptionHandler>();
            await exceptionHandler
                .HandleCanceledExceptionAsync(e, context.JobId, context.Type)
                .ConfigureAwait(false);
        }
        catch (Exception e)
        {
            context.ElapsedTime = stopWatch.ElapsedMilliseconds;
            context.ExceptionDetails = SerializeException(e);
            context.Status = SchedulerJobStatus.Failed;

            if (context.JobId != Guid.Empty)
            {
                var setResult = await internalJobManager!
                    .SetJobsStatusAsync(context, cancellationToken)
                    .ConfigureAwait(false);
                
                if (setResult.IsFailure)
                {
                    throw new Exception(setResult.Error);
                }
            }

            var exceptionHandler = scope.ServiceProvider.GetRequiredService<ISchedulerHostExceptionHandler>();
            await exceptionHandler
                .HandleExceptionAsync(e, context.JobId, context.Type)
                .ConfigureAwait(false);
        }
        finally
        {
            scope.Dispose();
            stopWatch.Reset();
            cancellationTokenSource.Dispose();
            
            if (context.JobId != Guid.Empty)
            {
                JobCancellationTokenManager.TryRemoveJobCancellationToken(context.JobId);
            }
        }
    }
    
}