using Sentyll.Domain.Data.Abstractions.Entities.Scheduler;
using Sentyll.Domain.Data.Services.Abstractions.Contracts.Repositories.Base;

namespace Sentyll.Domain.Data.Services.Abstractions.Contracts.Repositories.Scheduler;

public interface ICronJobEntityRepository : IEntityRepository<CronJobEntity>
{

    Task<Result<List<CronJobEntity>>> GetAsNoTrackingListUnsafeAsync(
        string[] expressions,
        CancellationToken cancellationToken = default
    );
    
    Task<Result<List<CronJobEntity>>> GetMemoryJobSeedsListUnsafeAsync(
        CancellationToken cancellationToken = default
    );

    Task<Result<List<string?>>> GetDistinctExpressionsListUnsafeAsync(
        CancellationToken cancellationToken = default
    );
}