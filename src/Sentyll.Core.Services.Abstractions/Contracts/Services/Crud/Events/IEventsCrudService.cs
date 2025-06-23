using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;
using Sentyll.Domain.Common.Abstractions.Enums;
using Sentyll.Domain.Common.Abstractions.Models.Definitions.Events.Messaging.Payload;
using Sentyll.Domain.Common.Abstractions.Models.Results;
using Sentyll.Domain.Common.Models.ApiRequests.Events;
using Sentyll.Domain.Common.Models.ApiResult.Events;

namespace Sentyll.Core.Services.Abstractions.Contracts.Services.Crud.Events;

public interface IEventsCrudService
{
    Task<Result<PaginationResult<MessagingEventResult>>> GetPaginatedEventsAsync(
        GetPaginatedEventsRequest request,
        CancellationToken cancellationToken = default
    );

    Task<Result> CreateNewMessagingEventAsync<T>(
        MessagingEventRestContentPayloadDefinition<T> definition,
        MessagingEventType type,
        CancellationToken cancellationToken = default
    ) where T : IValidatable, new();

}