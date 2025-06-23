using System.Text.Json.Serialization;
using Sentyll.Domain.Common.Abstractions.Extensions.CronTabSchedule;
using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;

namespace Sentyll.Domain.Common.Abstractions.Models.Definitions.HealthChecks.Payload;

public sealed class HealthCheckSchedulerPayloadDefinition : IValidatable
{

    /// <summary>
    /// Cron schedule that determines when the Health Check will be executed.
    /// </summary>
    [JsonPropertyName("schedule")]
    public string Schedule { get; set; }
    
    /// <summary>
    /// An optional <see cref="TimeSpan"/> representing the timeout of the check.
    /// </summary>
    [JsonPropertyName("timeout")]
    public int Timeout { get; set; }

    /// <summary>
    /// The <see cref="HealthStatus"/> that should be reported when the health check fails. Optional. If <c>null</c> then
    /// the default status of <see cref="HealthStatus.Unhealthy"/> will be reported.
    /// </summary>
    [JsonPropertyName("failureStatus")]
    public HealthStatus FailureStatus { get; set; }

    public Result Validate()
    {
        return Result
            .FailureIf(string.IsNullOrWhiteSpace(Schedule), "schedule is required")
            .Ensure(() => Schedule.IsValidCronTabSchedule(), "not a valid cron schedule")
            .Ensure(() => Timeout != null, "timeout is required")
            .Ensure(() => FailureStatus != null, "failureStatus is required");
    }
}