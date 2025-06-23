namespace Sentyll.Domain.Common.Abstractions.Enums;

public enum MessagingEventType
{
    /// <summary>
    /// Azure Communication Services (Email) <br />
    /// https://azure.microsoft.com/en-gb/pricing/details/communication-services/
    /// </summary>
    AZURE_COMMUNICATIONSERVICESES_EMAIL = 0,
    
    SLACK = 1,
    
    MICROSOFT_TEAMS = 2
}