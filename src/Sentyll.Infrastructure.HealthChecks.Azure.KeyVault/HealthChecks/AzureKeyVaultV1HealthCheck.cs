using Azure.Identity;
using Azure.Security.KeyVault.Certificates;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Secrets;
using Sentyll.Domain.Common.Abstractions.Enums;
using Sentyll.Domain.Common.Abstractions.Models.Definitions.HealthChecks.Payload;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Constants.Storage.Cache;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Jobs;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Services;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Storage.Cache;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Sentyll.Infrastructure.HealthChecks.Azure.KeyVault.Core.Constants;
using Sentyll.Infrastructure.HealthChecks.Azure.KeyVault.Core.Models.Definitions;

namespace Sentyll.Infrastructure.HealthChecks.Azure.KeyVault.HealthChecks;

internal sealed class AzureKeyVaultV1HealthCheck(
    IHealthCheckJobDependencyManager dependencyManager
    ) : HealthCheckJob<AzureKeyVaultV1HealthCheck, AzureKeyVaultV1Parameters>(
        dependencyManager,
        HealthCheckType.AZURE_KEYVAULT_V1
    )
{
    
    public override async Task<HealthCheckResult> CheckAsync(
        HealthCheckPayloadDefinition<AzureKeyVaultV1Parameters> jobContext, 
        CancellationToken cancellationToken)
    {
        try
        {
            if (
                jobContext.HealthCheck.Keys.Length == 0
                && jobContext.HealthCheck.Certificates.Length == 0
                && jobContext.HealthCheck.Secrets.Length == 0
            )
            {
                return HealthCheckResult.Unhealthy(KvConstants.NothingToCheckFailureMessage);
            }

            foreach (string secret in jobContext.HealthCheck.Secrets)
            {
                await CreateSecretClient(jobContext.HealthCheck.Uri)
                    .GetSecretAsync(secret, cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
            }

            foreach (string key in jobContext.HealthCheck.Keys)
            {
                await CreateKeyClient(jobContext.HealthCheck.Uri)
                    .GetKeyAsync(key, cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
            }

            foreach (var certificateDetails in jobContext.HealthCheck.Certificates)
            {
                var certificate = await CreateCertificateClient(jobContext.HealthCheck.Uri)
                    .GetCertificateAsync(certificateDetails.CertificateName, cancellationToken: cancellationToken)
                    .ConfigureAwait(false);

                if (
                    certificateDetails.CheckExpired 
                    && certificate.Value.Properties.ExpiresOn.HasValue
                    && certificate.Value.Properties.ExpiresOn.Value < DateTime.UtcNow
                    )
                {
                    throw new Exception(KvConstants.ExpiredCertificateMessage(
                        certificateDetails.CertificateName,
                        certificate.Value.Properties.ExpiresOn.Value
                    ));
                }
            }

            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return new HealthCheckResult(jobContext.Scheduler.FailureStatus, exception: ex);
        }
    }
    
    private KeyClient CreateKeyClient(Uri keyVaultUri)
        => HealthChecksClientCache
            .GetOrAdd(ClientCacheKeys.KeyVaultClient("KeyClient", keyVaultUri), _ =>
            {
                //TODO: Create V2 to Support other types of Azure Credentials
                var credentials = new DefaultAzureCredential();
                return new KeyClient(keyVaultUri, credentials);
            });

    private SecretClient CreateSecretClient(Uri keyVaultUri)
        => HealthChecksClientCache
            .GetOrAdd(ClientCacheKeys.KeyVaultClient("SecretClient", keyVaultUri), _ =>
            {
                //TODO: Create V2 to Support other types of Azure Credentials
                var credentials = new DefaultAzureCredential();
                return new SecretClient(keyVaultUri, credentials);
            });

    private CertificateClient CreateCertificateClient(Uri keyVaultUri) 
        => HealthChecksClientCache
            .GetOrAdd(ClientCacheKeys.KeyVaultClient("CertificateClient", keyVaultUri), _ =>
            {
                //TODO: Create V2 to Support other types of Azure Credentials
                var credentials = new DefaultAzureCredential();
                return new CertificateClient(keyVaultUri, credentials);
            });
}