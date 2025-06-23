using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;

namespace Sentyll.Infrastructure.HealthChecks.Redis.Core.Models.Definitions;

public sealed class RedisV1Parameters : IValidatable
{
    
    [JsonPropertyName("connectionString")]
    public string ConnectionString { get; set; }

    public Result Validate() 
        => Result.FailureIf(string.IsNullOrWhiteSpace(ConnectionString), "connectionString is required");
    
}