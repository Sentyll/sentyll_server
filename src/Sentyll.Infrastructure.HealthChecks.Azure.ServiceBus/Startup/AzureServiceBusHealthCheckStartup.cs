using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Sentyll.Infrastructure.HealthChecks.Azure.ServiceBus.HealthChecks;

namespace Sentyll.Infrastructure.HealthChecks.Azure.ServiceBus.Startup;

public static class AzureServiceBusHealthCheckStartup
{
    
    public static IServiceCollection RegisterAzureServiceBusTopicHealthChecksDependencies(this IServiceCollection serviceCollection) 
        => serviceCollection;

    public static IServiceProvider ConfigureAzureServiceBusTopicHealthChecks(this IServiceProvider serviceProvider) 
        => serviceProvider
            .ConfigureJob<AzureServiceBusTopicV1HealthCheck>()
            .ConfigureJob<AzureServiceBusSubscriptionV1HealthCheck>()
            .ConfigureJob<AzureServiceBusQueueMessageCountThresholdV1HealthCheck>()
            .ConfigureJob<AzureServiceBusQueueV1HealthCheck>();
    
}