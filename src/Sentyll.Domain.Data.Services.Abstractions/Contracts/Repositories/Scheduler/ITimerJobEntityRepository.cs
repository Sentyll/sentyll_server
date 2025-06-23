using Sentyll.Domain.Data.Abstractions.Entities.Scheduler;
using Sentyll.Domain.Data.Services.Abstractions.Contracts.Repositories.Base;

namespace Sentyll.Domain.Data.Services.Abstractions.Contracts.Repositories.Scheduler;

public interface ITimerJobEntityRepository : IEntityRepository<TimerJobEntity>
{
    Task<Result<byte[]>> GetRequestAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );

    Task<Result<List<TimerJobEntity>>> GetTimedOutUnsafeListAsync(
        DateTime executedTime,
        CancellationToken cancellationToken = default
    );

    Task<Maybe<DateTime>> GetEarliestExecutionTimeAsync(
        DateTime currentTime,
        CancellationToken cancellationToken = default
    );

    Task<Result<List<TimerJobEntity>>> GetLockedUnsafeListAsync(
        string lockHolder,
        CancellationToken cancellationToken = default
    );

    Task<Result<List<TimerJobEntity>>> GetNextUnsafeListAsync(
        string lockHolder,
        DateTime roundedMinDate,
        CancellationToken cancellationToken = default
    );

}