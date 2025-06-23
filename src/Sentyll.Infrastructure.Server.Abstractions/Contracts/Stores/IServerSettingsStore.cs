namespace Sentyll.Infrastructure.Server.Abstractions.Contracts.Stores;

public interface IServerSettingsStore
{
    Result LazyLoad(IDictionary<string, string> settings);
    
    Result<int> MaxActivateApiRequests { get; set; }
    Result<int> MinimumSecondsBetweenFailureNotifications { get; set; }
    Result<bool> NotifyUnHealthyOneTimeUntilChange { get; set; }
    Result<bool> ExecutionsCanExpire{ get; set; }
    Result<int> MaxExecutionsRetentionPeriodInDays { get; set; }
    Result<bool> ExecutionHistoriesCanExpire { get; set; }
    Result<int> MaxExecutionHistoriesRetentionPeriodInDays { get; set; }
}