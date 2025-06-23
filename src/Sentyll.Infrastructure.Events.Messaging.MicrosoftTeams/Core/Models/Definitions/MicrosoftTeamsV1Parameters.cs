using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;

namespace Sentyll.Infrastructure.Events.Messaging.MicrosoftTeams.Core.Models.Definitions;

public sealed class MicrosoftTeamsV1Parameters : IValidatable
{
    
    /// <summary>
    /// The Slack webhook uri.
    /// </summary>
    [JsonPropertyName("uri")]
    public Uri Uri { get; set; }

    public Result Validate()
        => Result.FailureIf(Uri == default, "uri is required");
    
}