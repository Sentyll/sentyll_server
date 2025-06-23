using Microsoft.Extensions.Diagnostics.HealthChecks;
using Sentyll.Infrastructure.HealthChecks.Uris.Core.Constants;
using Sentyll.Infrastructure.HealthChecks.Uris.Core.Models.Options;

namespace Sentyll.Infrastructure.HealthChecks.Uris.Services;

internal sealed class HttpService(IHttpClientFactory httpClientFactory)
{
    
    public async Task<HealthCheckResult> ExecuteHttpRequestAsync(
        UriOptions options,
        Action<HttpClient>? configureClient = null,
        CancellationToken cancellationToken = default)
    {
        var method = options.HttpMethod;
        var (min, max) = options.ExpectedHttpCodes;
        var timeout = options.Timeout;

        var httpClient = httpClientFactory.CreateClient(UriConstants.HttpClientName);
        configureClient?.Invoke(httpClient);

        using var requestMessage = new HttpRequestMessage(method, options.Uri);

        requestMessage.Version = httpClient.DefaultRequestVersion;
        requestMessage.VersionPolicy = httpClient.DefaultVersionPolicy;

        foreach (var (name, value) in options.Headers)
        {
            requestMessage.Headers.Add(name, value);
        }

        using (var timeoutSource = new CancellationTokenSource(timeout))
        using (var linkedSource = CancellationTokenSource.CreateLinkedTokenSource(timeoutSource.Token, cancellationToken))
        {
            using var response = await httpClient
                .SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead, linkedSource.Token)
                .ConfigureAwait(false);

            if (!((int)response.StatusCode >= min && (int)response.StatusCode <= max))
            {
                return new HealthCheckResult(HealthStatus.Unhealthy, UriConstants.InValidStatusCodeMessage(min, max, response.StatusCode));
            }

            if (options.ExpectedContent != null)
            {
                var responseBody = await response.Content
                    .ReadAsStringAsync(linkedSource.Token)
                    .ConfigureAwait(false);
                
                if (responseBody != options.ExpectedContent)
                {
                    return new HealthCheckResult(HealthStatus.Unhealthy, UriConstants.ExpectedContentMisMatchMessage(options.ExpectedContent));
                }
            }
        }

        return HealthCheckResult.Healthy();
    }
}