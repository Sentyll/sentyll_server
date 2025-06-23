using Sentyll.Domain.Data.Abstractions.Entities.Events;
using Sentyll.Domain.Data.Services.Abstractions.Contracts.Repositories.Base;

namespace Sentyll.Domain.Data.Services.Abstractions.Contracts.Repositories.Events;

public interface IEventParameterEntityRepository : IEntityRepository<EventParameterEntity>
{

    Task<Result<List<EventParameterEntity>>> GetEventParameters(
        Guid eventId,
        CancellationToken cancellationToken = default
    );

}