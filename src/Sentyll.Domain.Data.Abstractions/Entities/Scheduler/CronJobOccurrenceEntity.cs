using Sentyll.Domain.Common.Abstractions.Enums.Scheduler;
using Sentyll.Domain.Data.Abstractions.Constants;
using Sentyll.Domain.Data.Abstractions.Constants.PersistantStorage;
using Sentyll.Domain.Data.Abstractions.Entities.Base;
using Sentyll.Domain.Data.Abstractions.EntityTypeConfigurations;

namespace Sentyll.Domain.Data.Abstractions.Entities.Scheduler;

[Table(TableConstants.CronJobOccurrences, Schema = SchemaConstants.Scheduler)]
[EntityTypeConfiguration(typeof(CronJobOccurrenceEntityConfiguration))]
public class CronJobOccurrenceEntity : IdentityEntity
{
    
    [Required]
    public SchedulerJobStatus Status { get; set; }
    
    [StringLength(512)]
    public string? LockHolder { get; set; }
    
    [Required]
    public DateTime ExecutionTime { get; set; }
    
    public DateTime? LockedAt { get; set; }
    
    public DateTime? ExecutedAt { get; set; }
    
    [StringLength(512)]
    public string? Exception { get; set; }
    
    [Required]
    public long ElapsedTime { get; set; }
    
    [Required]
    public int RetryCount { get; set; }
    
    [Required]
    public Guid CronJobId { get; set; }
    
    [ForeignKey(nameof(CronJobId))]
    public virtual CronJobEntity CronJob { get; set; }
}