using Sentyll.Domain.Common.Mappers.Parameters;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sentyll.Infrastructure.Server.Stores;

namespace Sentyll.Infrastructure.Server.Builders;

public class SentyllServerWebApplicationBuilder
{
    
    private SentyllServerOptions SentyllServerOptions { get; set; }
    private IServerSettingsStore ServerSettingsStore { get; set; }
    
    
    private readonly WebApplicationBuilder _webApplication;
    
    public SentyllServerWebApplicationBuilder(WebApplicationBuilder webApplication)
    {
        _webApplication = webApplication;
    }

    /// <summary>
    /// STEP 1: Bind external configuration sources if applicable
    /// </summary>
    /// <param name="buildStorageFunc"></param>
    /// <returns></returns>
    public SentyllServerWebApplicationBuilder ConfigureAppConfiguration(Func<IConfiguration, SentyllServerOptions> buildStorageFunc)
    {
        var options = buildStorageFunc(
            _webApplication.Configuration
        );
        
        SentyllServerOptions = options ?? throw new ArgumentNullException();
        
        return this;
    }
    
    /// <summary>
    /// STEP 2: Based on the Data Source Provider, Register the Context with provider specific options.
    /// </summary>
    /// <param name="configureAction"></param>
    /// <returns></returns>
    public SentyllServerWebApplicationBuilder ConfigureContext(Action<WebApplicationBuilder, SentyllServerOptions> configureAction)
    {
        configureAction(
            _webApplication,
            SentyllServerOptions
        );
        
        EncryptionExtension.SetEncryptionKey(SentyllServerOptions.EncryptionKey);

        var sp = _webApplication.Services.BuildServiceProvider();
        var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
        using var scope = scopeFactory.CreateScope();
        
        var context = scope.ServiceProvider.GetRequiredService<SentyllContext>();
        
        var shouldMigrate = !SentyllServerOptions.DisableMigrations &&
            !context.Database.IsInMemory() &&
            context.Database.GetPendingMigrations().Any();
        
        if (shouldMigrate)
        {
            context.Database.Migrate();
        }
        
        SetServerSettingsStore(context);
        
        return this;
    }
    
    public SentyllServerWebApplicationBuilder ConfigureServices(Action<WebApplicationBuilder, IServerSettingsStore> configureAction)
    {
        configureAction(
            _webApplication,
            ServerSettingsStore
        );
        
        return this;
    }

    public IServerSettingsStore Build() => ServerSettingsStore;
    
    private void SetServerSettingsStore(SentyllContext ctx)
    {
        var serverSettingsStore = new ServerSettingsStore();
        
        var settings = ctx.ServerSettings.MapToDictionary();
        var lazyLoadResult = serverSettingsStore.LazyLoad(settings);

        ServerSettingsStore = serverSettingsStore;
    }
}
