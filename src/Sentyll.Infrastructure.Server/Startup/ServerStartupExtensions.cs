using Sentyll.Infrastructure.Server.Abstractions.Contracts.Services.Address;
using Microsoft.Extensions.DependencyInjection;
using Sentyll.Infrastructure.Server.Core.Constants;
using Sentyll.Infrastructure.Server.Core.Contracts.Services;
using Sentyll.Infrastructure.Server.services;
using Sentyll.Infrastructure.Server.services.Address;
using Sentyll.Infrastructure.Server.Stores;

namespace Sentyll.Infrastructure.Server.Startup;

public static class ServerStartupExtensions
{

    public static IServiceCollection RegisterServerDependencies(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddDependencies()
            .AddStores()
            .AddServices();
    
    private static IServiceCollection AddDependencies(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHttpClient(DependencyConstants.ReportCollectorHttpClient);

        return serviceCollection;
    }
    
    private static IServiceCollection AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ISystemClock, SystemClock>();
        serviceCollection.AddTransient<IServerAddressesService, ServerAddressesService>();
        serviceCollection.AddTransient<IHealthCheckReportCollector, HealthCheckReportCollector>();
        serviceCollection.AddTransient<IDefinitionService, DefinitionService>();

        return serviceCollection;
    }
    
    private static IServiceCollection AddStores(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IServerSettingsStore, ServerSettingsStore>();
        
        return serviceCollection;
    }
    
}