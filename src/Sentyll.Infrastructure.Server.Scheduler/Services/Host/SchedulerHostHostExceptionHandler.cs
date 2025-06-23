using Sentyll.Domain.Common.Abstractions.Enums.Scheduler;
using Microsoft.Extensions.Logging;
using Sentyll.Infrastructure.Server.Scheduler.Core.Contracts.Services.Host;

namespace Sentyll.Infrastructure.Server.Scheduler.Services.Host;

internal sealed class SchedulerHostHostExceptionHandler(
    ILogger<SchedulerHostHostExceptionHandler> logger
    ) : ISchedulerHostExceptionHandler
{
    public Task HandleExceptionAsync(Exception exception, Guid jobId, SchedulerJobType type)
    {
        logger.LogError(exception, "An unhandled exception had occured. [ JobId:{jobId},  schedulerJobType:{type}]", 
            jobId, type);
        
        return Task.CompletedTask;
    }

    public Task HandleCanceledExceptionAsync(Exception exception, Guid jobId, SchedulerJobType type)
    {
        logger.LogError(exception, "A job was cancelled. [ JobId:{jobId},  schedulerJobType:{type}]", 
            jobId, type);
        
        return Task.CompletedTask;
    }
}