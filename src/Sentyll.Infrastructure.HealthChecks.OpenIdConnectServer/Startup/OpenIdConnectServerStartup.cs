using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Sentyll.Infrastructure.HealthChecks.OpenIdConnectServer.Core.Constants;
using Sentyll.Infrastructure.HealthChecks.OpenIdConnectServer.HealthChecks;

namespace Sentyll.Infrastructure.HealthChecks.OpenIdConnectServer.Startup;

public static class OpenIdConnectServerStartup
{
    
    public static IServiceCollection RegisterOpenIdConnectServerHealthChecksDependencies(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHttpClient(OidcConstants.HttpClientName);
        
        return serviceCollection;
    }
    
    public static IServiceProvider ConfigureOpenIdConnectServerHealthChecks(this IServiceProvider serviceProvider) 
        => serviceProvider.ConfigureJob<OpenIdConnectServerV1HealthCheck>();
}