using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;

namespace Sentyll.Infrastructure.Events.Messaging.Azure.Communication.Emailer.Core.Models.Definitions;

public sealed class AzureCommunicationEmailerV1Parameters : IValidatable
{
    
    /// <summary>
    /// The Slack webhook uri.
    /// </summary>
    [JsonPropertyName("connectionString")]
    public string ConnectionString { get; set; }

    [JsonPropertyName("senderAddress")]
    public string SenderAddress { get; set; }
    
    [JsonPropertyName("recipientAddresses")]
    public string[] RecipientAddresses { get; set; }
    
    public Result Validate()
        => Result
            .FailureIf(string.IsNullOrWhiteSpace(ConnectionString), "connectionString is required")
            .Ensure(() => !string.IsNullOrWhiteSpace(SenderAddress), "senderAddress is required")
            .Ensure(() => RecipientAddresses != null, "recipientAddresses is required");
    
}