using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Sentyll.Infrastructure.HealthChecks.Azure.Storage.Blobs.HealthChecks;

namespace Sentyll.Infrastructure.HealthChecks.Azure.Storage.Blobs.Startup;

public static class AzureBlobStorageHealthCheckStartup
{
    
    public static IServiceCollection RegisterAzureBlobStorageHealthChecksDependencies(this IServiceCollection serviceCollection) 
        => serviceCollection;

    public static IServiceProvider ConfigureAzureBlobStorageHealthChecks(this IServiceProvider serviceProvider) 
        => serviceProvider.ConfigureJob<AzureBlobStorageV1HealthCheck>();
    
}