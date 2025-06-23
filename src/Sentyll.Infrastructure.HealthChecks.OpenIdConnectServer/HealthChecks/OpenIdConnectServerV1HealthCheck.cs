using System.Net.Http.Json;
using Sentyll.Domain.Common.Abstractions.Enums;
using Sentyll.Domain.Common.Abstractions.Models.Definitions.HealthChecks.Payload;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Jobs;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Sentyll.Infrastructure.HealthChecks.OpenIdConnectServer.Core.Constants;
using Sentyll.Infrastructure.HealthChecks.OpenIdConnectServer.Core.Models.Definitions;
using Sentyll.Infrastructure.HealthChecks.OpenIdConnectServer.Core.Models.Dto;

namespace Sentyll.Infrastructure.HealthChecks.OpenIdConnectServer.HealthChecks;

internal sealed class OpenIdConnectServerV1HealthCheck(
    IHealthCheckJobDependencyManager dependencyManager,
    IHttpClientFactory clientFactory
    ) : HealthCheckJob<OpenIdConnectServerV1HealthCheck, OpenIdConnectServerV1Parameters>(
        dependencyManager,
        HealthCheckType.OPENIDCONNECTSERVER_V1
    )
{
    
    public override async Task<HealthCheckResult> CheckAsync(
        HealthCheckPayloadDefinition<OpenIdConnectServerV1Parameters> jobContext, 
        CancellationToken cancellationToken)
    {
        try
        {
            var httpClient = clientFactory.CreateClient(OidcConstants.HttpClientName);

            httpClient.BaseAddress = jobContext.HealthCheck.IdSvrUri;
            
            using var response = await httpClient
                .GetAsync(jobContext.HealthCheck.DiscoveryConfigSegment, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                .ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content
                    .ReadAsStringAsync(cancellationToken)
                    .ConfigureAwait(false);
                
                return new HealthCheckResult(jobContext.Scheduler.FailureStatus, description: OidcConstants.RequestFailedMessage(response.StatusCode,content));
            }

            var discoveryResponse = await response.Content
                .ReadFromJsonAsync<DiscoveryEndpointResponse>(cancellationToken)
                .ConfigureAwait(false);

            ValidateResponse(discoveryResponse, jobContext.HealthCheck.IsDynamicOpenIdProvider);

            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return new HealthCheckResult(jobContext.Scheduler.FailureStatus, exception: ex);
        }
    }
    
    /// <summary>
    /// Validates Discovery response according to the <see href="https://openid.net/specs/openid-connect-discovery-1_0.html#ProviderMetadata">OpenID specification</see>
    /// </summary>
    private static void ValidateResponse(
        DiscoveryEndpointResponse? response,
        bool isDynamicOpenIdProvider = true)
    {
        ArgumentNullException.ThrowIfNull(response, OidcConstants.DeserializeFailedMessage);
        ArgumentException.ThrowIfNullOrWhiteSpace(response.Issuer, OidcConstants.GetMissingValueExceptionMessage(OidcConstants.Issuer));
        ArgumentException.ThrowIfNullOrWhiteSpace(response.AuthorizationEndpoint, OidcConstants.GetMissingValueExceptionMessage(OidcConstants.AuthorizationEndpoint));
        ArgumentException.ThrowIfNullOrWhiteSpace(response.JwksUri, OidcConstants.GetMissingValueExceptionMessage(OidcConstants.JwksUri));
        
        if (isDynamicOpenIdProvider)
        {
            ValidateRequiredValues(response.ResponseTypesSupported, OidcConstants.ResponseTypesSupported, OidcConstants.RequiredResponseTypes);

            // Specification describes 'token id_token' response type,
            // but some identity providers (f.e. IdentityServer and Entra ID) return 'id_token token'
            ValidateOneOfRequiredValues(response.ResponseTypesSupported, OidcConstants.ResponseTypesSupported, OidcConstants.RequiredCombinedResponseTypes);
        }

        ValidateOneOfRequiredValues(response.SubjectTypesSupported, OidcConstants.SubjectTypesSupported, OidcConstants.RequiredSubjectTypes);
        ValidateRequiredValues(response.SigningAlgorithmsSupported, OidcConstants.AlgorithmsSupported, OidcConstants.RequiredAlgorithms);
    }

    private static void ValidateRequiredValues(string[] values, string metadata, string[] requiredValues)
    {
        if (values == null || !requiredValues.All(v => values.Contains(v)))
        {
            throw new ArgumentException(OidcConstants.GetMissingRequiredAllValuesExceptionMessage(metadata, requiredValues));
        }
    }

    private static void ValidateOneOfRequiredValues(string[] values, string metadata, string[] requiredValues)
    {
        if (values == null || !requiredValues.Any(v => values.Contains(v)))
        {
            throw new ArgumentException(OidcConstants.GetMissingRequiredValuesExceptionMessage(metadata, requiredValues));
        }
    }
    
}