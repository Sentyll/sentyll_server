using Sentyll.Domain.Common.Abstractions.Enums;
using Sentyll.Domain.Common.Abstractions.Extensions.Definitions;
using Sentyll.Domain.Common.Abstractions.Models.Definitions.HealthChecks.Payload;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Jobs;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Sentyll.Infrastructure.HealthChecks.Uris.Core.Models.Definitions;
using Sentyll.Infrastructure.HealthChecks.Uris.Core.Models.Options;
using Sentyll.Infrastructure.HealthChecks.Uris.Services;

namespace Sentyll.Infrastructure.HealthChecks.Uris.HealthChecks;

internal sealed class UriPingV1HealthCheck(
    IHealthCheckJobDependencyManager dependencyManager,
    HttpService httpService
) : HealthCheckJob<UriPingV1HealthCheck, UriPingV1Parameters>(
    dependencyManager,
    HealthCheckType.URIS_PING_V1
)
{

    public override async Task<HealthCheckResult> CheckAsync(
        HealthCheckPayloadDefinition<UriPingV1Parameters> jobContext, 
        CancellationToken cancellationToken)
    {
        try
        {
            var executionOptions = new UriOptions(jobContext.HealthCheck.Uri);
            
            foreach (var header in jobContext.HealthCheck.Headers.ExtractKeyValueTags())
            {
                executionOptions.AddCustomHeader(header.Key, header.Value);
            }

            var executionResult = await httpService
                .ExecuteHttpRequestAsync(executionOptions, null, cancellationToken)
                .ConfigureAwait(false);

            return executionResult.Status == HealthStatus.Healthy
                ? HealthCheckResult.Healthy()
                : new HealthCheckResult(jobContext.Scheduler.FailureStatus, description: executionResult.Description);
        }
        catch (Exception ex)
        {
            return new HealthCheckResult(jobContext.Scheduler.FailureStatus, exception: ex);
        }
    }
}