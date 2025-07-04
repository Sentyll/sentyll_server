﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sentyll.Domain.Data.Abstractions.Context;

#nullable disable

namespace Sentyll.Domain.Data.Sqlite.Migrations
{
    [DbContext(typeof(SentyllContext))]
    partial class SentyllContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.14");

            modelBuilder.Entity("Sentyll.Domain.Data.Abstractions.Entities.Events.EventCategoryEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("TEXT");

                    b.Property<string>("TypeName")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("TEXT");

                    b.Property<int>("TypeValue")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("EventCategories", "Event");

                    b.HasData(
                        new
                        {
                            Id = new Guid("d158e6f6-e720-4c8f-b4d0-3ebab94665ca"),
                            Type = "Webhook",
                            TypeName = "Webhook",
                            TypeValue = 0
                        },
                        new
                        {
                            Id = new Guid("eba0c273-b054-46c4-814f-32ea6677ad81"),
                            Type = "MessagingEventType",
                            TypeName = "AZURE_COMMUNICATIONSERVICESES_EMAIL",
                            TypeValue = 0
                        },
                        new
                        {
                            Id = new Guid("9e7928eb-9d8a-4f7a-b6d6-320d5aa7e836"),
                            Type = "MessagingEventType",
                            TypeName = "MICROSOFT_TEAMS",
                            TypeValue = 2
                        },
                        new
                        {
                            Id = new Guid("32efd3b5-b25a-4b00-aa6a-edb5e7c771e5"),
                            Type = "MessagingEventType",
                            TypeName = "SLACK",
                            TypeValue = 1
                        });
                });

            modelBuilder.Entity("Sentyll.Domain.Data.Abstractions.Entities.Events.EventEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("TEXT");

                    b.Property<Guid>("EventCategoryId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("TEXT");

                    b.Property<string>("Tags")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("EventCategoryId");

                    b.ToTable("Events", "Event");
                });

            modelBuilder.Entity("Sentyll.Domain.Data.Abstractions.Entities.Events.EventExecutionEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasMaxLength(2058)
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("EventId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ExecutedOn")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ExecutionId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsSuccess")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("ExecutionId");

                    b.ToTable("EventExecutions", "Event");
                });

            modelBuilder.Entity("Sentyll.Domain.Data.Abstractions.Entities.Events.EventParameterEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ConfigurationId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("EventId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ConfigurationId");

                    b.HasIndex("EventId");

                    b.ToTable("EventParameters", "Event");
                });

            modelBuilder.Entity("Sentyll.Domain.Data.Abstractions.Entities.HealthChecks.HealthCheckEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("TEXT");

                    b.Property<string>("Tags")
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("HealthChecks", "HealthCheck");
                });

            modelBuilder.Entity("Sentyll.Domain.Data.Abstractions.Entities.HealthChecks.HealthCheckEventEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("EventId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("HealthCheckId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("HealthCheckId");

                    b.ToTable("HealthCheckEvents", "HealthCheck");
                });

            modelBuilder.Entity("Sentyll.Domain.Data.Abstractions.Entities.HealthChecks.HealthCheckExecutionEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasMaxLength(2058)
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ExecutedOn")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("HealthCheckId")
                        .HasColumnType("TEXT");

                    b.Property<int>("HealthStatus")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("HealthCheckId");

                    b.ToTable("HealthCheckExecutions", "HealthCheck");
                });

            modelBuilder.Entity("Sentyll.Domain.Data.Abstractions.Entities.HealthChecks.HealthCheckExecutionHistoryEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ExecutionId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("HealthCheckId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ExecutionId");

                    b.HasIndex("HealthCheckId");

                    b.ToTable("HealthCheckExecutionHistories", "HealthCheck");
                });

            modelBuilder.Entity("Sentyll.Domain.Data.Abstractions.Entities.HealthChecks.HealthCheckParameterEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ConfigurationId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("HealthCheckId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ConfigurationId");

                    b.HasIndex("HealthCheckId");

                    b.ToTable("HealthCheckParameters", "HealthCheck");
                });

            modelBuilder.Entity("Sentyll.Domain.Data.Abstractions.Entities.Scheduler.CronJobEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasMaxLength(512)
                        .HasColumnType("TEXT");

                    b.Property<string>("Expression")
                        .HasMaxLength(512)
                        .HasColumnType("TEXT");

                    b.Property<string>("Function")
                        .HasMaxLength(512)
                        .HasColumnType("TEXT");

                    b.Property<string>("InitIdentifier")
                        .HasMaxLength(512)
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Request")
                        .HasColumnType("BLOB");

                    b.Property<int>("Retries")
                        .HasColumnType("INTEGER");

                    b.Property<string>("RetryIntervals")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Expression" }, "IX_CronJobs_Expression");

                    b.ToTable("CronJobs", "Scheduler");
                });

            modelBuilder.Entity("Sentyll.Domain.Data.Abstractions.Entities.Scheduler.CronJobOccurrenceEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CronJobId")
                        .HasColumnType("TEXT");

                    b.Property<long>("ElapsedTime")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Exception")
                        .HasMaxLength(512)
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ExecutedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ExecutionTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("LockHolder")
                        .HasMaxLength(512)
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("LockedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("RetryCount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "CronJobId" }, "IX_CronJobOccurrence_CronJobId");

                    b.HasIndex(new[] { "ExecutionTime" }, "IX_CronJobOccurrence_ExecutionTime");

                    b.HasIndex(new[] { "Status", "ExecutionTime" }, "IX_CronJobOccurrence_Status_ExecutionTime");

                    b.ToTable("CronJobOccurrences", "Scheduler");
                });

            modelBuilder.Entity("Sentyll.Domain.Data.Abstractions.Entities.Scheduler.TimerJobEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasMaxLength(512)
                        .HasColumnType("TEXT");

                    b.Property<long>("ElapsedTime")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Exception")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ExecutedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ExecutionTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Function")
                        .HasMaxLength(512)
                        .HasColumnType("TEXT");

                    b.Property<string>("InitIdentifier")
                        .HasMaxLength(512)
                        .HasColumnType("TEXT");

                    b.Property<string>("LockHolder")
                        .HasMaxLength(512)
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("LockedAt")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Request")
                        .HasColumnType("BLOB");

                    b.Property<int>("Retries")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RetryCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("RetryIntervals")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "ExecutionTime" }, "IX_TimerJob_ExecutionTime");

                    b.HasIndex(new[] { "Status", "ExecutionTime" }, "IX_TimerJob_Status_ExecutionTime");

                    b.ToTable("TimerJobs", "Scheduler");
                });

            modelBuilder.Entity("Sentyll.Domain.Data.Abstractions.Entities.Settings.ConfigurationEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Configurations", "Settings");
                });

            modelBuilder.Entity("Sentyll.Domain.Data.Abstractions.Entities.Settings.ServerSettingEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ServerSettings", "Settings");

                    b.HasData(
                        new
                        {
                            Id = new Guid("a7bdf726-e589-4527-9acd-b76d5940becf"),
                            Key = "SETTINGS:API:MAX_ACTIVE_REQUESTS",
                            Value = "50"
                        },
                        new
                        {
                            Id = new Guid("12c60d84-596e-4e01-8900-c3aceb312a4b"),
                            Key = "SETTINGS:NOTIFICATIONS:MINIMUM_SECONDS_BETWEEN_FAILURE_NOTIFICATIONS",
                            Value = "30"
                        },
                        new
                        {
                            Id = new Guid("2e1efd99-3c49-467d-bf51-f1d40e03cd45"),
                            Key = "SETTINGS:NOTIFICATIONS:NOTIFY_UNHEALTHY_ONE_TIME_UNTIL_CHANGE",
                            Value = "true"
                        },
                        new
                        {
                            Id = new Guid("578c5f34-6c78-417f-a8d7-d502a7900a79"),
                            Key = "SETTINGS:POLICIES:EXECUTIONS_CAN_EXPIRE",
                            Value = "true"
                        },
                        new
                        {
                            Id = new Guid("9756b55a-7102-4397-87ca-4e918723843b"),
                            Key = "SETTINGS:POLICIES:MAX_EXECUTION_RETENTION_PERIOD_IN_DAYS",
                            Value = "45"
                        },
                        new
                        {
                            Id = new Guid("bcf0e4c4-96e1-46f5-bd84-171b217926e9"),
                            Key = "SETTINGS:POLICIES:EXECUTION_HISTORIES_CAN_EXPIRE",
                            Value = "true"
                        },
                        new
                        {
                            Id = new Guid("1097f95d-376a-463b-b02f-86832e2c4516"),
                            Key = "SETTINGS:POLICIES:MAX_EXECUTION_HISTORIES_RETENTION_PERIOD_IN_DAYS",
                            Value = "365"
                        });
                });

            modelBuilder.Entity("Sentyll.Domain.Data.Abstractions.Entities.Events.EventEntity", b =>
                {
                    b.HasOne("Sentyll.Domain.Data.Abstractions.Entities.Events.EventCategoryEntity", "EventCategory")
                        .WithMany()
                        .HasForeignKey("EventCategoryId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("EventCategory");
                });

            modelBuilder.Entity("Sentyll.Domain.Data.Abstractions.Entities.Events.EventExecutionEntity", b =>
                {
                    b.HasOne("Sentyll.Domain.Data.Abstractions.Entities.Events.EventEntity", "Event")
                        .WithMany("Executions")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Sentyll.Domain.Data.Abstractions.Entities.HealthChecks.HealthCheckExecutionEntity", "Execution")
                        .WithMany()
                        .HasForeignKey("ExecutionId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("Execution");
                });

            modelBuilder.Entity("Sentyll.Domain.Data.Abstractions.Entities.Events.EventParameterEntity", b =>
                {
                    b.HasOne("Sentyll.Domain.Data.Abstractions.Entities.Settings.ConfigurationEntity", "Configuration")
                        .WithMany()
                        .HasForeignKey("ConfigurationId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Sentyll.Domain.Data.Abstractions.Entities.Events.EventEntity", "Event")
                        .WithMany("Parameters")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Configuration");

                    b.Navigation("Event");
                });

            modelBuilder.Entity("Sentyll.Domain.Data.Abstractions.Entities.HealthChecks.HealthCheckEventEntity", b =>
                {
                    b.HasOne("Sentyll.Domain.Data.Abstractions.Entities.Events.EventEntity", "Event")
                        .WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Sentyll.Domain.Data.Abstractions.Entities.HealthChecks.HealthCheckEntity", "HealthCheck")
                        .WithMany()
                        .HasForeignKey("HealthCheckId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("HealthCheck");
                });

            modelBuilder.Entity("Sentyll.Domain.Data.Abstractions.Entities.HealthChecks.HealthCheckExecutionEntity", b =>
                {
                    b.HasOne("Sentyll.Domain.Data.Abstractions.Entities.HealthChecks.HealthCheckEntity", "HealthCheck")
                        .WithMany("Executions")
                        .HasForeignKey("HealthCheckId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("HealthCheck");
                });

            modelBuilder.Entity("Sentyll.Domain.Data.Abstractions.Entities.HealthChecks.HealthCheckExecutionHistoryEntity", b =>
                {
                    b.HasOne("Sentyll.Domain.Data.Abstractions.Entities.HealthChecks.HealthCheckExecutionEntity", "Execution")
                        .WithMany()
                        .HasForeignKey("ExecutionId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Sentyll.Domain.Data.Abstractions.Entities.HealthChecks.HealthCheckEntity", "HealthCheck")
                        .WithMany("ExecutionHistories")
                        .HasForeignKey("HealthCheckId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Execution");

                    b.Navigation("HealthCheck");
                });

            modelBuilder.Entity("Sentyll.Domain.Data.Abstractions.Entities.HealthChecks.HealthCheckParameterEntity", b =>
                {
                    b.HasOne("Sentyll.Domain.Data.Abstractions.Entities.Settings.ConfigurationEntity", "Configuration")
                        .WithMany()
                        .HasForeignKey("ConfigurationId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Sentyll.Domain.Data.Abstractions.Entities.HealthChecks.HealthCheckEntity", "HealthCheck")
                        .WithMany("Parameters")
                        .HasForeignKey("HealthCheckId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Configuration");

                    b.Navigation("HealthCheck");
                });

            modelBuilder.Entity("Sentyll.Domain.Data.Abstractions.Entities.Scheduler.CronJobOccurrenceEntity", b =>
                {
                    b.HasOne("Sentyll.Domain.Data.Abstractions.Entities.Scheduler.CronJobEntity", "CronJob")
                        .WithMany()
                        .HasForeignKey("CronJobId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CronJob");
                });

            modelBuilder.Entity("Sentyll.Domain.Data.Abstractions.Entities.Events.EventEntity", b =>
                {
                    b.Navigation("Executions");

                    b.Navigation("Parameters");
                });

            modelBuilder.Entity("Sentyll.Domain.Data.Abstractions.Entities.HealthChecks.HealthCheckEntity", b =>
                {
                    b.Navigation("ExecutionHistories");

                    b.Navigation("Executions");

                    b.Navigation("Parameters");
                });
#pragma warning restore 612, 618
        }
    }
}
