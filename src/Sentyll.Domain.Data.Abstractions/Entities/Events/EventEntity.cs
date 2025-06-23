using Sentyll.Domain.Data.Abstractions.Constants.PersistantStorage;
using Sentyll.Domain.Data.Abstractions.Entities.Base;

namespace Sentyll.Domain.Data.Abstractions.Entities.Events;

[Table(TableConstants.Events, Schema = SchemaConstants.Event)]
public class EventEntity : ActivatableEntity
{
    
    [Required]
    [StringLength(512)]
    public string Name { get; set; }
    
    [Required]
    [StringLength(1024)]
    public string? Description { get; set; }
    
    public string[]? Tags { get; set; }
    
    [Required]
    public Guid EventCategoryId { get; set; }

    [ForeignKey(nameof(EventCategoryId))]
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public virtual EventCategoryEntity EventCategory { get; set; }

    public virtual IList<EventParameterEntity> Parameters { get; set; }
    
    public virtual IList<EventExecutionEntity> Executions { get; set; }
    
}
