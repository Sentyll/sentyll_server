using System.Collections.Concurrent;
using System.Text.Json;

namespace Sentyll.Infrastructure.Server.Context;

internal sealed class ServerSettingsContext
{
    
    private readonly ConcurrentDictionary<string, object> _concurrentStore = new();
    private static readonly ServerSettingsContext Context = new();
    
    private ServerSettingsContext()
    {
        Console.WriteLine("Singleton server setting context has been initialized.");
    }
    
    public static ServerSettingsContext GetInstance() => Context;

    public Result<bool> AddOrUpdate<T>(string key, T value)
    {
        if (_concurrentStore.ContainsKey(key))
        {
            _concurrentStore.TryRemove(key, out _);
        }

        var serializedSetting = (value is string stringSetting)
            ? stringSetting
            : JsonSerializer.Serialize(value);
        
        if (string.IsNullOrWhiteSpace(serializedSetting))
        {
            return Result.Failure<bool>(SettingsStoreFailures.SerializeFailed);
        }
        
        return _concurrentStore.TryAdd(key, serializedSetting);
    }

    public Result<T> Get<T>(string key)
    {
        if (!_concurrentStore.TryGetValue(key, out var value))
        {
            return Result.Failure<T>(SettingsStoreFailures.NotFound);
        }

        if (value is not string stringSetting)
        {
            return Result.Failure<T>(SettingsStoreFailures.UnsupportedSettingType);
        }
        
        var serializedSetting = JsonSerializer.Deserialize<T>(stringSetting);
        if (serializedSetting is null)
        {
            return Result.Failure<T>(SettingsStoreFailures.DeserializeFailed);
        }

        return serializedSetting;
    }

    public Result<bool> Remove(string key)
    {
        return _concurrentStore.TryRemove(key, out _);
    }

    public Result Clear()
    {
        _concurrentStore.Clear();
        return Result.Success();
    }
    
}