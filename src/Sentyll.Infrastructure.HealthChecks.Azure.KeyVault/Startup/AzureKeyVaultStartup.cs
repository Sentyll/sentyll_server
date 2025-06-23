using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Sentyll.Infrastructure.HealthChecks.Azure.KeyVault.HealthChecks;

namespace Sentyll.Infrastructure.HealthChecks.Azure.KeyVault.Startup;

public static class AzureKeyVaultStartup
{
    
    public static IServiceCollection RegisterAzureKeyVaultHealthChecksDependencies(this IServiceCollection serviceCollection) 
        => serviceCollection;

    public static IServiceProvider ConfigureAzureKeyVaultHealthChecks(this IServiceProvider serviceProvider) 
        => serviceProvider.ConfigureJob<AzureKeyVaultV1HealthCheck>();
    
}