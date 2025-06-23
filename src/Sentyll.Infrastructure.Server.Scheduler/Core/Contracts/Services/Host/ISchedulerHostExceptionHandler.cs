using Sentyll.Domain.Common.Abstractions.Enums.Scheduler;

namespace Sentyll.Infrastructure.Server.Scheduler.Core.Contracts.Services.Host;

internal interface ISchedulerHostExceptionHandler
{
    public Task HandleExceptionAsync(
        Exception exception,
        Guid jobId,
        SchedulerJobType type
    );

    public Task HandleCanceledExceptionAsync(
        Exception exception,
        Guid jobId,
        SchedulerJobType type
    );
}