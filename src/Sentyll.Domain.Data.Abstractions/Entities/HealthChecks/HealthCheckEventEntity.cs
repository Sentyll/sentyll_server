using Sentyll.Domain.Data.Abstractions.Constants.PersistantStorage;
using Sentyll.Domain.Data.Abstractions.Entities.Base;
using Sentyll.Domain.Data.Abstractions.Entities.Events;

namespace Sentyll.Domain.Data.Abstractions.Entities.HealthChecks;

[Table(TableConstants.HealthCheckEvents, Schema = SchemaConstants.HealthCheck)]
public class HealthCheckEventEntity : ActivatableEntity
{
    
    [Required]
    public Guid EventId { get; set; }

    [ForeignKey(nameof(EventId))]
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public virtual EventEntity Event { get; set; }
    
    [Required]
    public Guid HealthCheckId { get; set; }

    [ForeignKey(nameof(HealthCheckId))]
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public virtual HealthCheckEntity HealthCheck { get; set; }
    
}