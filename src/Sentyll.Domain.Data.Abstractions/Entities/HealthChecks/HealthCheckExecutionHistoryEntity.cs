using Sentyll.Domain.Data.Abstractions.Constants.PersistantStorage;
using Sentyll.Domain.Data.Abstractions.Entities.Base;

namespace Sentyll.Domain.Data.Abstractions.Entities.HealthChecks;

[Table(TableConstants.HealthCheckExecutionHistories, Schema = SchemaConstants.HealthCheck)]
public class HealthCheckExecutionHistoryEntity  : IdentityEntity
{
    [Required]
    public Guid ExecutionId { get; set; }
    
    [ForeignKey(nameof(ExecutionId))]
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public virtual HealthCheckExecutionEntity Execution { get; set; }
    
    [Required]
    public Guid HealthCheckId { get; set; }

    [ForeignKey(nameof(HealthCheckId))]
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public virtual HealthCheckEntity HealthCheck { get; set; }
    
}