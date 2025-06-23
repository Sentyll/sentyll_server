using Sentyll.Infrastructure.Events.Messaging.Abstractions.Contracts.Services.Templating;
using Sentyll.Infrastructure.Events.Messaging.Abstractions.Enums;
using Sentyll.Infrastructure.Server.TaskPipeline.Abstraction.Contracts.Services.EventProcessors;
using Microsoft.Extensions.DependencyInjection;
using Sentyll.Infrastructure.Events.Messaging.MicrosoftTeams.Services.EventProcessor;
using Sentyll.Infrastructure.Events.Messaging.MicrosoftTeams.Services.Templating;

namespace Sentyll.Infrastructure.Events.Messaging.MicrosoftTeams.Startup;

public static class MicrosoftTeamsStartup
{
    public static IServiceCollection RegisterMicrosoftTeamsDependencies(this IServiceCollection serviceCollection)
        => serviceCollection
            .RegisterTemplatingServices()
            .RegisterEventProcessors();
    
    private static IServiceCollection RegisterEventProcessors(this IServiceCollection serviceCollection) 
        => serviceCollection.AddTransient<IMessagingEventProcessor, MicrosoftTeamsEventProcessor>();

    private static IServiceCollection RegisterTemplatingServices(this IServiceCollection serviceCollection)
        => serviceCollection.AddKeyedTransient<ITemplateGenerator, MicrosoftTeamsTemplateGenerator>(TemplateGeneratorType.JSON_MICROSOFTTEAMS);
}