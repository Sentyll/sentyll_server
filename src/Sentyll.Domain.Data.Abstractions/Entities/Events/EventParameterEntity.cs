using Sentyll.Domain.Data.Abstractions.Constants.PersistantStorage;
using Sentyll.Domain.Data.Abstractions.Entities.Base;
using Sentyll.Domain.Data.Abstractions.Entities.Settings;

namespace Sentyll.Domain.Data.Abstractions.Entities.Events;

[Table(TableConstants.EventParameters, Schema = SchemaConstants.Event)]
public class EventParameterEntity : IdentityEntity
{
    
    [Required]
    public Guid EventId { get; set; }

    [ForeignKey(nameof(EventId))]
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public virtual EventEntity Event { get; set; }

    [Required]
    public Guid ConfigurationId { get; set; }

    [ForeignKey(nameof(ConfigurationId))]
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public virtual ConfigurationEntity Configuration { get; set; }
    
}
