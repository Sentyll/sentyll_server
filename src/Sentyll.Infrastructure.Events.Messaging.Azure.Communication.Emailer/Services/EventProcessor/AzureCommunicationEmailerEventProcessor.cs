using CSharpFunctionalExtensions;
using Sentyll.Domain.Common.Abstractions.Enums;
using Sentyll.Domain.Data.Services.Abstractions.Contracts.Services.Events;
using Sentyll.Infrastructure.Events.Messaging.Abstractions.Contracts.Factories;
using Sentyll.Infrastructure.Events.Messaging.Abstractions.Enums;
using Sentyll.Infrastructure.Server.TaskPipeline.Abstraction.Contracts.Services.EventProcessors;
using Sentyll.Infrastructure.Server.TaskPipeline.Abstraction.Models.EventProcessors;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Sentyll.Infrastructure.Events.Messaging.Azure.Communication.Emailer.Core.Contracts.Services.Client;
using Sentyll.Infrastructure.Events.Messaging.Azure.Communication.Emailer.Core.Models.Definitions;
using Sentyll.Infrastructure.Events.Messaging.Azure.Communication.Emailer.Core.Models.Requests;

namespace Sentyll.Infrastructure.Events.Messaging.Azure.Communication.Emailer.Services.EventProcessor;

internal sealed class AzureCommunicationEmailerEventProcessor(
    ITemplateGeneratorFactory templateGeneratorFactory,
    IEmailClientService emailClientService,
    IEventsParameterDataService eventsParameterDataService
) : IMessagingEventProcessor
{
    
    public MessagingEventType Type => MessagingEventType.AZURE_COMMUNICATIONSERVICESES_EMAIL;
    
    public async Task<Result> ProcessAsync(HealthCheckEventRequest eventRequest, CancellationToken cancellationToken = default)
        => await templateGeneratorFactory
            .Resolve(TemplateGeneratorType.HTML)
            .Bind((templateGenerator) => eventRequest.JobResult.Status == HealthStatus.Healthy
                ? templateGenerator.GenerateUpAsync(new(eventRequest.HealthCheckProfile, eventRequest.JobResult, eventRequest.ExecutedOn), cancellationToken)
                : templateGenerator.GenerateDownAsync(new(eventRequest.HealthCheckProfile, eventRequest.JobResult, eventRequest.ExecutedOn), cancellationToken))
            .Bind(emailTemplate => RetrieveEventMailerRequestAsync(eventRequest.EventId, emailTemplate, cancellationToken))
            .Bind(emailRequest => emailClientService.SendAsync(emailRequest, cancellationToken))
            .ConfigureAwait(false);

    private async Task<Result<SendEmailRequest>> RetrieveEventMailerRequestAsync(
        Guid eventId,
        string emailContent,
        CancellationToken cancellationToken = default)
        => await eventsParameterDataService
            .GetParsedEventParametersAsync<AzureCommunicationEmailerV1Parameters>(eventId, cancellationToken)
            .Map(eventParams => new SendEmailRequest(
                ConnectionString: eventParams.ConnectionString,
                SenderAddress: eventParams.SenderAddress,
                Recipients: eventParams.RecipientAddresses,
                "Health Checks Alert", //TODO: this should be based on the UP / DOWN state of the health check.
                emailContent
                ));

}