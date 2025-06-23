using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;

namespace Sentyll.Infrastructure.HealthChecks.Azure.Storage.Files.Shares.Core.Models.Definitions;

public class AzureFileShareV1Parameters : IValidatable
{
    [JsonPropertyName("connectionString")]
    public string ConnectionString { get; set; }
    
    [JsonPropertyName("shareName")]
    public string? ShareName { get; set; }

    public Result Validate() 
        => Result
            .FailureIf(string.IsNullOrWhiteSpace(ConnectionString), "connectionString is required");
}