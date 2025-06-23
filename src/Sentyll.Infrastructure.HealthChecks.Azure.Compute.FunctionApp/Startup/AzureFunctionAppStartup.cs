using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Sentyll.Infrastructure.HealthChecks.Azure.Compute.FunctionApp.Core.Constants;
using Sentyll.Infrastructure.HealthChecks.Azure.Compute.FunctionApp.HealthChecks;

namespace Sentyll.Infrastructure.HealthChecks.Azure.Compute.FunctionApp.Startup;

public static class AzureFunctionAppStartup
{
    
    public static IServiceCollection RegisterAzureFunctionAppHealthChecksDependencies(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHttpClient(AfaConstants.HttpClientName);
        return serviceCollection;
    }
    
    public static IServiceProvider ConfigureAzureFunctionAppHealthChecks(this IServiceProvider serviceProvider) 
        => serviceProvider.ConfigureJob<AzureFunctionAppHttpEndpointV1HealthCheck>();
}