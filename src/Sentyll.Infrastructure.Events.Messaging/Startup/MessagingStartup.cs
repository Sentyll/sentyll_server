using Sentyll.Infrastructure.Events.Messaging.Abstractions.Contracts.Factories;
using Sentyll.Infrastructure.Events.Messaging.Abstractions.Contracts.Services.Templating;
using Sentyll.Infrastructure.Events.Messaging.Abstractions.Enums;
using Sentyll.Infrastructure.Events.Messaging.Abstractions.models.Options;
using Sentyll.Infrastructure.Events.Messaging.Azure.Communication.Emailer.Startup;
using Sentyll.Infrastructure.Events.Messaging.MicrosoftTeams.Startup;
using Sentyll.Infrastructure.Events.Messaging.Slack.Startup;
using Microsoft.Extensions.DependencyInjection;
using Sentyll.Infrastructure.Events.Messaging.Factories;
using Sentyll.Infrastructure.Events.Messaging.Services.Templating;

namespace Sentyll.Infrastructure.Events.Messaging.Startup;

public static class MessagingStartup
{
    public static IServiceCollection RegisterMessagingDependencies(this IServiceCollection serviceCollection)
        => serviceCollection
            .RegisterMessagingProcessors()
            .RegisterTemplatingServices()
            .ConfigureOptions();
    
    private static IServiceCollection RegisterMessagingProcessors(this IServiceCollection serviceCollection) 
        => serviceCollection
            .RegisterSlackDependencies()
            .RegisterMicrosoftTeamsDependencies()
            .RegisterAzureCommunicationEmailerDependencies();

    private static IServiceCollection RegisterTemplatingServices(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddKeyedTransient<ITemplateGenerator, EmailTemplateGenerator>(TemplateGeneratorType.HTML)
            .AddTransient<ITemplateGeneratorFactory>((sp) => new TemplateGeneratorFactory()
            {
                {
                    TemplateGeneratorType.HTML,
                    sp.GetRequiredKeyedService<ITemplateGenerator>(TemplateGeneratorType.HTML)
                },
                {
                    TemplateGeneratorType.JSON_MICROSOFTTEAMS,
                    sp.GetRequiredKeyedService<ITemplateGenerator>(TemplateGeneratorType.JSON_MICROSOFTTEAMS)
                },
                {
                    TemplateGeneratorType.JSON_SLACK,
                    sp.GetRequiredKeyedService<ITemplateGenerator>(TemplateGeneratorType.JSON_SLACK)
                }
            });

    private static IServiceCollection ConfigureOptions(this IServiceCollection serviceCollection)
    {
        serviceCollection.Configure<ServerEndpointsOptions>(options =>
        {
            
        });

        return serviceCollection;
    }

}