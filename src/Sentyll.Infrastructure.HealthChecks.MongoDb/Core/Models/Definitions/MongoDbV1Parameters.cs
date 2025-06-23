using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;

namespace Sentyll.Infrastructure.HealthChecks.MongoDb.Core.Models.Definitions;

public sealed class MongoDbV1Parameters : IValidatable
{
    [JsonPropertyName("connectionString")]
    public string ConnectionString { get; set; }
    
    [JsonPropertyName("databaseName")]
    public string? DatabaseName { get; set; }

    public Result Validate()
        => Result
            .FailureIf(string.IsNullOrWhiteSpace(ConnectionString), "connectionString is required");
    
}