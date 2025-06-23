using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sentyll.Domain.Data.Abstractions.Entities.Scheduler;

namespace Sentyll.Domain.Data.Abstractions.EntityTypeConfigurations;

public class CronJobOccurrenceEntityConfiguration : IEntityTypeConfiguration<CronJobOccurrenceEntity>
{
    public void Configure(EntityTypeBuilder<CronJobOccurrenceEntity> builder)
    {
        builder.HasIndex(job => job.CronJobId, "IX_CronJobOccurrence_CronJobId");
        builder.HasIndex(job => job.ExecutionTime, "IX_CronJobOccurrence_ExecutionTime");
        builder.HasIndex(job => new
        {
            job.Status, 
            job.ExecutionTime
        }, "IX_CronJobOccurrence_Status_ExecutionTime");
    }
}