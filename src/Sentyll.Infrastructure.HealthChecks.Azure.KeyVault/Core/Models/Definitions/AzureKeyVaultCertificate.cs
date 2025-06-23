using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;

namespace Sentyll.Infrastructure.HealthChecks.Azure.KeyVault.Core.Models.Definitions;

public sealed class AzureKeyVaultCertificate : IValidatable
{
    [JsonPropertyName("certificateName")]
    public string CertificateName  { get; set; }

    [JsonPropertyName("checkExpired")]
    public bool CheckExpired { get; set; } = false;

    public Result Validate()
        => Result.FailureIf(string.IsNullOrWhiteSpace(CertificateName), "certificateName is required");
}