using Azure;
using Azure.Communication.Email;
using CSharpFunctionalExtensions;
using Sentyll.Domain.Common.Abstractions.Failures;
using Microsoft.Extensions.Logging;
using Sentyll.Infrastructure.Events.Messaging.Azure.Communication.Emailer.Core.Contracts.Services.Client;
using Sentyll.Infrastructure.Events.Messaging.Azure.Communication.Emailer.Core.Models.Requests;

namespace Sentyll.Infrastructure.Events.Messaging.Azure.Communication.Emailer.Services.Client;

internal sealed class EmailClientService(
    ILogger<EmailClientService> logger
    ) : IEmailClientService
{

    public async Task<Result> SendAsync(SendEmailRequest request, CancellationToken cancellationToken = default)
    {
        var recipients = request.Recipients
            .Select(recipient => new EmailAddress(recipient))
            .ToList();
        
        var emailClient = new EmailClient(request.ConnectionString);
        var emailMessage = new EmailMessage(
            senderAddress: request.SenderAddress,
            content: new EmailContent(request.Heading)
            {
                PlainText = request.Heading,
                Html = request.Content
            },
            recipients: new EmailRecipients(recipients));
        
        var sendResult = await emailClient.SendAsync(
                WaitUntil.Completed,
                emailMessage,
                cancellationToken)
            .ConfigureAwait(false);
        
        if (sendResult.Value.Status != EmailSendStatus.Succeeded)
        {
            logger.LogError("Invocation indicated a successful repose.");
            return Result.Failure(new Failure("ACSEN","0001", $"Send result indicated a unsuccessful response : {sendResult.Value}"));
        }

        return Result.Success();
    }
    
}