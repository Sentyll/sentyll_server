using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Sentyll.Infrastructure.HealthChecks.Uris.Core.Constants;
using Sentyll.Infrastructure.HealthChecks.Uris.HealthChecks;
using Sentyll.Infrastructure.HealthChecks.Uris.Services;

namespace Sentyll.Infrastructure.HealthChecks.Uris.Startup;

public static class UriHealthCheckStartup
{
    
    public static IServiceCollection RegisterUriHealthChecksDependencies(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHttpClient(UriConstants.HttpClientName);
        serviceCollection.AddSingleton<HttpService>();
        
        return serviceCollection;
    }
    
    public static IServiceProvider ConfigureUriHealthChecks(this IServiceProvider serviceProvider) 
        => serviceProvider.ConfigureJob<UriPingV1HealthCheck>();
}