using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;

namespace Sentyll.Infrastructure.HealthChecks.Azure.Storage.Blobs.Core.Models.Definitions;

public sealed class AzureBlobStorageV1Parameters : IValidatable
{
    [JsonPropertyName("connectionString")]
    public string ConnectionString { get; set; }
    
    [JsonPropertyName("containerName")]
    public string? ContainerName { get; set; }

    public Result Validate() 
        => Result
            .FailureIf(string.IsNullOrWhiteSpace(ConnectionString), "connectionString is required");
}