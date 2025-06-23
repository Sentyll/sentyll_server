namespace Sentyll.Infrastructure.Events.Messaging.Azure.Communication.Emailer.Core.Models.Requests;

internal sealed record SendEmailRequest(
    string ConnectionString,
    string SenderAddress,
    string[] Recipients,
    string Heading,
    string Content
    );