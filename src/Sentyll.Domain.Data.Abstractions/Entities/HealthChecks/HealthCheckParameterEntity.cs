using Sentyll.Domain.Data.Abstractions.Constants.PersistantStorage;
using Sentyll.Domain.Data.Abstractions.Entities.Base;
using Sentyll.Domain.Data.Abstractions.Entities.Settings;

namespace Sentyll.Domain.Data.Abstractions.Entities.HealthChecks;

[Table(TableConstants.HealthCheckParameters, Schema = SchemaConstants.HealthCheck)]
public class HealthCheckParameterEntity : IdentityEntity
{
    [Required]
    public Guid HealthCheckId { get; set; }

    [ForeignKey(nameof(HealthCheckId))]
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public virtual HealthCheckEntity HealthCheck { get; set; }

    [Required]
    public Guid ConfigurationId { get; set; }

    [ForeignKey(nameof(ConfigurationId))]
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public virtual ConfigurationEntity Configuration { get; set; }
    
}
