using Sentyll.Domain.Data.Abstractions.Constants.Seeds.Events;
using Sentyll.Domain.Data.Abstractions.Entities.Events;

namespace Sentyll.Domain.Data.Abstractions.Seeds;

public static class EventCategoryEntitiesSeed
{
    public static ModelBuilder SeedEventCategoryEntities(this ModelBuilder builder)
    {
        builder
            .Entity<EventCategoryEntity>()
            .HasData(WebhookEventCategoryEntitiesConstants.Webhook);
        
        builder
            .Entity<EventCategoryEntity>()
            .HasData(
                MessagingEventCategoryEntitiesConstants.AzureCommunicationServicesEmail,
                MessagingEventCategoryEntitiesConstants.MicrosoftTeams,
                MessagingEventCategoryEntitiesConstants.Slack
            );

        return builder;
    }
}