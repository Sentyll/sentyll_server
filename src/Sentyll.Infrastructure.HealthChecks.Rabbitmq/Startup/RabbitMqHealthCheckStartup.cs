using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Sentyll.Infrastructure.HealthChecks.Rabbitmq.HealthChecks;

namespace Sentyll.Infrastructure.HealthChecks.Rabbitmq.Startup;

public static class RabbitMqHealthCheckStartup
{
    
    public static IServiceCollection RegisterRabbitMqHealthChecksDependencies(this IServiceCollection serviceCollection) 
        => serviceCollection;

    public static IServiceProvider ConfigureRabbitMqHealthChecks(this IServiceProvider serviceProvider) 
        => serviceProvider.ConfigureJob<RabbitMqV1HealthCheck>();
}