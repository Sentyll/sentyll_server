using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;

namespace Sentyll.Infrastructure.HealthChecks.Azure.ApplicationInsights.Core.Models.Definitions;

public sealed class AzureApplicationInsightsV1Parameters : IValidatable
{
    /// <summary>
    /// The azure app insights instrumentation key.
    /// </summary>
    [JsonPropertyName("instrumentationKey")]
    public string InstrumentationKey { get; set; }

    public Result Validate() 
        => Result.FailureIf(string.IsNullOrWhiteSpace(InstrumentationKey), "instrumentationKey is required");
}