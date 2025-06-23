using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Sentyll.Infrastructure.HealthChecks.Azure.SignalR.HealthChecks;

namespace Sentyll.Infrastructure.HealthChecks.Azure.SignalR.Startup;

public static class AzureSignalRHealthCheckStartup
{
    
    public static IServiceCollection RegisterAzureSignalrHealthChecksDependencies(this IServiceCollection serviceCollection) 
        => serviceCollection;

    public static IServiceProvider ConfigureAzureSignalrHealthChecks(this IServiceProvider serviceProvider) 
        => serviceProvider.ConfigureJob<AzureSignalrV1HealthCheck>();
    
}