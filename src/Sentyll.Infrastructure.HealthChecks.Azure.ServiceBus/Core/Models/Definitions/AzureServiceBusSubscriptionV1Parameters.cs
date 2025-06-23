using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;

namespace Sentyll.Infrastructure.HealthChecks.Azure.ServiceBus.Core.Models.Definitions;

public sealed class AzureServiceBusSubscriptionV1Parameters : AzureServiceBusTopicV1Parameters, IValidatable
{
    /// <summary>
    /// The subscription name of the topic subscription to check.
    /// </summary>
    [JsonPropertyName("subscriptionName")]
    public string SubscriptionName { get; set; }

    /// <summary>
    /// Will use <c>PeekMessageAsync</c> method to determine status if set to <see langword="true"/> (default),
    /// otherwise; will use <c>GetProperties*</c> method.
    /// </summary>
    /// <remarks>
    /// Peek requires Listen claim to work. However, if only Sender claim using the Azure built-in roles (RBAC)
    /// <see href="https://learn.microsoft.com/azure/role-based-access-control/built-in-roles#azure-service-bus-data-sender">Azure Service Bus Data Sender</see>
    /// is used set this to <see langword="false"/>. By default <see langword="true"/>.
    /// </remarks>
    [JsonPropertyName("usePeekMode")]
    public bool UsePeekMode { get; set; } = true;

    public override Result Validate() 
        => base
            .Validate()
            .Ensure(() => !string.IsNullOrWhiteSpace(SubscriptionName), "subscriptionName is required")
            .Ensure(() => UsePeekMode != default, "usePeekMode is required");
}