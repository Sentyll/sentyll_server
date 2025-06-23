using Sentyll.Core.Services.Abstractions.Contracts.Services.Crud.Events;
using Sentyll.Core.Services.Abstractions.Contracts.Services.Crud.HealthChecks;
using Sentyll.Core.Services.Services.Crud.Events;
using Sentyll.Core.Services.Services.Crud.HealthChecks;

namespace Sentyll.Core.Services.Startup;

public static class CoreServicesStartup
{
    public static IServiceCollection RegisterCoreServicesDependencies(this IServiceCollection serviceCollection) 
        => serviceCollection
            .RegisterHealthCheckServices()
            .RegisterEventServices();
    
    private static IServiceCollection RegisterHealthCheckServices(this IServiceCollection serviceCollection) 
        => serviceCollection
            .AddTransient<IHealthCheckAssignmentCrudService, HealthCheckAssignmentCrudService>()
            .AddTransient<IHealthChecksCrudService, HealthChecksCrudService>();

    private static IServiceCollection RegisterEventServices(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddTransient<IEventsCrudService, EventsCrudService>();

}