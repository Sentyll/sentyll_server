using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;
using Sentyll.Domain.Common.Abstractions.Models.Definitions.Events.Messaging.Payload;
using Sentyll.Domain.Common.Mappers.Parameters;
using Sentyll.Domain.Data.Abstractions.Constants.Seeds.Events;
using Sentyll.Domain.Data.Abstractions.Entities.Events;

namespace Sentyll.Domain.Common.Mappers.Definition;

public static class EventDefinitionMapperExtensions
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
    
    public static Result<EventEntity> ToEventEntity<T>(
        this MessagingEventRestContentPayloadDefinition<T> definition,
        MessagingEventType type)
        where T : IValidatable, new()
        => definition.MessagingEvent
            .ToEventParameterEntities()
            .Map(parameters => new EventEntity()
            {
                Name = definition.Name,
                Description = definition.Description,
                Tags = definition.Tags,
                EventCategoryId = MessagingEventCategoryEntitiesConstants.TypeToEntityMapping[type].Id,
                IsEnabled = definition.IsEnabled,
                Parameters = parameters
            });
    
}