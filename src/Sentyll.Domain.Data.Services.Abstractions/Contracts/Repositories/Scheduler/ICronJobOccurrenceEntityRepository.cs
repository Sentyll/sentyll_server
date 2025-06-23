using Sentyll.Domain.Common.Abstractions.Enums.Scheduler;
using Sentyll.Domain.Data.Abstractions.Entities.Scheduler;
using Sentyll.Domain.Data.Services.Abstractions.Contracts.Repositories.Base;

namespace Sentyll.Domain.Data.Services.Abstractions.Contracts.Repositories.Scheduler;

public interface ICronJobOccurrenceEntityRepository : IEntityRepository<CronJobOccurrenceEntity>
{
    
    Task<Result<bool>> AnyAsync(
        Guid cronJobId,
        SchedulerJobStatus status,
        CancellationToken cancellationToken = default
    );

    Task<Result<List<CronJobOccurrenceEntity>>> GetListUnsafeAsync(
        Guid cronJobId,
        SchedulerJobStatus status,
        CancellationToken cancellationToken = default
    );

    Task<Result<byte[]>> GetRequestAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );

    Task<Result<List<CronJobOccurrenceEntity>>> GetTimedOutCronJobsOccurrencesAsync(
        DateTime executedTime,
        CancellationToken cancellationToken = default
    );

    Task<Result<List<CronJobOccurrenceEntity>>> GetLockedAsNoTrackingUnsafeListAsync(
        string lockHolder,
        CancellationToken cancellationToken = default
    );

    Task<Result<List<CronJobOccurrenceEntity>>> GetNextOccurrencesForJobsAsync(
        Guid[] cronJobIds,
        string lockHolder,
        CancellationToken cancellationToken = default
    );

}