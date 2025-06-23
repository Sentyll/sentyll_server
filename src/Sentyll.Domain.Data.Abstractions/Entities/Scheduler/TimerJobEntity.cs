using Sentyll.Domain.Common.Abstractions.Enums.Scheduler;
using Sentyll.Domain.Data.Abstractions.Constants;
using Sentyll.Domain.Data.Abstractions.Constants.PersistantStorage;
using Sentyll.Domain.Data.Abstractions.Entities.Base;
using Sentyll.Domain.Data.Abstractions.EntityTypeConfigurations;

namespace Sentyll.Domain.Data.Abstractions.Entities.Scheduler;

[Table(TableConstants.TimerJobs, Schema = SchemaConstants.Scheduler)]
[EntityTypeConfiguration(typeof(TimerJobEntityConfiguration))]
public class TimerJobEntity : JobEntity
{
    [Required]
    public SchedulerJobStatus Status { get; set; }
    
    [StringLength(512)]
    public string? LockHolder { get; set; }
    
    public byte[]? Request { get; set; }
    
    [Required]
    public DateTime ExecutionTime { get; set; }
    
    public DateTime? LockedAt { get; set; }
    
    public DateTime? ExecutedAt { get; set; }
    
    public string? Exception { get; set; }
    
    [Required]
    public long ElapsedTime { get; set; }
    
    [Required]
    public int Retries { get; set; }
    
    [Required]
    public int RetryCount { get; set; }
    
    public int[]? RetryIntervals { get; set; }
}