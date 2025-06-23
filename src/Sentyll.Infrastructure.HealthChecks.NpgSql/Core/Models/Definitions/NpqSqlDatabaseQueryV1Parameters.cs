using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;

namespace Sentyll.Infrastructure.HealthChecks.NpgSql.Core.Models.Definitions;

public sealed class NpqSqlDatabaseQueryV1Parameters : IValidatable
{
    
    [JsonPropertyName("connectionString")]
    public string ConnectionString { get; set; }
    
    [JsonPropertyName("query")]
    public string Query { get; set; }
    
    [JsonPropertyName("expectedJson")]
    public string? ExpectedJson { get; set; }

    public Result Validate() 
        => Result
            .FailureIf(string.IsNullOrWhiteSpace(ConnectionString), "connectionString is required")
            .Ensure(() => !string.IsNullOrWhiteSpace(Query), "query is required");
    
}