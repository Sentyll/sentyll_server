using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;

namespace Sentyll.Infrastructure.HealthChecks.Azure.KeyVault.Core.Models.Definitions;

public sealed class AzureKeyVaultV1Parameters : IValidatable
{
    
    /// <summary>
    /// The AzureKeyVault service uri.
    /// </summary>
    [JsonPropertyName("uri")]
    public Uri Uri { get; set; }

    [JsonPropertyName("certificates")] 
    public AzureKeyVaultCertificate[] Certificates { get; set; }

    [JsonPropertyName("keys")]
    public string[] Keys { get; set; }
    
    [JsonPropertyName("secrets")]
    public string[] Secrets { get; set; }

    public Result Validate() 
        => Result
            .FailureIf(Uri == default, "uri is required")
            .Ensure(() =>
            {
                foreach (var certificate in Certificates)
                {
                    var certificateValidationResult = certificate.Validate();
                    if (certificateValidationResult.IsFailure)
                    {
                        return certificateValidationResult;
                    }
                }

                return Result.Success();
            })
            .Ensure(() => Keys != default, "keys is required")
            .Ensure(() => Secrets != default, "secrets is required");
}