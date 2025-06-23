using Sentyll.Infrastructure.Events.Messaging.Abstractions.Contracts.Services.Templating;
using Sentyll.Infrastructure.Events.Messaging.Abstractions.Enums;
using Sentyll.Infrastructure.Server.TaskPipeline.Abstraction.Contracts.Services.EventProcessors;
using Microsoft.Extensions.DependencyInjection;
using Sentyll.Infrastructure.Events.Messaging.Slack.Services.EventProcessors;
using Sentyll.Infrastructure.Events.Messaging.Slack.Services.Templating;

namespace Sentyll.Infrastructure.Events.Messaging.Slack.Startup;

public static class SlackStartup
{
    public static IServiceCollection RegisterSlackDependencies(this IServiceCollection serviceCollection)
        => serviceCollection
            .RegisterTemplatingServices()
            .RegisterEventProcessors();
    
    private static IServiceCollection RegisterEventProcessors(this IServiceCollection serviceCollection) 
        => serviceCollection.AddTransient<IMessagingEventProcessor, SlackEventProcessor>();

    private static IServiceCollection RegisterTemplatingServices(this IServiceCollection serviceCollection)
        => serviceCollection.AddKeyedTransient<ITemplateGenerator, SlackTemplateGenerator>(TemplateGeneratorType.JSON_SLACK);
}