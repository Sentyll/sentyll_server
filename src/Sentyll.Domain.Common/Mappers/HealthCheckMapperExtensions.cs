namespace Sentyll.Domain.Common.Mappers;

public static class HealthCheckMapperExtensions
{
    
    public static HealthStatus MapToHealthStatus(this HealthCheckStatus status)
        => status switch
        {
            HealthCheckStatus.Unhealthy => HealthStatus.Unhealthy,
            HealthCheckStatus.Degraded => HealthStatus.Degraded,
            HealthCheckStatus.Healthy => HealthStatus.Healthy,
            _ => throw new ArgumentOutOfRangeException()
        };
    
    public static HealthCheckStatus MapToHealthCheckStatus(this HealthStatus status)
        => status switch
        {
            HealthStatus.Unhealthy => HealthCheckStatus.Unhealthy,
            HealthStatus.Degraded => HealthCheckStatus.Degraded,
            HealthStatus.Healthy => HealthCheckStatus.Healthy,
            _ => throw new ArgumentOutOfRangeException()
        };
    
    public static HealthReport MapToHealthReport(this HealthStatusReport report)
        => new(
            report.Entries.MapToHealthReportEntries(), 
            report.Status.MapToHealthStatus(),
            report.TotalDuration);

    public static Dictionary<string, HealthReportEntry> MapToHealthReportEntries(
        this Dictionary<string, HealthStatusReportEntry> entries)
        => entries
            .Select(entry => new KeyValuePair<string, HealthReportEntry>(
                    entry.Key,
                    entry.Value.MapToHealthReportEntry()
                )
            )
            .ToDictionary();

    public static HealthReportEntry MapToHealthReportEntry(this HealthStatusReportEntry entry)
        => new(
            entry.Status.MapToHealthStatus(),
            entry.Description,
            entry.Duration,
            new Exception(entry.Exception),
            entry.Data
        );
    
    public static List<HealthCheckExecutionEntry> ToExecutionEntries(this HealthStatusReport report)
    {
        return report.Entries
            .Select(item => new HealthCheckExecutionEntry
            {
                Name = item.Key,
                Status = item.Value.Status,
                Description = item.Value.Description,
                Duration = item.Value.Duration,
                Tags = item.Value.Tags?.ToList() ?? null
            }).ToList();
    }
    
    public static Dictionary<string, HealthStatusReportEntry> ExtractUnhealthyEntries(this HealthStatusReport report)
        => report.Entries
            .Where(entry => entry.Value.Status != HealthCheckStatus.Healthy)
            .ToDictionary();
    
}