using CSharpFunctionalExtensions;
using Sentyll.Infrastructure.Events.WebHooks.Abstractions.Models.Requests;
using Sentyll.Infrastructure.Events.WebHooks.Abstractions.Models.Response;

namespace Sentyll.Infrastructure.Events.WebHooks.Abstractions.Contracts.Services;

public interface IWebhookHttpClientService
{
    Task<Result<WebhookSendResponse>> ExecuteRequestAsync(
        SendWebhookRequest request,
        CancellationToken cancellationToken = default
    );

    Task<Result<WebhookSendResponse>> ExecuteRequestAsync(
        SendWebhookRequest request,
        Action<HttpClient>? configureClient = null,
        CancellationToken cancellationToken = default
    );
}