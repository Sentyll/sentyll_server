using CSharpFunctionalExtensions;
using Sentyll.Domain.Common.Abstractions.Extensions.CSharpFunctionalExtensions;
using Sentyll.Infrastructure.Events.Messaging.Abstractions.Contracts.Services.Templating;
using Sentyll.Infrastructure.Events.Messaging.Abstractions.models.Options;
using Sentyll.Infrastructure.Events.Messaging.Abstractions.models.Request;
using Microsoft.Extensions.Options;
using Sentyll.Infrastructure.Events.Messaging.MicrosoftTeams.Builders;

namespace Sentyll.Infrastructure.Events.Messaging.MicrosoftTeams.Services.Templating;

internal sealed class MicrosoftTeamsTemplateGenerator(
    IOptions<ServerEndpointsOptions> serverAddressOptions
    ) : ITemplateGenerator
{
    
    public Task<Result<string>> GenerateUpAsync(GenerateTemplateRequest eventRequest, CancellationToken cancellationToken = default)
        => MicrosoftTeamsRestContentBuilder
            .Init(serverAddressOptions.Value)
            .WithRestoredHeading(1)
            .WithDivider()
            .WithRestoredHealthCheck(eventRequest)
            .WithDivider()
            .Build()
            .AsMaybe()
            .ToResult("Microsoft Teams template generated empty content")
            .ToTask();

    public Task<Result<string>> GenerateDownAsync(GenerateTemplateRequest eventRequest, CancellationToken cancellationToken = default)
        => MicrosoftTeamsRestContentBuilder
            .Init(serverAddressOptions.Value)
            .WithFailureHeading(1)
            .WithDivider()
            .WithFailureHealthCheck(eventRequest)
            .WithDivider()
            .Build()
            .AsMaybe()
            .ToResult("Microsoft Teams template generated empty content")
            .ToTask();
}