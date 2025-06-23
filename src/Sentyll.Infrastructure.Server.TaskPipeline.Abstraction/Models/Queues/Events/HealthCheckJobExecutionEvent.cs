using Sentyll.Domain.Common.Abstractions.Contracts.Models.Results;
using Sentyll.Domain.Common.Abstractions.Enums;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Sentyll.Infrastructure.Server.TaskPipeline.Abstraction.Models.Queues.Events;

public readonly record struct HealthCheckJobExecutionEvent
{
    
    /// <summary>
    /// NOT SURE YET HOW SOMETHING LIKE THIS WOULD BE USEFUL?
    /// TODO: Delete if not used
    /// 
    /// </summary>
    public string EventType { get; init; }
    
    public DateTime QueuedOn { get; init; }

    public HealthCheckResult JobResult { get; init; }
    
    public IUnstructuredResult JobDefinition { get; init; }

    public HealthCheckJobExecutionEvent(
        HealthCheckType healthCheckType,
        HealthCheckResult jobResult,
        IUnstructuredResult jobDefinition
        )
    {
        EventType = $"HEALTH-CHECK::JOB::EXECUTION::{healthCheckType.ToString().ToUpper()}";
        QueuedOn = DateTime.UtcNow;
        JobResult = jobResult;
        JobDefinition = jobDefinition;
    }
    
}