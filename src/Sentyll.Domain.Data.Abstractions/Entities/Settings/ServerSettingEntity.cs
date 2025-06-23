using Sentyll.Domain.Data.Abstractions.Constants.PersistantStorage;
using Sentyll.Domain.Data.Abstractions.Contracts.Entities;
using Sentyll.Domain.Data.Abstractions.Entities.Base;

namespace Sentyll.Domain.Data.Abstractions.Entities.Settings;

[Table(TableConstants.ServerSettings, Schema = SchemaConstants.Settings)]
public class ServerSettingEntity : IdentityEntity, IParameterEntity
{
    [Required]
    [StringLength(512)]
    public string Key { get; set; }
    
    [Required]
    public string Value { get; set; }
}
