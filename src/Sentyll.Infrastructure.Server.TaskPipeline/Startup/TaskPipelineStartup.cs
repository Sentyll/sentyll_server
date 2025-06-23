using System.Threading.Channels;
using Sentyll.Domain.Common.Abstractions.Contracts.Queues;
using Sentyll.Domain.Common.Abstractions.Queues;
using Sentyll.Infrastructure.Server.TaskPipeline.Abstraction.Contracts.Services.EventProcessors;
using Sentyll.Infrastructure.Server.TaskPipeline.Abstraction.Models.Queues.Events;
using Microsoft.Extensions.DependencyInjection;
using Sentyll.Infrastructure.Server.TaskPipeline.BackgroundServices;
using Sentyll.Infrastructure.Server.TaskPipeline.Core.Contracts.Services.EventDispatchers;
using Sentyll.Infrastructure.Server.TaskPipeline.Core.Contracts.Services.EventResolvers;
using Sentyll.Infrastructure.Server.TaskPipeline.Services.EventDispatchers;
using Sentyll.Infrastructure.Server.TaskPipeline.Services.EventProcessors;
using Sentyll.Infrastructure.Server.TaskPipeline.Services.EventResolvers;

namespace Sentyll.Infrastructure.Server.TaskPipeline.Startup;

public static class TaskPipelineStartup
{

    public static IServiceCollection RegisterTaskPipelineDependencies(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddBackgroundServices()
            .AddQueues()
            .AddEventDispatcher()
            .AddEventProcessors()
            .AddEventResolvers();
    
    private static IServiceCollection AddBackgroundServices(this IServiceCollection serviceCollection) 
        => serviceCollection.AddHostedService<HealthCheckExecutionQueueConsumer>();

    private static IServiceCollection AddQueues(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IUnboundedInMemoryQueue<HealthCheckJobExecutionEvent>>((sp) =>
        {
            //TODO: Should this potentially be configurable from app settings?
            var queueOptions = new UnboundedChannelOptions()
            {
                SingleWriter = false,
                SingleReader = true,
                AllowSynchronousContinuations = false
            };
            
            return new UnboundedInMemoryQueue<HealthCheckJobExecutionEvent>(queueOptions);
        });
        
        return serviceCollection;
    }
    
    private static IServiceCollection AddEventDispatcher(this IServiceCollection serviceCollection) 
        => serviceCollection
            .AddTransient<IEventDispatcher, MessagingEventDispatcher>()
            .AddTransient<IEventDispatcher, WebhookEventDispatcher>()
            .AddSingleton<IEventDispatcher, ServerEventDispatcher>();

    private static IServiceCollection AddEventProcessors(this IServiceCollection serviceCollection) 
        => serviceCollection.AddSingleton<IServerEventProcessor, LoggerServerEventProcessor>();
    
    private static IServiceCollection AddEventResolvers(this IServiceCollection serviceCollection) 
        => serviceCollection.AddTransient<IEventDescriptorProvider, EventDescriptorProvider>();
}