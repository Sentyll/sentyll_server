using Sentyll.Domain.Data.Abstractions.Entities.Settings;

namespace Sentyll.Domain.Data.Abstractions.Constants.Seeds.Settings;

public static class ServerSettingEntitiesConstants
{

    public static ServerSettingEntity MaxActivateApiRequests => new()
    {
        Id = Guid.Parse("A7BDF726-E589-4527-9ACD-B76D5940BECF"),
        Key = "SETTINGS:API:MAX_ACTIVE_REQUESTS",
        Value = "50"
    };
    
    public static ServerSettingEntity MinimumSecondsBetweenFailureNotifications => new()
    {
        Id = Guid.Parse("12C60D84-596E-4E01-8900-C3ACEB312A4B"),
        Key = "SETTINGS:NOTIFICATIONS:MINIMUM_SECONDS_BETWEEN_FAILURE_NOTIFICATIONS",
        Value = "30"
    };
    
    public static ServerSettingEntity NotifyUnHealthyOneTimeUntilChange => new()
    {
        Id = Guid.Parse("2E1EFD99-3C49-467D-BF51-F1D40E03CD45"),
        Key = "SETTINGS:NOTIFICATIONS:NOTIFY_UNHEALTHY_ONE_TIME_UNTIL_CHANGE",
        Value = "true"
    };
    
    public static ServerSettingEntity ExecutionsCanExpire => new()
    {
        Id = Guid.Parse("578C5F34-6C78-417F-A8D7-D502A7900A79"),
        Key = "SETTINGS:POLICIES:EXECUTIONS_CAN_EXPIRE",
        Value = "true"
    };
    
    public static ServerSettingEntity MaxExecutionsRetentionPeriodInDays => new()
    {
        Id = Guid.Parse("9756B55A-7102-4397-87CA-4E918723843B"),
        Key = "SETTINGS:POLICIES:MAX_EXECUTION_RETENTION_PERIOD_IN_DAYS",
        Value = "45"
    };
    
    public static ServerSettingEntity ExecutionHistoriesCanExpire => new()
    {
        Id = Guid.Parse("BCF0E4C4-96E1-46F5-BD84-171B217926E9"),
        Key = "SETTINGS:POLICIES:EXECUTION_HISTORIES_CAN_EXPIRE",
        Value = "true"
    };
    
    public static ServerSettingEntity MaxExecutionHistoriesRetentionPeriodInDays => new()
    {
        Id = Guid.Parse("1097F95D-376A-463B-B02F-86832E2C4516"),
        Key = "SETTINGS:POLICIES:MAX_EXECUTION_HISTORIES_RETENTION_PERIOD_IN_DAYS",
        Value = "365"
    };
}
