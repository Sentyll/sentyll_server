using Sentyll.Domain.Data.Abstractions.Constants;
using Sentyll.Domain.Data.Abstractions.Constants.PersistantStorage;
using Sentyll.Domain.Data.Abstractions.Entities.Base;

namespace Sentyll.Domain.Data.Abstractions.Entities.HealthChecks;

[Table(TableConstants.HealthChecks, Schema = SchemaConstants.HealthCheck)]
public class HealthCheckEntity : ActivatableEntity
{

    [Required]
    [StringLength(512)]
    public string Name { get; set; }
    
    [Required]
    [StringLength(1024)]
    public string? Description { get; set; }
    
    [Required]
    public HealthCheckType Type { get; set; }

    public string[]? Tags { get; set; }
    
    public virtual IList<HealthCheckParameterEntity> Parameters { get; set; }
    
    public virtual IList<HealthCheckExecutionEntity> Executions { get; set; }
    
    public virtual IList<HealthCheckExecutionHistoryEntity> ExecutionHistories { get; set; }

}
