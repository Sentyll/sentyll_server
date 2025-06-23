using Sentyll.Domain.Data.Abstractions.Entities.Events;

namespace Sentyll.Domain.Data.Abstractions.Constants.Seeds.Events;

public static class WebhookEventCategoryEntitiesConstants
{
    
    public static EventCategoryEntity Webhook => new()
    {
        Id = Guid.Parse("D158E6F6-E720-4C8F-B4D0-3EBAB94665CA"),
        Type = "Webhook",
        TypeName = "Webhook",
        TypeValue = 0
    };

}