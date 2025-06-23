using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;

namespace Sentyll.Infrastructure.HealthChecks.Azure.SignalR.Core.Models.Definitions;

public class AzureSignalrV1Parameter : IValidatable
{
    
    [JsonPropertyName("uri")]
    public Uri Uri { get; set; }

    public Result Validate() 
        => Result
            .FailureIf(Uri == default, "uri is required");
    
}