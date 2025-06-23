using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;

namespace Sentyll.Infrastructure.HealthChecks.Azure.ServiceBus.Core.Models.Definitions;

public class AzureServiceBusTopicV1Parameters : BaseAzureServiceBusHealthCheckParameters, IValidatable
{
    /// <summary>
    /// The name of the topic to check.
    /// </summary>
    [JsonPropertyName("topicName")]
    public string TopicName { get; set; }

    public override Result Validate() 
        => base
            .Validate()
            .Ensure(() => !string.IsNullOrWhiteSpace(TopicName), "topicName is required");
}