using System.Net.Http.Json;
using CSharpFunctionalExtensions;
using Sentyll.Infrastructure.Events.WebHooks.Abstractions.Contracts.Services;
using Sentyll.Infrastructure.Events.WebHooks.Abstractions.Models.Requests;
using Sentyll.Infrastructure.Events.WebHooks.Abstractions.Models.Response;
using Sentyll.Infrastructure.Events.WebHooks.Core.Constants;

namespace Sentyll.Infrastructure.Events.WebHooks.Services;

internal sealed class WebhookHttpClientService(
    IHttpClientFactory httpClientFactory 
    ) : IWebhookHttpClientService
{

    public async Task<Result<WebhookSendResponse>> ExecuteRequestAsync(
        SendWebhookRequest request,
        CancellationToken cancellationToken = default)
        => await ExecuteRequestAsync(request, null, cancellationToken)
            .ConfigureAwait(false);
    
    public async Task<Result<WebhookSendResponse>> ExecuteRequestAsync(
        SendWebhookRequest request,
        Action<HttpClient>? configureClient = null,
        CancellationToken cancellationToken = default)
    {
        var method = request.HttpMethod;
        var timeout = request.Timeout;

        var httpClient = httpClientFactory.CreateClient(WebhookConstants.HttpClientName);
        configureClient?.Invoke(httpClient);

        using var requestMessage = new HttpRequestMessage(method, request.Uri);

        requestMessage.Version = httpClient.DefaultRequestVersion;
        requestMessage.VersionPolicy = httpClient.DefaultVersionPolicy;

        if (!string.IsNullOrWhiteSpace(request.Payload))
        {
            requestMessage.Content = new StringContent(request.Payload);
        }

        using var timeoutSource = new CancellationTokenSource(timeout);
        using var linkedSource = CancellationTokenSource.CreateLinkedTokenSource(timeoutSource.Token, cancellationToken);
        
        using var sendResponse = await httpClient
            .SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead, linkedSource.Token)
            .ConfigureAwait(false);
        
        var responseContent = await sendResponse.Content
            .ReadAsStringAsync(linkedSource.Token)
            .ConfigureAwait(false);

        var response = new WebhookSendResponse(
            IsSuccess: sendResponse.IsSuccessStatusCode,
            StatusCode: sendResponse.StatusCode,
            Content: responseContent
            );
        
        response.AppendResponseHeaders(sendResponse.Headers.Concat(sendResponse.Content.Headers));

        return response;
    }
}