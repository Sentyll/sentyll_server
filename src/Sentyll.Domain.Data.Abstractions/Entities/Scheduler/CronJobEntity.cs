using Sentyll.Domain.Data.Abstractions.Constants;
using Sentyll.Domain.Data.Abstractions.Constants.PersistantStorage;
using Sentyll.Domain.Data.Abstractions.Entities.Base;
using Sentyll.Domain.Data.Abstractions.EntityTypeConfigurations;

namespace Sentyll.Domain.Data.Abstractions.Entities.Scheduler;

[Table(TableConstants.CronJobs, Schema = SchemaConstants.Scheduler)]
[EntityTypeConfiguration(typeof(CronJobEntityConfiguration))]
public class CronJobEntity : JobEntity
{
    
    [StringLength(512)]
    public virtual string? Expression { get; set; }
    
    public virtual byte[]? Request { get; set; }
    
    [Required]
    public int Retries { get; set; }
    
    public int[]? RetryIntervals { get; set; }
}