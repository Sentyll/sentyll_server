using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Sentyll.Infrastructure.HealthChecks.SqlServer.HealthChecks;
using Sentyll.Infrastructure.HealthChecks.SqlServer.Services;

namespace Sentyll.Infrastructure.HealthChecks.SqlServer.Startup;

public static class SqlServerDatabaseHealthCheckStartup
{
    
    public static IServiceCollection RegisterSqlServerHealthChecksDependencies(this IServiceCollection serviceCollection) 
        => serviceCollection.AddSingleton<SqlServerConnectionService>();

    public static IServiceProvider ConfigureSqlServerHealthChecks(this IServiceProvider serviceProvider) 
        => serviceProvider
            .ConfigureJob<SqlServerDatabaseV1HealthCheck>()
            .ConfigureJob<SqlServerDatabaseQueryV1HealthCheck>();
}