using Sentyll.Domain.Common.Abstractions.Enums;
using Sentyll.Domain.Common.Abstractions.Models.Definitions.HealthChecks.Payload;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Jobs;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Services;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Sentyll.Infrastructure.HealthChecks.Azure.SignalR.Core.Models.Definitions;

namespace Sentyll.Infrastructure.HealthChecks.Azure.SignalR.HealthChecks;

internal sealed class AzureSignalrV1HealthCheck(
    IHealthCheckJobDependencyManager dependencyManager
    ) : HealthCheckJob<AzureSignalrV1HealthCheck, AzureSignalrV1Parameter>(
        dependencyManager,
        HealthCheckType.AZURE_SIGNALR_V1
    )
{
    
    public override async Task<HealthCheckResult> CheckAsync(
        HealthCheckPayloadDefinition<AzureSignalrV1Parameter> jobContext, 
        CancellationToken cancellationToken)
    {
        HubConnection? connection = null;

        try
        {
            connection = new HubConnectionBuilder()
                .WithUrl(jobContext.HealthCheck.Uri)
                .Build();

            await connection
                .StartAsync(cancellationToken)
                .ConfigureAwait(false);

            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return new HealthCheckResult(jobContext.Scheduler.FailureStatus, exception: ex);
        }
        finally
        {
            if (connection != null)
            {
                await connection
                    .DisposeAsync()
                    .ConfigureAwait(false);
            }
        }
    }
    
}