using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;

namespace Sentyll.Infrastructure.HealthChecks.SqlServer.Core.Models.Definitions;

public class SqlServerDatabaseV1Parameters : IValidatable
{
    
    [JsonPropertyName("connectionString")]
    public string ConnectionString { get; set; }

    public Result Validate() 
        => Result
            .FailureIf(string.IsNullOrWhiteSpace(ConnectionString), "connectionString is required");
}