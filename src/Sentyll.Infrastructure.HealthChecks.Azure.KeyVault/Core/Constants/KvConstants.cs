namespace Sentyll.Infrastructure.HealthChecks.Azure.KeyVault.Core.Constants;

internal static class KvConstants
{
    public const string NothingToCheckFailureMessage = "No keys, certificates or secrets configured.";

    public static string ExpiredCertificateMessage(string certificateName, DateTimeOffset expiredOn)
        => $"The certificate with key {certificateName} has expired with date {expiredOn}";

}