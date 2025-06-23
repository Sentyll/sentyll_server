using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Sentyll.Domain.Data.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Settings");

            migrationBuilder.EnsureSchema(
                name: "Scheduler");

            migrationBuilder.EnsureSchema(
                name: "Event");

            migrationBuilder.EnsureSchema(
                name: "HealthCheck");

            migrationBuilder.CreateTable(
                name: "Configurations",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Key = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CronJobs",
                schema: "Scheduler",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Expression = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    Request = table.Column<byte[]>(type: "BLOB", nullable: true),
                    Retries = table.Column<int>(type: "INTEGER", nullable: false),
                    RetryIntervals = table.Column<string>(type: "TEXT", nullable: true),
                    Function = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    Description = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    InitIdentifier = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CronJobs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventCategories",
                schema: "Event",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false),
                    TypeName = table.Column<string>(type: "TEXT", maxLength: 1024, nullable: false),
                    TypeValue = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HealthChecks",
                schema: "HealthCheck",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1024, nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Tags = table.Column<string>(type: "TEXT", nullable: true),
                    IsEnabled = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthChecks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServerSettings",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Key = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimerJobs",
                schema: "Scheduler",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    LockHolder = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    Request = table.Column<byte[]>(type: "BLOB", nullable: true),
                    ExecutionTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LockedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ExecutedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Exception = table.Column<string>(type: "TEXT", nullable: true),
                    ElapsedTime = table.Column<long>(type: "INTEGER", nullable: false),
                    Retries = table.Column<int>(type: "INTEGER", nullable: false),
                    RetryCount = table.Column<int>(type: "INTEGER", nullable: false),
                    RetryIntervals = table.Column<string>(type: "TEXT", nullable: true),
                    Function = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    Description = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    InitIdentifier = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimerJobs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CronJobOccurrences",
                schema: "Scheduler",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    LockHolder = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    ExecutionTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LockedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ExecutedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Exception = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    ElapsedTime = table.Column<long>(type: "INTEGER", nullable: false),
                    RetryCount = table.Column<int>(type: "INTEGER", nullable: false),
                    CronJobId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CronJobOccurrences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CronJobOccurrences_CronJobs_CronJobId",
                        column: x => x.CronJobId,
                        principalSchema: "Scheduler",
                        principalTable: "CronJobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                schema: "Event",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1024, nullable: false),
                    Tags = table.Column<string>(type: "TEXT", nullable: true),
                    EventCategoryId = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsEnabled = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_EventCategories_EventCategoryId",
                        column: x => x.EventCategoryId,
                        principalSchema: "Event",
                        principalTable: "EventCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HealthCheckExecutions",
                schema: "HealthCheck",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 2058, nullable: true),
                    HealthStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    ExecutedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    HealthCheckId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthCheckExecutions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HealthCheckExecutions_HealthChecks_HealthCheckId",
                        column: x => x.HealthCheckId,
                        principalSchema: "HealthCheck",
                        principalTable: "HealthChecks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HealthCheckParameters",
                schema: "HealthCheck",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    HealthCheckId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ConfigurationId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthCheckParameters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HealthCheckParameters_Configurations_ConfigurationId",
                        column: x => x.ConfigurationId,
                        principalSchema: "Settings",
                        principalTable: "Configurations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HealthCheckParameters_HealthChecks_HealthCheckId",
                        column: x => x.HealthCheckId,
                        principalSchema: "HealthCheck",
                        principalTable: "HealthChecks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EventParameters",
                schema: "Event",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    EventId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ConfigurationId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventParameters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventParameters_Configurations_ConfigurationId",
                        column: x => x.ConfigurationId,
                        principalSchema: "Settings",
                        principalTable: "Configurations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EventParameters_Events_EventId",
                        column: x => x.EventId,
                        principalSchema: "Event",
                        principalTable: "Events",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HealthCheckEvents",
                schema: "HealthCheck",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    EventId = table.Column<Guid>(type: "TEXT", nullable: false),
                    HealthCheckId = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsEnabled = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthCheckEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HealthCheckEvents_Events_EventId",
                        column: x => x.EventId,
                        principalSchema: "Event",
                        principalTable: "Events",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HealthCheckEvents_HealthChecks_HealthCheckId",
                        column: x => x.HealthCheckId,
                        principalSchema: "HealthCheck",
                        principalTable: "HealthChecks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EventExecutions",
                schema: "Event",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    ExecutedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsSuccess = table.Column<bool>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 2058, nullable: true),
                    EventId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ExecutionId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventExecutions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventExecutions_Events_EventId",
                        column: x => x.EventId,
                        principalSchema: "Event",
                        principalTable: "Events",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EventExecutions_HealthCheckExecutions_ExecutionId",
                        column: x => x.ExecutionId,
                        principalSchema: "HealthCheck",
                        principalTable: "HealthCheckExecutions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HealthCheckExecutionHistories",
                schema: "HealthCheck",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ExecutionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    HealthCheckId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthCheckExecutionHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HealthCheckExecutionHistories_HealthCheckExecutions_ExecutionId",
                        column: x => x.ExecutionId,
                        principalSchema: "HealthCheck",
                        principalTable: "HealthCheckExecutions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HealthCheckExecutionHistories_HealthChecks_HealthCheckId",
                        column: x => x.HealthCheckId,
                        principalSchema: "HealthCheck",
                        principalTable: "HealthChecks",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                schema: "Event",
                table: "EventCategories",
                columns: new[] { "Id", "Type", "TypeName", "TypeValue" },
                values: new object[,]
                {
                    { new Guid("32efd3b5-b25a-4b00-aa6a-edb5e7c771e5"), "MessagingEventType", "SLACK", 1 },
                    { new Guid("9e7928eb-9d8a-4f7a-b6d6-320d5aa7e836"), "MessagingEventType", "MICROSOFT_TEAMS", 2 },
                    { new Guid("d158e6f6-e720-4c8f-b4d0-3ebab94665ca"), "Webhook", "Webhook", 0 },
                    { new Guid("eba0c273-b054-46c4-814f-32ea6677ad81"), "MessagingEventType", "AZURE_COMMUNICATIONSERVICESES_EMAIL", 0 }
                });

            migrationBuilder.InsertData(
                schema: "Settings",
                table: "ServerSettings",
                columns: new[] { "Id", "Key", "Value" },
                values: new object[,]
                {
                    { new Guid("1097f95d-376a-463b-b02f-86832e2c4516"), "SETTINGS:POLICIES:MAX_EXECUTION_HISTORIES_RETENTION_PERIOD_IN_DAYS", "365" },
                    { new Guid("12c60d84-596e-4e01-8900-c3aceb312a4b"), "SETTINGS:NOTIFICATIONS:MINIMUM_SECONDS_BETWEEN_FAILURE_NOTIFICATIONS", "30" },
                    { new Guid("2e1efd99-3c49-467d-bf51-f1d40e03cd45"), "SETTINGS:NOTIFICATIONS:NOTIFY_UNHEALTHY_ONE_TIME_UNTIL_CHANGE", "true" },
                    { new Guid("578c5f34-6c78-417f-a8d7-d502a7900a79"), "SETTINGS:POLICIES:EXECUTIONS_CAN_EXPIRE", "true" },
                    { new Guid("9756b55a-7102-4397-87ca-4e918723843b"), "SETTINGS:POLICIES:MAX_EXECUTION_RETENTION_PERIOD_IN_DAYS", "45" },
                    { new Guid("a7bdf726-e589-4527-9acd-b76d5940becf"), "SETTINGS:API:MAX_ACTIVE_REQUESTS", "50" },
                    { new Guid("bcf0e4c4-96e1-46f5-bd84-171b217926e9"), "SETTINGS:POLICIES:EXECUTION_HISTORIES_CAN_EXPIRE", "true" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CronJobOccurrence_CronJobId",
                schema: "Scheduler",
                table: "CronJobOccurrences",
                column: "CronJobId");

            migrationBuilder.CreateIndex(
                name: "IX_CronJobOccurrence_ExecutionTime",
                schema: "Scheduler",
                table: "CronJobOccurrences",
                column: "ExecutionTime");

            migrationBuilder.CreateIndex(
                name: "IX_CronJobOccurrence_Status_ExecutionTime",
                schema: "Scheduler",
                table: "CronJobOccurrences",
                columns: new[] { "Status", "ExecutionTime" });

            migrationBuilder.CreateIndex(
                name: "IX_CronJobs_Expression",
                schema: "Scheduler",
                table: "CronJobs",
                column: "Expression");

            migrationBuilder.CreateIndex(
                name: "IX_EventExecutions_EventId",
                schema: "Event",
                table: "EventExecutions",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventExecutions_ExecutionId",
                schema: "Event",
                table: "EventExecutions",
                column: "ExecutionId");

            migrationBuilder.CreateIndex(
                name: "IX_EventParameters_ConfigurationId",
                schema: "Event",
                table: "EventParameters",
                column: "ConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_EventParameters_EventId",
                schema: "Event",
                table: "EventParameters",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_EventCategoryId",
                schema: "Event",
                table: "Events",
                column: "EventCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthCheckEvents_EventId",
                schema: "HealthCheck",
                table: "HealthCheckEvents",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthCheckEvents_HealthCheckId",
                schema: "HealthCheck",
                table: "HealthCheckEvents",
                column: "HealthCheckId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthCheckExecutionHistories_ExecutionId",
                schema: "HealthCheck",
                table: "HealthCheckExecutionHistories",
                column: "ExecutionId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthCheckExecutionHistories_HealthCheckId",
                schema: "HealthCheck",
                table: "HealthCheckExecutionHistories",
                column: "HealthCheckId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthCheckExecutions_HealthCheckId",
                schema: "HealthCheck",
                table: "HealthCheckExecutions",
                column: "HealthCheckId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthCheckParameters_ConfigurationId",
                schema: "HealthCheck",
                table: "HealthCheckParameters",
                column: "ConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthCheckParameters_HealthCheckId",
                schema: "HealthCheck",
                table: "HealthCheckParameters",
                column: "HealthCheckId");

            migrationBuilder.CreateIndex(
                name: "IX_TimerJob_ExecutionTime",
                schema: "Scheduler",
                table: "TimerJobs",
                column: "ExecutionTime");

            migrationBuilder.CreateIndex(
                name: "IX_TimerJob_Status_ExecutionTime",
                schema: "Scheduler",
                table: "TimerJobs",
                columns: new[] { "Status", "ExecutionTime" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CronJobOccurrences",
                schema: "Scheduler");

            migrationBuilder.DropTable(
                name: "EventExecutions",
                schema: "Event");

            migrationBuilder.DropTable(
                name: "EventParameters",
                schema: "Event");

            migrationBuilder.DropTable(
                name: "HealthCheckEvents",
                schema: "HealthCheck");

            migrationBuilder.DropTable(
                name: "HealthCheckExecutionHistories",
                schema: "HealthCheck");

            migrationBuilder.DropTable(
                name: "HealthCheckParameters",
                schema: "HealthCheck");

            migrationBuilder.DropTable(
                name: "ServerSettings",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "TimerJobs",
                schema: "Scheduler");

            migrationBuilder.DropTable(
                name: "CronJobs",
                schema: "Scheduler");

            migrationBuilder.DropTable(
                name: "Events",
                schema: "Event");

            migrationBuilder.DropTable(
                name: "HealthCheckExecutions",
                schema: "HealthCheck");

            migrationBuilder.DropTable(
                name: "Configurations",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "EventCategories",
                schema: "Event");

            migrationBuilder.DropTable(
                name: "HealthChecks",
                schema: "HealthCheck");
        }
    }
}
