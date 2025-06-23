using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Services.Jobs;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Sentyll.Infrastructure.Server.Scheduler.Core.Contracts.Services.Host;
using Sentyll.Infrastructure.Server.Scheduler.Core.Models.Options;
using Sentyll.Infrastructure.Server.Scheduler.Services.Host;
using Sentyll.Infrastructure.Server.Scheduler.Services.Jobs;
using Sentyll.Infrastructure.Server.Scheduler.Services.Notifiers;
using Sentyll.Infrastructure.Server.Scheduler.Services.Scheduler;
using Host_SchedulerHost = Sentyll.Infrastructure.Server.Scheduler.Services.Host.SchedulerHost;
using SchedulerHost = Sentyll.Infrastructure.Server.Scheduler.Services.Host.SchedulerHost;

namespace Sentyll.Infrastructure.Server.Scheduler.Startup;

public static class ServerSchedulerStartup
{

    public static IServiceCollection RegisterSchedulerDependencies(
        this IServiceCollection serviceCollection,
        Action<SchedulerOptions> configureScheduler)
        => serviceCollection
            .ConfigureSchedulerOptions(configureScheduler)
            .RegisterHostServices()
            .RegisterNotifiers()
            .RegisterSchedulerServices()
            .RegisterJobServices();

    public static IApplicationBuilder UseScheduler(this IApplicationBuilder app)
    {
        using var useScope = app.ApplicationServices.CreateScope();
        
        var schedulerOptions = useScope.ServiceProvider.GetRequiredService<SchedulerOptions>();
        var internalJobManager = useScope.ServiceProvider.GetRequiredService<ISchedulerHostStateManager>();
        var jobStoreManager = useScope.ServiceProvider.GetRequiredService<IJobProviderManager>();

        app.UseSchedulerHostEvents(schedulerOptions);
        
        var functionsToSeed = jobStoreManager.GetJobSchedules();
        
        internalJobManager
            .SyncJobsWithPersistantStorageAsync(functionsToSeed)
            .GetAwaiter()
            .GetResult();

        internalJobManager
            .ReleaseOrCancelAllLockedJobsAsync(schedulerOptions.CancelMissedJobsOnReset)
            .GetAwaiter()
            .GetResult();

        var jobHost = app.ApplicationServices.GetRequiredService<ISchedulerHost>();

        jobHost.Start();

        return app;
    }
    
    private static IServiceCollection RegisterHostServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ISchedulerHost, Host_SchedulerHost>();
        serviceCollection.AddScoped<ISchedulerHostExceptionHandler, SchedulerHostHostExceptionHandler>();
        
        return serviceCollection;
    }
    
    private static IServiceCollection RegisterJobServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IJobProviderManager, JobProviderManager>();
        
        return serviceCollection;
    }
    
    private static IServiceCollection RegisterNotifiers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<INotificationHub, NotificationHub>();
        
        //Register notifier to be invoked by the notification hub above.
        serviceCollection.AddSingleton<IJobStateChangeNotifier, JobStateChangeLoggerNotifier>();
        serviceCollection.AddSingleton<IHostStateChangeNotifier, HostStateChangeLoggerNotifier>();
        
        return serviceCollection;
    }
    
    private static IServiceCollection RegisterSchedulerServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ISchedulerHostStateManager, SchedulerHostStateManager>();
        serviceCollection.AddScoped<ISchedulerJobStateManager, SchedulerJobStateManager>();
        
        return serviceCollection;
    }
    
    private static IServiceCollection ConfigureSchedulerOptions(
        this IServiceCollection serviceCollection,
        Action<SchedulerOptions> configureScheduler)
    {
        var optionInstance = new SchedulerOptions();

        configureScheduler(optionInstance);

        if (optionInstance.MaxConcurrency <= 0)
        {
            optionInstance.SetMaxConcurrency(Environment.ProcessorCount);
        }
        
        if (string.IsNullOrEmpty(optionInstance.InstanceIdentifier))
        {
            optionInstance.SetInstanceIdentifier(Environment.MachineName);
        }

        serviceCollection.AddSingleton<SchedulerOptions>(_ => optionInstance);

        return serviceCollection;
    }

    private static void UseSchedulerHostEvents(
        this IApplicationBuilder app, 
        SchedulerOptions options)
    {
        options.SubscribeToHostEvents(
            notifyThreadCountFunc: (threadCount) =>
            {
                using var scope = app.ApplicationServices.CreateScope();
                var notificationHubSender = scope.ServiceProvider.GetRequiredService<INotificationHub>();
                notificationHubSender.NotifyActiveThreads(threadCount);
            },
            notifyNextOccurenceFunc: (nextOccurrence) =>
            {
                using var scope = app.ApplicationServices.CreateScope();
                var notificationHubSender = scope.ServiceProvider.GetRequiredService<INotificationHub>();
                notificationHubSender.NotifyUpdateNextOccurrence(nextOccurrence);
            },
            notifyHostRunningFunc: (active) =>
            {
                using var scope = app.ApplicationServices.CreateScope();
                var notificationHubSender = scope.ServiceProvider.GetRequiredService<INotificationHub>();
                notificationHubSender.NotifyHostStatus(active);
            },
            hostExceptionMessageFunc: (message) =>
            {
                using var scope = app.ApplicationServices.CreateScope();
                var notificationHubSender = scope.ServiceProvider.GetRequiredService<INotificationHub>();
                notificationHubSender.NotifyHostException(message);
            }
        );
    }
    
}