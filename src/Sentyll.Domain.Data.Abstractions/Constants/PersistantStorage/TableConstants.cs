namespace Sentyll.Domain.Data.Abstractions.Constants.PersistantStorage;

public static class TableConstants
{
    public const string HealthChecks = "HealthChecks";
    public const string HealthCheckParameters = "HealthCheckParameters";
    public const string HealthCheckEvents = "HealthCheckEvents";
    public const string HealthCheckExecutions = "HealthCheckExecutions";
    public const string HealthCheckExecutionHistories = "HealthCheckExecutionHistories";
    
    public const string Events = "Events";
    public const string EventParameters = "EventParameters";
    public const string EventExecutions = "EventExecutions";
    public const string EventCategories = "EventCategories";
    
    public const string ServerSettings = "ServerSettings";
    public const string Configurations = "Configurations";
    
    public const string CronJobs = "CronJobs";
    public const string TimerJobs = "TimerJobs";
    public const string CronJobOccurrences = "CronJobOccurrences";
}