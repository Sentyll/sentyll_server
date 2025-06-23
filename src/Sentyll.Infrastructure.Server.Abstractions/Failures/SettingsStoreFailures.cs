namespace Sentyll.Infrastructure.Server.Abstractions.Failures;

public static class SettingsStoreFailures
{
    private const string Code = "SETST";
    
    public static readonly Failure NotFound = new(Code,"0001", "Target setting was not found");
    public static readonly Failure UnsupportedSettingType = new(Code,"0002","Target setting was not deserialized successfully");
    public static readonly Failure DeserializeFailed = new(Code,"0003","Target setting was not deserialized successfully");
    public static readonly Failure SerializeFailed = new(Code,"0004","Target setting was not serialized successfully");
}