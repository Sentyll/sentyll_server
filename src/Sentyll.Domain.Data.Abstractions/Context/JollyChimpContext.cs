using Sentyll.Domain.Data.Abstractions.Encryption;
using Sentyll.Domain.Data.Abstractions.Seeds;
using Sentyll.Domain.Data.Abstractions.Entities.Events;
using Sentyll.Domain.Data.Abstractions.Entities.HealthChecks;
using Sentyll.Domain.Data.Abstractions.Entities.Scheduler;
using Sentyll.Domain.Data.Abstractions.Entities.Settings;

namespace Sentyll.Domain.Data.Abstractions.Context;

public class SentyllContext : DbContext
{

    #region Scheduler

    public DbSet<CronJobEntity> CronJobs { get; set; }
    
    public DbSet<TimerJobEntity> TimerJobs { get; set; }
    
    public DbSet<CronJobOccurrenceEntity> CronJobOccurrences { get; set; }

    #endregion

    #region HealthCheck

    public DbSet<HealthCheckEntity> HealthChecks { get; set; }
    public DbSet<HealthCheckEventEntity> HealthCheckEvents { get; set; }
    public DbSet<HealthCheckExecutionEntity> HealthCheckExecutions { get; set; }
    public DbSet<HealthCheckExecutionHistoryEntity> HealthCheckExecutionHistories { get; set; }
    public DbSet<HealthCheckParameterEntity> HealthCheckParameters { get; set; }

    #endregion

    #region Event

    public DbSet<EventEntity> Events { get; set; }
    public DbSet<EventCategoryEntity> EventCategories { get; set; }
    public DbSet<EventExecutionEntity> EventExecutions { get; set; }
    public DbSet<EventParameterEntity> EventParameters { get; set; }

    #endregion

    #region Settings

    public DbSet<ConfigurationEntity> Configurations { get; set; }
    public DbSet<ServerSettingEntity> ServerSettings { get; set; }

    #endregion
    
    public SentyllContext(DbContextOptions<SentyllContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseEncryption();
        modelBuilder.SeedEntities();
        
        base.OnModelCreating(modelBuilder);
    }
}


