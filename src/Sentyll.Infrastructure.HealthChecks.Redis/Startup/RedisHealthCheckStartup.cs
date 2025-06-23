using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Sentyll.Infrastructure.HealthChecks.Redis.HealthChecks;

namespace Sentyll.Infrastructure.HealthChecks.Redis.Startup;

public static class RedisHealthCheckStartup
{
    
    public static IServiceCollection RegisterRedisHealthChecksDependencies(this IServiceCollection serviceCollection) 
        => serviceCollection;

    public static IServiceProvider ConfigureRedisHealthChecks(this IServiceProvider serviceProvider) 
        => serviceProvider.ConfigureJob<RedisV1HealthCheck>();
}