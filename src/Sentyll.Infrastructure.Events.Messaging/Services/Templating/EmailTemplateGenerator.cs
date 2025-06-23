using CSharpFunctionalExtensions;
using Sentyll.Domain.Common.Abstractions.Extensions.CSharpFunctionalExtensions;
using Sentyll.Infrastructure.Events.Messaging.Abstractions.Builders.Content;
using Sentyll.Infrastructure.Events.Messaging.Abstractions.Contracts.Services.Templating;
using Sentyll.Infrastructure.Events.Messaging.Abstractions.models.Options;
using Sentyll.Infrastructure.Events.Messaging.Abstractions.models.Request;
using Microsoft.Extensions.Options;

namespace Sentyll.Infrastructure.Events.Messaging.Services.Templating;

internal sealed class EmailTemplateGenerator(
    IOptions<ServerEndpointsOptions> serverAddressOptions
) : ITemplateGenerator
{
    
    public Task<Result<string>> GenerateUpAsync(GenerateTemplateRequest eventRequest, CancellationToken cancellationToken = default)
        => EmailContentBuilder
            .Init(serverAddressOptions.Value)
            .WithFailureHealthCheck(eventRequest)
            .Build()
            .AsMaybe()
            .ToResult("Microsoft Teams template generated empty content")
            .ToTask();

    public Task<Result<string>> GenerateDownAsync(GenerateTemplateRequest eventRequest, CancellationToken cancellationToken = default)
        => EmailContentBuilder
            .Init(serverAddressOptions.Value)
            .WithFailureHealthCheck(eventRequest)
            .Build()
            .AsMaybe()
            .ToResult("Microsoft Teams template generated empty content")
            .ToTask();
}