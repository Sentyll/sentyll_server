using CSharpFunctionalExtensions;
using Sentyll.Domain.Common.Abstractions.Enums;
using Sentyll.Domain.Data.Services.Abstractions.Contracts.Services.Events;
using Sentyll.Infrastructure.Events.Messaging.Abstractions.Contracts.Factories;
using Sentyll.Infrastructure.Events.Messaging.Abstractions.Enums;
using Sentyll.Infrastructure.Events.WebHooks.Abstractions.Contracts.Services;
using Sentyll.Infrastructure.Server.TaskPipeline.Abstraction.Contracts.Services.EventProcessors;
using Sentyll.Infrastructure.Server.TaskPipeline.Abstraction.Models.EventProcessors;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Sentyll.Infrastructure.Events.Messaging.Slack.Core.Models.Definitions;

namespace Sentyll.Infrastructure.Events.Messaging.Slack.Services.EventProcessors;

internal sealed class SlackEventProcessor(
    IWebhookHttpClientService webhookHttpClientService,
    ITemplateGeneratorFactory templateGeneratorFactory,
    IEventsParameterDataService eventsParameterDataService
    ) : IMessagingEventProcessor
{

    public MessagingEventType Type => MessagingEventType.SLACK;
    
    public async Task<Result> ProcessAsync(HealthCheckEventRequest eventRequest, CancellationToken cancellationToken = default)
        => await templateGeneratorFactory
            .Resolve(TemplateGeneratorType.JSON_SLACK)
            .Bind((templateGenerator) => eventRequest.JobResult.Status == HealthStatus.Healthy
                ? templateGenerator.GenerateUpAsync(new(eventRequest.HealthCheckProfile, eventRequest.JobResult, eventRequest.ExecutedOn), cancellationToken)
                : templateGenerator.GenerateDownAsync(new(eventRequest.HealthCheckProfile, eventRequest.JobResult, eventRequest.ExecutedOn), cancellationToken))
            .BindZip(_ => eventsParameterDataService.GetParsedEventParametersAsync<SlackV1Parameters>(eventRequest.EventId, cancellationToken))
            .Bind((zipResult) => webhookHttpClientService.ExecuteRequestAsync(new(HttpMethod.Post, zipResult.Second.Uri, zipResult.First), cancellationToken))
            .ConfigureAwait(false);

}