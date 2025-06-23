using Sentyll.Domain.Data.Abstractions.Entities.Events;
using Sentyll.Domain.Data.Services.Abstractions.Contracts.Repositories.Events;
using Sentyll.Domain.Data.Services.Repositories.Base;

namespace Sentyll.Domain.Data.Services.Repositories.Events;

internal sealed class EventExecutionEntityRepository(
    SentyllContext ctx
) : EntityRepository<EventExecutionEntity>(ctx), IEventExecutionEntityRepository
{

}