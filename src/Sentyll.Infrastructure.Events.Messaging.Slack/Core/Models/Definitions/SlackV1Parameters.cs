using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;

namespace Sentyll.Infrastructure.Events.Messaging.Slack.Core.Models.Definitions;

public sealed class SlackV1Parameters : IValidatable
{
    
    /// <summary>
    /// The Slack webhook uri.
    /// </summary>
    [JsonPropertyName("uri")]
    public Uri Uri { get; set; }

    public Result Validate()
        => Result.FailureIf(Uri == default, "uri is required");
    
}