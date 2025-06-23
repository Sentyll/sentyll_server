using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Sentyll.Infrastructure.Events.Messaging.Abstractions.Constants;

public static class NotificationVerbiageConstants
{

    public static string HEADLINE_FAILING_TITLE(int failureCount) 
        => $"There {(failureCount > 1 ? "are" : "is")} at least {failureCount} {(failureCount > 1 ? "healthchecks" : "healthcheck")} failing.";

    public static string HEADLINE_RESTORED_TITLE(int restoreCount) 
        => $"{restoreCount} {(restoreCount > 1 ? "healthchecks" : "healthcheck")} has successfully recovered.";

    public const string HEADLINE_FAILING_DESCRIPTION = "We've detected that one or more of your configured health checks have failed. Please review the affected services to ensure system stability and address any potential issues promptly.";

    public const string HEADLINE_RESTORED_DESCRIPTION = "Your services are now reporting healthy statuses. Please review the system to confirm stability and ensure that everything is operating as expected.";
    
    public const string ACTIONS_GOTODASHBOARD = "Go to dashboard";
    
    public const string ACTIONS_VIEWDETAILS = "View Details";
    
    public const string ACTIONS_ACTIONFAILURE = "Action Failure";

    public const string HEALTHCHECK_DETAILS_HEALTHSTATUS = "Health Status:";
    
    public const string HEALTHCHECK_DETAILS_TRIGGEREDON = "Triggered on:";
    
    public const string HEALTHCHECK_DETAILS_OVERVIEW = "Health Overview:";
    
    public static string HealthStatusChip(HealthStatus status)
        => status switch
        {
            HealthStatus.Healthy => "🟢 Healthy",
            HealthStatus.Degraded => "🟡 Degraded",
            _ => "🔴 Unhealthy"
        };
}