using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Sentyll.Infrastructure.HealthChecks.MongoDb.HealthChecks;

namespace Sentyll.Infrastructure.HealthChecks.MongoDb.Startup;

public static class MongoDbHealthCheckStartup
{
    
    public static IServiceCollection RegisterMongoDbHealthChecksDependencies(this IServiceCollection serviceCollection) 
        => serviceCollection;

    public static IServiceProvider ConfigureMongoDbHealthChecks(this IServiceProvider serviceProvider) 
        => serviceProvider.ConfigureJob<MongoDbV1HealthCheck>();
}