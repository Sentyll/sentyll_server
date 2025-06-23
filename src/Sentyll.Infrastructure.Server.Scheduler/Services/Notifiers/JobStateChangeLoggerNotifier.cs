using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Models.Dto;
using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Services.Notifiers;
using Microsoft.Extensions.Logging;

namespace Sentyll.Infrastructure.Server.Scheduler.Services.Notifiers;

internal sealed class JobStateChangeLoggerNotifier(
    ILogger<JobStateChangeLoggerNotifier> logger
    ) : IJobStateChangeNotifier
{

    public Task NotifyAddCronJobAsync(CronJobDto cronJob)
    {
        logger.LogInformation("New cron job has been registered. [id: {Id}, Function: {Function}, Expression: {Expression}]",
            cronJob.Id, 
            cronJob.Function, 
            cronJob.Expression);
        
        return Task.CompletedTask;
    }

    public Task NotifyUpdateCronJobAsync(CronJobDto cronJob)
    {
        logger.LogInformation("Existing cron job has been updated. [id: {Id}, Function: {Function}, Expression: {Expression}]",
            cronJob.Id, 
            cronJob.Function, 
            cronJob.Expression);
        
        return Task.CompletedTask;
    }

    public Task NotifyRemoveCronJobAsync(Guid id)
    {
        logger.LogInformation("Existing cron job has been deleted. [id: {Id}]", 
            id);
        
        return Task.CompletedTask;
    }

    public Task NotifyAddTimerJobAsync(TimerJobDto timerJob)
    {
        logger.LogInformation("New timer job has been registered. [id: {Id}, Function: {Function}, ExecutionTime: {ExecutionTime}]",
            timerJob.Id, 
            timerJob.Function, 
            timerJob.ExecutionTime);
        
        return Task.CompletedTask;
    }

    public Task NotifyUpdateTimerJobAsync(TimerJobDto timerJob)
    {
        logger.LogInformation("Existing timer job has been updated. [id: {Id}, Function: {Function}, ExecutionTime: {ExecutionTime}]",
            timerJob.Id, 
            timerJob.Function, 
            timerJob.ExecutionTime);
        
        return Task.CompletedTask;
    }

    public Task NotifyRemoveTimerJobAsync(Guid id)
    {
        logger.LogInformation("Existing timer job has been deleted. [id: {Id}]", 
            id);
        
        return Task.CompletedTask;
    }

    public Task NotifyUpdateNextOccurrence(DateTime? nextOccurrence)
    {
        logger.LogInformation("Next job invocation will be triggered on {nextOccurrence}", nextOccurrence);
        
        return Task.CompletedTask;
    }

    public Task NotifyAddCronOccurrenceAsync(Guid jobId, JobOccurrenceDto occurrence)
    {
        logger.LogInformation("New cron occurrence has been registered. [jobId: {jobId}, ExecutedAt: {ExecutedAt}]", 
            jobId, occurrence.ExecutedAt);
        
        return Task.CompletedTask;
    }

    public Task NotifyUpdateCronOccurrenceAsync(Guid jobId, JobOccurrenceDto occurrence)
    {
        logger.LogInformation("Existing cron occurrence has been updated. [jobId: {jobId}, ExecutedAt: {ExecutedAt}]", 
            jobId, occurrence.ExecutedAt);
        
        return Task.CompletedTask;
    }

    public Task NotifyCanceledJobAsync(Guid id)
    {
        logger.LogInformation("Job has been cancelled. [ id: {id}]", id);
        
        return Task.CompletedTask;
    }
}