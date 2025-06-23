using Sentyll.Domain.Common.Abstractions.Enums;

namespace Sentyll.Domain.Common.Abstractions.Models.Health;

public record HealthStatusReport()
{

    public string DiscoveryServiceName { get; init; }
    
    public HealthCheckStatus Status { get; init; }
    
    public TimeSpan TotalDuration { get; init; }
    
    public Dictionary<string, HealthStatusReportEntry> Entries { get; init; }

    public HealthStatusReport(
        string discoveryServiceName,
        Dictionary<string, HealthStatusReportEntry>? entries, 
        TimeSpan totalDuration) : this()
    {
        DiscoveryServiceName = discoveryServiceName;
        Entries = entries ?? new Dictionary<string, HealthStatusReportEntry>();
        TotalDuration = totalDuration;
    }

    public static HealthStatusReport CreateFrom(
        string discoveryServiceName, 
        HealthReport report, 
        Func<Exception, string>? exceptionMessage = null)
    {
        var uiReport = new HealthStatusReport(
            discoveryServiceName, 
            new Dictionary<string, HealthStatusReportEntry>(), 
            report.TotalDuration
            )
        {
            Status = (HealthCheckStatus)report.Status
        };

        foreach (var item in report.Entries)
        {
            var entry = new HealthStatusReportEntry
            {
                Data = item.Value.Data,
                Description = item.Value.Description,
                Duration = item.Value.Duration,
                Status = (HealthCheckStatus)item.Value.Status
            };

            if (item.Value.Exception != null)
            {
                var message = exceptionMessage == null ? item.Value.Exception?.Message : exceptionMessage(item.Value.Exception);

                entry.Exception = message;
                entry.Description = item.Value.Description ?? message;
            }

            entry.Tags = item.Value.Tags;

            uiReport.Entries.Add(item.Key, entry);
        }

        return uiReport;
    }

    public static HealthStatusReport CreateFrom(
        string discoveryServiceName, 
        Exception exception, 
        string entryName = "Endpoint")
    {
        var uiReport = new HealthStatusReport(
            discoveryServiceName, 
            new Dictionary<string, HealthStatusReportEntry>(),
            TimeSpan.FromSeconds(0)
            )
        {
            Status = HealthCheckStatus.Unhealthy
        };

        uiReport.Entries.Add(entryName, new HealthStatusReportEntry
        {
            Exception = exception.Message,
            Description = exception.Message,
            Duration = TimeSpan.FromSeconds(0),
            Status = HealthCheckStatus.Unhealthy
        });

        return uiReport;
    }
};