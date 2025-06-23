using Sentyll.Domain.Data.Abstractions.Constants.Seeds.Settings;
using Sentyll.Infrastructure.Server.Context;

namespace Sentyll.Infrastructure.Server.Stores;

internal sealed class ServerSettingsStore : IServerSettingsStore
{

    private readonly ServerSettingsContext _ctx;

    public ServerSettingsStore()
    {
        _ctx = ServerSettingsContext.GetInstance();
    }

    public Result LazyLoad(IDictionary<string, string> settings)
    {
        foreach (var setting in settings)
        {
            var addResult = _ctx.AddOrUpdate(setting.Key, setting.Value);
            if (!addResult.IsSuccess)
            {
                return Result.Failure(addResult.Error);
            }
        }
        
        return Result.Success();
    }

    public Result<int> MaxActivateApiRequests
    {
        get => _ctx.Get<int>(ServerSettingEntitiesConstants.MaxActivateApiRequests.Key);
        set => _ctx.AddOrUpdate(ServerSettingEntitiesConstants.MaxActivateApiRequests.Key, value.Value);
    }
    
    public Result<int> MinimumSecondsBetweenFailureNotifications
    {
        get => _ctx.Get<int>(ServerSettingEntitiesConstants.MinimumSecondsBetweenFailureNotifications.Key);
        set => _ctx.AddOrUpdate(ServerSettingEntitiesConstants.MinimumSecondsBetweenFailureNotifications.Key, value.Value);
    }
    
    public Result<bool> NotifyUnHealthyOneTimeUntilChange
    {
        get => _ctx.Get<bool>(ServerSettingEntitiesConstants.NotifyUnHealthyOneTimeUntilChange.Key);
        set => _ctx.AddOrUpdate(ServerSettingEntitiesConstants.NotifyUnHealthyOneTimeUntilChange.Key, value.Value);
    }
    
    public Result<bool> ExecutionsCanExpire
    {
        get => _ctx.Get<bool>(ServerSettingEntitiesConstants.ExecutionsCanExpire.Key);
        set => _ctx.AddOrUpdate(ServerSettingEntitiesConstants.ExecutionsCanExpire.Key, value.Value);
    }
    
    public Result<int> MaxExecutionsRetentionPeriodInDays
    {
        get => _ctx.Get<int>(ServerSettingEntitiesConstants.MaxExecutionsRetentionPeriodInDays.Key);
        set => _ctx.AddOrUpdate(ServerSettingEntitiesConstants.MaxExecutionsRetentionPeriodInDays.Key, value.Value);
    }
    
    public Result<bool> ExecutionHistoriesCanExpire
    {
        get => _ctx.Get<bool>(ServerSettingEntitiesConstants.ExecutionHistoriesCanExpire.Key);
        set => _ctx.AddOrUpdate(ServerSettingEntitiesConstants.ExecutionHistoriesCanExpire.Key, value.Value);
    }
    
    public Result<int> MaxExecutionHistoriesRetentionPeriodInDays
    {
        get => _ctx.Get<int>(ServerSettingEntitiesConstants.MaxExecutionHistoriesRetentionPeriodInDays.Key);
        set => _ctx.AddOrUpdate(ServerSettingEntitiesConstants.MaxExecutionHistoriesRetentionPeriodInDays.Key, value.Value);
    }
    
}