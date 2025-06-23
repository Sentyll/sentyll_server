using Sentyll.Domain.Data.Abstractions.Constants.PersistantStorage;
using Sentyll.Domain.Data.Abstractions.Contracts.Entities;
using Sentyll.Domain.Data.Abstractions.Encryption;
using Sentyll.Domain.Data.Abstractions.Entities.Base;

namespace Sentyll.Domain.Data.Abstractions.Entities.Settings;

[Table(TableConstants.Configurations, Schema = SchemaConstants.Settings)]
public class ConfigurationEntity : IdentityEntity, IParameterEntity
{
    [Required]
    [StringLength(512)]
    public string Key { get; set; }
    
    [Required]
    [EncryptProperty]
    public string Value { get; set; }
}
