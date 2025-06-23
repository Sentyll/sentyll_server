using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Sentyll.Infrastructure.HealthChecks.Azure.ServiceBus.Core.Models.Definitions;

public sealed class AzureServiceBusQueueMessageCountThresholdV1Parameters : BaseAzureServiceBusHealthCheckParameters, IValidatable
{
    
    /// <summary>
    /// The name of the queue to check.
    /// </summary>
    [JsonPropertyName("queueName")]
    public string QueueName { get; set; }
    
    /// <summary>
    /// Number of active/dead letter Service Bus messages in the queue before message health check returned <see cref="HealthStatus.Unhealthy"/>.
    /// </summary>
    [JsonPropertyName("activeMessagesUnhealthyThreshold")]
    public int ActiveMessagesUnhealthyThreshold { get; set; }
    
    /// <summary>
    /// Number of active/dead letter Service Bus messages in the queue before message health check returned <see cref="HealthStatus.Degraded"/>.
    /// </summary>
    [JsonPropertyName("activeMessagesDegradedThreshold")]
    public int ActiveMessagesDegradedThreshold { get; set; }
    
    /// <summary>
    /// Number of active/dead letter Service Bus messages in the queue before message health check returned <see cref="HealthStatus.Unhealthy"/>.
    /// </summary>
    [JsonPropertyName("deadLetterMessagesUnhealthyThreshold")]
    public int DeadLetterMessagesUnhealthyThreshold { get; set; }
    
    /// <summary>
    /// Number of active/dead letter Service Bus messages in the queue before message health check returned <see cref="HealthStatus.Degraded"/>.
    /// </summary>
    [JsonPropertyName("deadLetterMessagesDegradedThreshold")]
    public int DeadLetterMessagesDegradedThreshold { get; set; }

    public override Result Validate() 
        => base
            .Validate()
            .Ensure(() => !string.IsNullOrWhiteSpace(QueueName), "queueName is required")
            .Ensure(() => ActiveMessagesUnhealthyThreshold != default, "activeMessagesUnhealthyThreshold is required")
            .Ensure(() => ActiveMessagesDegradedThreshold != default, "activeMessagesDegradedThreshold is required")
            .Ensure(() => DeadLetterMessagesUnhealthyThreshold != default, "deadLetterMessagesUnhealthyThreshold is required")
            .Ensure(() => DeadLetterMessagesDegradedThreshold != default, "deadLetterMessagesDegradedThreshold is required");
}