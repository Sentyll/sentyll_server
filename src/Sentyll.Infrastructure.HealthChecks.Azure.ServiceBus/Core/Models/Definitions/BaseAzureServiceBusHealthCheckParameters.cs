using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;

namespace Sentyll.Infrastructure.HealthChecks.Azure.ServiceBus.Core.Models.Definitions;

public abstract class BaseAzureServiceBusHealthCheckParameters : IValidatable
{
    /// <summary>
    /// The azure service bus fully qualified namespace.
    /// </summary>
    [JsonPropertyName("fullyQualifiedNamespace")]
    public string FullyQualifiedNamespace { get; set; }

    [JsonPropertyName("tenantId")]
    public string TenantId { get; set; }
    
    [JsonPropertyName("clientId")]
    public string ClientId { get; set; }
    
    [JsonPropertyName("clientSecret")]
    public string ClientSecret { get; set; }

    public virtual Result Validate() 
        => Result
            .FailureIf(string.IsNullOrWhiteSpace(FullyQualifiedNamespace), "fullyQualifiedNamespace is required")
            .Ensure(() => !string.IsNullOrWhiteSpace(TenantId), "tenantId is required")
            .Ensure(() => !string.IsNullOrWhiteSpace(ClientId), "clientId is required")
            .Ensure(() => !string.IsNullOrWhiteSpace(ClientSecret), "clientSecret is required");
    
}
