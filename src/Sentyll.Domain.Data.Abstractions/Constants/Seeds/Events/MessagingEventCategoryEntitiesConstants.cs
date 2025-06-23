using Sentyll.Domain.Data.Abstractions.Entities.Events;

namespace Sentyll.Domain.Data.Abstractions.Constants.Seeds.Events;

public static class MessagingEventCategoryEntitiesConstants
{

    public static Dictionary<MessagingEventType, EventCategoryEntity> TypeToEntityMapping => new()
    {
        { MessagingEventType.AZURE_COMMUNICATIONSERVICESES_EMAIL, AzureCommunicationServicesEmail },
        { MessagingEventType.MICROSOFT_TEAMS, MicrosoftTeams },
        { MessagingEventType.SLACK, Slack },
    };
    
    public static EventCategoryEntity AzureCommunicationServicesEmail => new()
    {
        Id = Guid.Parse("EBA0C273-B054-46C4-814F-32EA6677AD81"),
        Type = nameof(MessagingEventType),
        TypeName = nameof(MessagingEventType.AZURE_COMMUNICATIONSERVICESES_EMAIL),
        TypeValue = (int)MessagingEventType.AZURE_COMMUNICATIONSERVICESES_EMAIL
    };
    
    public static EventCategoryEntity MicrosoftTeams => new()
    {
        Id = Guid.Parse("9E7928EB-9D8A-4F7A-B6D6-320D5AA7E836"),
        Type = nameof(MessagingEventType),
        TypeName = nameof(MessagingEventType.MICROSOFT_TEAMS),
        TypeValue = (int)MessagingEventType.MICROSOFT_TEAMS
    };

    public static EventCategoryEntity Slack => new()
    {
        Id = Guid.Parse("32EFD3B5-B25A-4B00-AA6A-EDB5E7C771E5"),
        Type = nameof(MessagingEventType),
        TypeName = nameof(MessagingEventType.SLACK),
        TypeValue = (int)MessagingEventType.SLACK
    };
    
}