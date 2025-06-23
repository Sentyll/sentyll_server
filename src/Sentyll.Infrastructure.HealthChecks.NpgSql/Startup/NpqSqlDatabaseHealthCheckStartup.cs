using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Sentyll.Infrastructure.HealthChecks.NpgSql.HealthChecks;
using Sentyll.Infrastructure.HealthChecks.NpgSql.Services;

namespace Sentyll.Infrastructure.HealthChecks.NpgSql.Startup;

public static class NpqSqlDatabaseHealthCheckStartup
{
    
    public static IServiceCollection RegisterNpqSqlDatabaseHealthChecksDependencies(this IServiceCollection serviceCollection) 
        => serviceCollection.AddSingleton<NpqSqlCommandService>();

    public static IServiceProvider ConfigureNpqSqlDatabaseHealthChecks(this IServiceProvider serviceProvider) 
        => serviceProvider
            .ConfigureJob<NpqSqlDatabaseV1HealthCheck>()
            .ConfigureJob<NpqSqlDatabaseQueryV1HealthCheck>();
}