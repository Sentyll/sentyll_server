using Sentyll.Infrastructure.Events.WebHooks.Abstractions.Contracts.Services;
using Microsoft.Extensions.DependencyInjection;
using Sentyll.Infrastructure.Events.WebHooks.Core.Constants;
using Sentyll.Infrastructure.Events.WebHooks.Services;

namespace Sentyll.Infrastructure.Events.WebHooks.Startup;

public static class WebhooksStartup
{
    public static IServiceCollection RegisterWebHookDependencies(this IServiceCollection serviceCollection)
        => serviceCollection
            .RegisterHttpClients()
            .RegisterServices();
    
    private static IServiceCollection RegisterHttpClients(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHttpClient(WebhookConstants.HttpClientName);
        
        return serviceCollection;
    }
    
    private static IServiceCollection RegisterServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IWebhookHttpClientService, WebhookHttpClientService>();
        
        return serviceCollection;
    }
}