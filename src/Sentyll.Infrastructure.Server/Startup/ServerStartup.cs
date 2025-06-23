using Sentyll.Core.Services.Startup;
using Sentyll.Domain.Data.Services.Startup;
using Sentyll.Domain.Data.Sqlite.Startup;
using Sentyll.Infrastructure.Events.Messaging.Startup;
using Sentyll.Infrastructure.Events.WebHooks.Startup;
using Sentyll.Infrastructure.HealthChecks.Startup;
using Sentyll.Infrastructure.Server.Scheduler.Startup;
using Sentyll.Infrastructure.Server.TaskPipeline.Startup;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Sentyll.Infrastructure.Server.Builders;

namespace Sentyll.Infrastructure.Server.Startup;

public static class ServerStartup
{
    public static IServerSettingsStore AddSentyllServer(this WebApplicationBuilder applicationBuilder)
    {
        return new SentyllServerWebApplicationBuilder(applicationBuilder)
            .ConfigureAppConfiguration((configuration) =>
            {
                var dataOptions = applicationBuilder.Configuration
                    .GetSection("Sentyll")
                    .Get<SentyllServerOptions>() ?? throw new RottenBanana(DependencyFailures.ConfigurationNotFound);

                return dataOptions;
            })
            .ConfigureContext((appBuilder, storageOptions) =>
            {
                appBuilder.Services.AddSqlitePersistantStorage(storageOptions.ConnectionString);
            })
            .ConfigureServices((appBuilder, store) =>
            {
                appBuilder.Services.RegisterDataDependencies();
                appBuilder.Services.RegisterCoreServicesDependencies();
                appBuilder.Services.RegisterServerDependencies();
                // appBuilder.Services.RegisterNotificationDependencies();
                appBuilder.Services.RegisterTaskPipelineDependencies();
                appBuilder.Services.RegisterWebHookDependencies();
                appBuilder.Services.RegisterSchedulerDependencies(schedulerOptions =>
                {
                    schedulerOptions.SetMaxConcurrency(1);
                    schedulerOptions.CancelMissedJobsOnApplicationRestart();
                    schedulerOptions.SetTimeOutJobChecker(TimeSpan.FromMinutes(5));
                    schedulerOptions.SetInstanceIdentifier("Local PC");
                });
                
                appBuilder.Services.RegisterHealthCheckDependencies();
                appBuilder.Services.RegisterMessagingDependencies();
            })
            .Build();
    }

    public static WebApplication UseSentyllServer(this WebApplication application)
    {
        var builder = new SentyllApplicationBuilder(application)
            .UseServices(appBuilder =>
            {
                appBuilder.UseScheduler();
            })
            .MapEndpoints((endpointBuilder) =>
            {
                
            });

        return application;
    }
    
}