using CSharpFunctionalExtensions;
using Sentyll.Domain.Common.Abstractions.Enums.Scheduler;
using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Models.Context;

namespace Sentyll.Infrastructure.Server.Scheduler.Abstractions.Services.Scheduler;

public interface ISchedulerHostStateManager
{
    
    Task<Result<T>> GetOccurrenceRequestAsync<T>(
        Guid jobId,
        SchedulerJobType type,
        CancellationToken cancellationToken = default
    );
    
    Task<Result> DeleteJobAsync(
        Guid jobId, 
        SchedulerJobType type, 
        CancellationToken cancellationToken = default
    );
    
    Task<Result<InternalFunctionContext[]>> GetTimedOutFunctionsAsync(
        CancellationToken cancellationToken = default
    );
    
    Task<Result> SyncJobsWithPersistantStorageAsync(
        IList<(string, string)> cronExpressions,
        CancellationToken cancellationToken = default
    );
    
    Task<Result> ReleaseOrCancelAllLockedJobsAsync(
        bool terminateExpiredJobs,
        CancellationToken cancellationToken = default
    );
    
    Task<Result> UpdateJobRetriesAsync(
        InternalFunctionContext context,
        CancellationToken cancellationToken = default
    );
    
    Task<Result> SetJobsInprogressAsync(
        InternalFunctionContext[] context,
        CancellationToken cancellationToken = default
    );
    
    Task<Result> ReleaseLockedJobsAsync(
        InternalFunctionContext[] context,
        CancellationToken cancellationToken = default
    );
    
    Task<Result> SetJobsStatusAsync(
        InternalFunctionContext context,
        CancellationToken cancellationToken = default
    );
    
    Task<Result<(TimeSpan TimeRemaining, InternalFunctionContext[] Functions)>> GetNextJobsAsync(
        CancellationToken cancellationToken = default
    );
}