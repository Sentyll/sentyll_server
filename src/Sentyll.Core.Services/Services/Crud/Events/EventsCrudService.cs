using Sentyll.Core.Services.Abstractions.Contracts.Services.Crud.Events;
using Sentyll.Core.Services.Extensions;
using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;
using Sentyll.Domain.Common.Abstractions.Enums;
using Sentyll.Domain.Common.Abstractions.Models.Definitions.Events.Messaging.Payload;
using Sentyll.Domain.Common.Abstractions.Models.Results;
using Sentyll.Domain.Common.Mappers.Definition;
using Sentyll.Domain.Common.Mappers.Events;
using Sentyll.Domain.Common.Models.ApiRequests.Events;
using Sentyll.Domain.Common.Models.ApiResult.Events;
using Sentyll.Domain.Data.Abstractions.Entities.Events;
using Sentyll.Domain.Data.Services.Abstractions.Contracts.Repositories.Events;

namespace Sentyll.Core.Services.Services.Crud.Events;

internal sealed class EventsCrudService(
    IEventEntityRepository eventEntityRepository
    ) : IEventsCrudService
{

    public async Task<Result<PaginationResult<MessagingEventResult>>> GetPaginatedEventsAsync(
        GetPaginatedEventsRequest request,
        CancellationToken cancellationToken = default)
    {
        Expression<Func<EventEntity, object>> orderFunc = request.OrderBy switch
        {
            GetPaginatedEventsRequest.OrderByIsEnabled => unit => unit.IsEnabled,
            _ => unit => unit.Name
        };

        var filter = PredicateBuilder
            .New<EventEntity>(_ => true)
            .AndIf(!string.IsNullOrWhiteSpace(request.SearchText), () =>
            {
                var loweredSearchText = request.SearchText!.ToLower();
                return x => x.Name.ToLower().Contains(loweredSearchText);
            });
        
        return await eventEntityRepository
            .GetFilteredPaginatedEventsAsync(filter, orderFunc, request, cancellationToken)
            .Bind(entity => entity.ToPaginatedMessagingEventResult())
            .ConfigureAwait(false);
    }
    
    public async Task<Result> CreateNewMessagingEventAsync<T>(
        MessagingEventRestContentPayloadDefinition<T> definition,
        MessagingEventType type,
        CancellationToken cancellationToken = default)
        where T : IValidatable, new()
        => await definition
            .ToEventEntity(type)
            .Bind(eventEntity => eventEntityRepository.AddAsync(eventEntity, cancellationToken))
            .ConfigureAwait(false);
    
}