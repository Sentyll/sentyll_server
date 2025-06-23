using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;

namespace Sentyll.Infrastructure.HealthChecks.Azure.Compute.FunctionApp.Core.Models.Definitions;

public sealed class AzureFunctionAppHttpEndpointV1Parameters : IValidatable
{
    
    [JsonPropertyName("uri")]
    public Uri Uri { get; set; }
    
    [JsonPropertyName("xFunctionKey")]
    public string XFunctionKey { get; set; }

    [JsonPropertyName("headers")] 
    public string[] Headers { get; set; } = [];

    public Result Validate() 
        => Result
            .FailureIf(Uri == default, "uri is required")
            .Ensure(() => !string.IsNullOrWhiteSpace(XFunctionKey), "xFunctionKey is required");
}