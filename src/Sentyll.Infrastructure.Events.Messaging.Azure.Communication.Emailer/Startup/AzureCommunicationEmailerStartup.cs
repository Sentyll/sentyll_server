using Sentyll.Infrastructure.Server.TaskPipeline.Abstraction.Contracts.Services.EventProcessors;
using Microsoft.Extensions.DependencyInjection;
using Sentyll.Infrastructure.Events.Messaging.Azure.Communication.Emailer.Core.Contracts.Services.Client;
using Sentyll.Infrastructure.Events.Messaging.Azure.Communication.Emailer.Services.Client;
using Sentyll.Infrastructure.Events.Messaging.Azure.Communication.Emailer.Services.EventProcessor;

namespace Sentyll.Infrastructure.Events.Messaging.Azure.Communication.Emailer.Startup;

public static class AzureCommunicationEmailerStartup
{
    public static IServiceCollection RegisterAzureCommunicationEmailerDependencies(this IServiceCollection serviceCollection)
        => serviceCollection
            .RegisterEventProcessors()
            .RegisterServices();
    
    private static IServiceCollection RegisterEventProcessors(this IServiceCollection serviceCollection) 
        => serviceCollection.AddTransient<IMessagingEventProcessor, AzureCommunicationEmailerEventProcessor>();

    private static IServiceCollection RegisterServices(this IServiceCollection serviceCollection)
        => serviceCollection.AddTransient<IEmailClientService, EmailClientService>();

}