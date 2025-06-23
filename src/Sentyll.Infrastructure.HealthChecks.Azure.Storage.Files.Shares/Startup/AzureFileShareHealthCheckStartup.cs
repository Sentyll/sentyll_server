using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Sentyll.Infrastructure.HealthChecks.Azure.Storage.Files.Shares.HealthChecks;

namespace Sentyll.Infrastructure.HealthChecks.Azure.Storage.Files.Shares.Startup;

public static class AzureFileShareHealthCheckStartup
{
    
    public static IServiceCollection RegisterAzureFileShareHealthChecksDependencies(this IServiceCollection serviceCollection) 
        => serviceCollection;

    public static IServiceProvider ConfigureAzureFileShareHealthChecks(this IServiceProvider serviceProvider) 
        => serviceProvider.ConfigureJob<AzureFileShareV1HealthCheck>();
}