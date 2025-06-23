using Sentyll.Domain.Common.Abstractions.Enums;

namespace Sentyll.Domain.Common.Abstractions.Models.Health;

public sealed record HealthCheckExecutionEntry
{
    public string Name { get; set; }
    
    public string? Description { get; set; }
    
    public TimeSpan Duration { get; set; }
    
    public HealthCheckStatus Status { get; set; }
    
    public IEnumerable<string>? Tags { get; set; }
}