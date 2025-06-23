using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sentyll.Domain.Data.Abstractions.Entities.Scheduler;

namespace Sentyll.Domain.Data.Abstractions.EntityTypeConfigurations;

internal class CronJobEntityConfiguration : IEntityTypeConfiguration<CronJobEntity>
{
    public void Configure(EntityTypeBuilder<CronJobEntity> builder)
    {
        builder.HasIndex(job => job.Expression, "IX_CronJobs_Expression");
    }
}