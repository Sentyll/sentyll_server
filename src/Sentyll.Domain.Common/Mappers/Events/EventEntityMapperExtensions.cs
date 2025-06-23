using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;
using Sentyll.Domain.Common.Mappers.Pagination;
using Sentyll.Domain.Common.Mappers.Parameters;
using Sentyll.Domain.Data.Abstractions.Entities.Events;
using Sentyll.Domain.Common.Models.ApiResult.Events;

namespace Sentyll.Domain.Common.Mappers.Events;

public static class EventEntityMapperExtensions
{
    
    public static Result<List<EventParameterEntity>> ToEventParameterEntities<T>(this T parameterObj)
        where T : IValidatable, new()
        => parameterObj
            .ToConfigurationEntities()
            .Map((configurations) => configurations
                .Select(config => new EventParameterEntity()
                {
                    Configuration = config
                })
                .ToList()
            );
    
    public static Result<PaginationResult<MessagingEventResult>> ToPaginatedMessagingEventResult(
        this PaginationResult<EventEntity> result
        ) => result.ToPaginationResult(entity => new MessagingEventResult(
            entity.Id,
            entity.IsEnabled,
            entity.Name,
            entity.Description,
            (MessagingEventType)entity.EventCategory.TypeValue
        ));

}