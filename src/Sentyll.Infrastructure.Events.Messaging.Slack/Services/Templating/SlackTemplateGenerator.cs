using CSharpFunctionalExtensions;
using Sentyll.Domain.Common.Abstractions.Extensions.CSharpFunctionalExtensions;
using Sentyll.Infrastructure.Events.Messaging.Abstractions.Contracts.Services.Templating;
using Sentyll.Infrastructure.Events.Messaging.Abstractions.models.Options;
using Sentyll.Infrastructure.Events.Messaging.Abstractions.models.Request;
using Microsoft.Extensions.Options;
using Sentyll.Infrastructure.Events.Messaging.Slack.Builders;

namespace Sentyll.Infrastructure.Events.Messaging.Slack.Services.Templating;

internal sealed class SlackTemplateGenerator(
    IOptions<ServerEndpointsOptions> serverAddressOptions
    ) : ITemplateGenerator
{
    
    public Task<Result<string>> GenerateUpAsync(GenerateTemplateRequest eventRequest, CancellationToken cancellationToken = default)
        => SlackRestContentBuilder
            .Init(serverAddressOptions.Value)
            .WithRestoredHeading(1)
            .WithDivider()
            .WithRestoredHealthCheck(eventRequest)
            .WithDivider()
            .Build()
            .AsMaybe()
            .ToResult("Slack template generated empty content")
            .ToTask();

    public Task<Result<string>> GenerateDownAsync(GenerateTemplateRequest eventRequest, CancellationToken cancellationToken = default)
        => SlackRestContentBuilder
            .Init(serverAddressOptions.Value)
            .WithFailureHeading(1)
            .WithDivider()
            .WithFailureHealthCheck(eventRequest)
            .WithDivider()
            .Build()
            .AsMaybe()
            .ToResult("Slack template generated empty content")
            .ToTask();
    
}