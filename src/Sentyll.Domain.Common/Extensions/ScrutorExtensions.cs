using Microsoft.Extensions.DependencyInjection;

namespace Sentyll.Domain.Common.Extensions;

public static class ScrutorExtensions
{

    public static IServiceCollection DecorateKeyedSingleton<TService, TDecorator>(
        this IServiceCollection serviceCollection,
        string serviceKey,
        string decoratorKey) 
        where TService : class
        where TDecorator : class, TService =>
        serviceCollection.AddKeyedSingleton<TService>(decoratorKey, (sp, key) =>
        {
            var service = sp.GetRequiredKeyedService<TService>(serviceKey);
            return ActivatorUtilities.CreateInstance<TDecorator>(sp, service);
        });
    
    public static IServiceCollection DecorateKeyedScoped<TService, TDecorator>(
        this IServiceCollection serviceCollection,
        string serviceKey,
        string decoratorKey) 
        where TService : class
        where TDecorator : class, TService =>
        serviceCollection.AddKeyedScoped<TService>(decoratorKey, (sp, key) =>
        {
            var service = sp.GetRequiredKeyedService<TService>(serviceKey);
            return ActivatorUtilities.CreateInstance<TDecorator>(sp, service);
        });
    
    public static IServiceCollection DecorateKeyedTransient<TService, TDecorator>(
        this IServiceCollection serviceCollection,
        string serviceKey,
        string decoratorKey) 
        where TService : class
        where TDecorator : class, TService =>
        serviceCollection.AddKeyedTransient<TService>(decoratorKey, (sp, key) =>
        {
            var service = sp.GetRequiredKeyedService<TService>(serviceKey);
            return ActivatorUtilities.CreateInstance<TDecorator>(sp, service);
        });
}