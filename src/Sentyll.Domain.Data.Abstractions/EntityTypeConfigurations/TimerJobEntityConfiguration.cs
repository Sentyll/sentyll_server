using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sentyll.Domain.Data.Abstractions.Entities.Scheduler;

namespace Sentyll.Domain.Data.Abstractions.EntityTypeConfigurations;

internal class TimerJobEntityConfiguration : IEntityTypeConfiguration<TimerJobEntity>
{
    public void Configure(EntityTypeBuilder<TimerJobEntity> builder)
    {
        builder.HasIndex(job => job.ExecutionTime, "IX_TimerJob_ExecutionTime");
        builder.HasIndex(job => new
        {
            job.Status, 
            job.ExecutionTime
        }, "IX_TimerJob_Status_ExecutionTime");
    }
}