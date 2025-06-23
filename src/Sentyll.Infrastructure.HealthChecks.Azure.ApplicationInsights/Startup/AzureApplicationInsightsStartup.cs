using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Sentyll.Infrastructure.HealthChecks.Azure.ApplicationInsights.Core.Constants;
using Sentyll.Infrastructure.HealthChecks.Azure.ApplicationInsights.HealthChecks;

namespace Sentyll.Infrastructure.HealthChecks.Azure.ApplicationInsights.Startup;

public static class AzureApplicationInsightsStartup
{
    
    public static IServiceCollection RegisterAzureApplicationInsightsHealthChecksDependencies(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHttpClient(ApiConstants.HttpClientName);
        
        return serviceCollection;
    }
    
    public static IServiceProvider ConfigureAzureApplicationInsightsHealthChecks(this IServiceProvider serviceProvider) 
        => serviceProvider.ConfigureJob<AzureApplicationInsightsV1HealthCheck>();
}