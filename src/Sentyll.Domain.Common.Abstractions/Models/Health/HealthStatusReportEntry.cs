using Sentyll.Domain.Common.Abstractions.Enums;

namespace Sentyll.Domain.Common.Abstractions.Models.Health;

public record HealthStatusReportEntry
{
    
    public IReadOnlyDictionary<string, object> Data { get; set; } = null!;
    
    public string? Description { get; set; }
    
    public TimeSpan Duration { get; set; }
    
    public string? Exception { get; set; }
    
    public HealthCheckStatus Status { get; set; }
    
    public IEnumerable<string>? Tags { get; set; }
    
}