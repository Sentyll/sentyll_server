using Sentyll.Domain.Data.Services.Abstractions.Contracts.Repositories.Events;
using Sentyll.Domain.Data.Services.Abstractions.Contracts.Repositories.HealthChecks;
using Sentyll.Domain.Data.Services.Abstractions.Contracts.Repositories.Scheduler;
using Sentyll.Domain.Data.Services.Abstractions.Contracts.Repositories.Settings;
using Sentyll.Domain.Data.Services.Abstractions.Contracts.Services.Events;
using Microsoft.Extensions.DependencyInjection;
using Sentyll.Domain.Data.Services.Repositories.Events;
using Sentyll.Domain.Data.Services.Repositories.HealthChecks;
using Sentyll.Domain.Data.Services.Repositories.Scheduler;
using Sentyll.Domain.Data.Services.Repositories.Settings;
using Sentyll.Domain.Data.Services.Services.Events;

namespace Sentyll.Domain.Data.Services.Startup;

public static class DataServicesStartup
{
    public static IServiceCollection RegisterDataDependencies(this IServiceCollection serviceCollection) 
        => serviceCollection
            .RegisterSchedulerRepositoryServices()
            .RegisterEventRepositoryServices()
            .RegisterEventDataServices()
            .RegisterSettingsRepositoryServices()
            .RegisterHealthCheckRepositoryServices();

    private static IServiceCollection RegisterSchedulerRepositoryServices(this IServiceCollection services) 
        => services
            .AddTransient<ICronJobEntityRepository, CronJobEntityRepository>()
            .AddTransient<ITimerJobEntityRepository, TimerJobEntityRepository>()
            .AddTransient<ICronJobOccurrenceEntityRepository, CronJobOccurrenceEntityRepository>();

    private static IServiceCollection RegisterEventRepositoryServices(this IServiceCollection services) 
        => services
            .AddTransient<IEventParameterEntityRepository, EventParameterEntityRepository>()
            .AddTransient<IEventCategoryEntityRepository, EventCategoryEntityRepository>()
            .AddTransient<IEventEntityRepository, EventEntityRepository>()
            .AddTransient<IEventExecutionEntityRepository, EventExecutionEntityRepository>();

    private static IServiceCollection RegisterEventDataServices(this IServiceCollection services)
        => services
            .AddTransient<IEventsParameterDataService, EventsParameterDataService>();

    private static IServiceCollection RegisterSettingsRepositoryServices(this IServiceCollection services) 
        => services
            .AddTransient<IServerSettingEntityRepository, ServerSettingEntityRepository>();

    private static IServiceCollection RegisterHealthCheckRepositoryServices(this IServiceCollection services)
        => services
            .AddTransient<IHealthCheckParameterEntityRepository, HealthCheckParameterEntityRepository>()
            .AddTransient<IHealthCheckEntityRepository, HealthCheckEntityRepository>()
            .AddTransient<IHealthCheckEventEntityRepository, HealthCheckEventEntityRepository>()
            .AddTransient<IHealthCheckExecutionEntityRepository, HealthCheckExecutionEntityRepository>()
            .AddTransient<IHealthCheckExecutionHistoryEntityRepository, HealthCheckExecutionHistoryEntityRepository>();
}