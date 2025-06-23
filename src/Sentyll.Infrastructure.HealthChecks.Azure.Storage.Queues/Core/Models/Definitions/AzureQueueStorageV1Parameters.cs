using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;

namespace Sentyll.Infrastructure.HealthChecks.Azure.Storage.Queues.Core.Models.Definitions;

public sealed class AzureQueueStorageV1Parameters : IValidatable
{
    [JsonPropertyName("connectionString")]
    public string ConnectionString { get; set; }
    
    [JsonPropertyName("queueName")]
    public string? QueueName { get; set; }

    public Result Validate() 
        => Result
            .FailureIf(string.IsNullOrWhiteSpace(ConnectionString), "connectionString is required");
}