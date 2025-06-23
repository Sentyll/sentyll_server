using Sentyll.Domain.Data.Abstractions.Constants.PersistantStorage;
using Sentyll.Domain.Data.Abstractions.Entities.Base;

namespace Sentyll.Domain.Data.Abstractions.Entities.HealthChecks;

[Table(TableConstants.HealthCheckExecutions, Schema = SchemaConstants.HealthCheck)]
public class HealthCheckExecutionEntity : IdentityEntity
{
    
    [StringLength(2058)] 
    public string? Description { get; set; }
    
    [Required] 
    public HealthCheckStatus HealthStatus { get; set; }

    [Required] 
    public TimeSpan Duration { get; set; }

    [Required] 
    public DateTime ExecutedOn { get; set; }
    
    [Required]
    public Guid HealthCheckId { get; set; }

    [ForeignKey(nameof(HealthCheckId))]
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public virtual HealthCheckEntity HealthCheck { get; set; }
    
}