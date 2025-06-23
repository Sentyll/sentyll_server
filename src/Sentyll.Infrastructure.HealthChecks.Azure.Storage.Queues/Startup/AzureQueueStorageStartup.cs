using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Sentyll.Infrastructure.HealthChecks.Azure.Storage.Queues.HealthChecks;

namespace Sentyll.Infrastructure.HealthChecks.Azure.Storage.Queues.Startup;

public static class AzureQueueStorageStartup
{
    
    public static IServiceCollection RegisterAzureQueueStorageHealthChecksDependencies(this IServiceCollection serviceCollection) 
        => serviceCollection;

    public static IServiceProvider ConfigureAzureQueueStorageHealthChecks(this IServiceProvider serviceProvider) 
        => serviceProvider.ConfigureJob<AzureQueueStorageV1HealthCheck>();
    
}