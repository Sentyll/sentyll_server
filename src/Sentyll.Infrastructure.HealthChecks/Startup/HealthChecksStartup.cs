using Sentyll.Infrastructure.HealthChecks.Abstractions.Services;
using Sentyll.Infrastructure.HealthChecks.Azure.ApplicationInsights.Startup;
using Sentyll.Infrastructure.HealthChecks.Azure.Compute.FunctionApp.Startup;
using Sentyll.Infrastructure.HealthChecks.Azure.KeyVault.Startup;
using Sentyll.Infrastructure.HealthChecks.Azure.ServiceBus.Startup;
using Sentyll.Infrastructure.HealthChecks.Azure.SignalR.Startup;
using Sentyll.Infrastructure.HealthChecks.Azure.Storage.Blobs.Startup;
using Sentyll.Infrastructure.HealthChecks.Azure.Storage.Files.Shares.Startup;
using Sentyll.Infrastructure.HealthChecks.Azure.Storage.Queues.Startup;
using Sentyll.Infrastructure.HealthChecks.MongoDb.Startup;
using Sentyll.Infrastructure.HealthChecks.NpgSql.Startup;
using Sentyll.Infrastructure.HealthChecks.OpenIdConnectServer.Startup;
using Sentyll.Infrastructure.HealthChecks.Rabbitmq.Startup;
using Sentyll.Infrastructure.HealthChecks.Redis.Startup;
using Sentyll.Infrastructure.HealthChecks.SqlServer.Startup;
using Sentyll.Infrastructure.HealthChecks.Uris.Startup;
using Microsoft.Extensions.DependencyInjection;
using Sentyll.Infrastructure.HealthChecks.Services;

namespace Sentyll.Infrastructure.HealthChecks.Startup;

public static class HealthChecksStartup
{
    
    public static IServiceCollection RegisterHealthCheckDependencies(this IServiceCollection serviceCollection)
        => serviceCollection
            .RegisterServices()
            .RegisterHealthCheckJobs();
    
    private static IServiceCollection RegisterHealthCheckJobs(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .RegisterOpenIdConnectServerHealthChecksDependencies()
            .RegisterAzureApplicationInsightsHealthChecksDependencies()
            .RegisterAzureKeyVaultHealthChecksDependencies()
            .RegisterAzureQueueStorageHealthChecksDependencies()
            .RegisterAzureServiceBusTopicHealthChecksDependencies()
            .RegisterAzureSignalrHealthChecksDependencies()
            .RegisterAzureBlobStorageHealthChecksDependencies()
            .RegisterAzureFileShareHealthChecksDependencies()
            .RegisterMongoDbHealthChecksDependencies()
            .RegisterNpqSqlDatabaseHealthChecksDependencies()
            .RegisterRabbitMqHealthChecksDependencies()
            .RegisterRedisHealthChecksDependencies()
            .RegisterSqlServerHealthChecksDependencies()
            .RegisterUriHealthChecksDependencies()
            .RegisterAzureFunctionAppHealthChecksDependencies();
        
        using var sp = serviceCollection.BuildServiceProvider();

        sp.ConfigureOpenIdConnectServerHealthChecks()
            .ConfigureAzureApplicationInsightsHealthChecks()
            .ConfigureAzureKeyVaultHealthChecks()
            .ConfigureAzureQueueStorageHealthChecks()
            .ConfigureAzureServiceBusTopicHealthChecks()
            .ConfigureAzureSignalrHealthChecks()
            .ConfigureAzureBlobStorageHealthChecks()
            .ConfigureAzureFileShareHealthChecks()
            .ConfigureMongoDbHealthChecks()
            .ConfigureNpqSqlDatabaseHealthChecks()
            .ConfigureRabbitMqHealthChecks()
            .ConfigureRedisHealthChecks()
            .ConfigureSqlServerHealthChecks()
            .ConfigureUriHealthChecks()
            .ConfigureAzureFunctionAppHealthChecks();

        return serviceCollection;
    }

    private static IServiceCollection RegisterServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IHealthCheckRegistrationService, HealthCheckRegistrationService>();
        serviceCollection.AddTransient<IHealthCheckJobDependencyManager, HealthCheckJobDependencyManager>();

        return serviceCollection;
    }
    
    
}