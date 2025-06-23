using Sentyll.Domain.Data.Abstractions.Constants.PersistantStorage;
using Sentyll.Domain.Data.Abstractions.Entities.Base;

namespace Sentyll.Domain.Data.Abstractions.Entities.Events;

[Table(TableConstants.EventCategories, Schema = SchemaConstants.Event)]
public class EventCategoryEntity : IdentityEntity
{
    
    [Required]
    [StringLength(512)]
    public string Type { get; set; }
    
    [Required]
    [StringLength(1024)]
    public string TypeName { get; set; }
    
    [Required]
    public int TypeValue { get; set; }
    
}
