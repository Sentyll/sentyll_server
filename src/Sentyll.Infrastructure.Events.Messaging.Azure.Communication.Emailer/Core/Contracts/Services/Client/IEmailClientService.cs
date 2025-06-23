using CSharpFunctionalExtensions;
using Sentyll.Infrastructure.Events.Messaging.Azure.Communication.Emailer.Core.Models.Requests;

namespace Sentyll.Infrastructure.Events.Messaging.Azure.Communication.Emailer.Core.Contracts.Services.Client;

internal interface IEmailClientService
{
    Task<Result> SendAsync(SendEmailRequest request, CancellationToken cancellationToken = default);
}