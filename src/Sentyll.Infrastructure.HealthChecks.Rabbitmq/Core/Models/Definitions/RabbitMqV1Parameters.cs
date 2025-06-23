using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;

namespace Sentyll.Infrastructure.HealthChecks.Rabbitmq.Core.Models.Definitions;

public sealed class RabbitMqV1Parameters : IValidatable
{
    
    /// <summary>
    /// An Uri representing a connection string for RabbitMQ.
    /// </summary>
    [JsonPropertyName("connectionUri")]
    public Uri ConnectionUri { get; set; }
    
    /// <summary>
    /// Timeout setting for connection attempts.
    /// </summary>
    [JsonPropertyName("connectionTimeout")]
    public TimeSpan? ConnectionTimeout { get; set; }
    
    public Result Validate() 
        => Result
            .FailureIf(ConnectionUri == default, "connectionUri is required");
    
}