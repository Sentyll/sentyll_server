using Sentyll.Domain.Data.Abstractions.Constants.PersistantStorage;
using Sentyll.Domain.Data.Abstractions.Entities.Base;
using Sentyll.Domain.Data.Abstractions.Entities.HealthChecks;

namespace Sentyll.Domain.Data.Abstractions.Entities.Events;

[Table(TableConstants.EventExecutions, Schema = SchemaConstants.Event)]
public class EventExecutionEntity : IdentityEntity
{
    
    [Required] 
    public TimeSpan Duration { get; set; }

    [Required] 
    public DateTime ExecutedOn { get; set; }
    
    [Required] 
    public bool IsSuccess { get; set; }
    
    [StringLength(2058)] 
    public string? Description { get; set; }
    
    [Required]
    public Guid EventId { get; set; }

    [ForeignKey(nameof(EventId))]
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public virtual EventEntity Event { get; set; }
    
    [Required]
    public Guid ExecutionId { get; set; }
    
    [ForeignKey(nameof(ExecutionId))]
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public virtual HealthCheckExecutionEntity Execution { get; set; }
    
}