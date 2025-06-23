using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;
using Sentyll.Domain.Common.Abstractions.Enums;
using Sentyll.Domain.Common.Abstractions.Models.Definitions.HealthChecks.Payload;
using Sentyll.Domain.Common.Abstractions.Models.Results;
using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Jobs;
using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Models.Context;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Services;

namespace Sentyll.Infrastructure.HealthChecks.Abstractions.Jobs;

public abstract class HealthCheckJob<TJob, TRequest>(
    IHealthCheckJobDependencyManager dependencyManager,
    HealthCheckType healthCheckType
    ) : Job<TJob, HealthCheckPayloadDefinition<TRequest>>(healthCheckType.ToString())
    where TJob : Job<TJob, HealthCheckPayloadDefinition<TRequest>>
    where TRequest : IValidatable
{
    
    public override async Task ExecuteAsync(
        SchedulerFunctionContext<HealthCheckPayloadDefinition<TRequest>> functionContext,
        CancellationToken cancellationToken)
    {
        //TODO: Add Try Catch statement here potentially? not sure if its required just yet.
        
        var logger = dependencyManager.LoggerFactory.CreateLogger<HealthCheckJob<TJob, TRequest>>();
        
        logger.LogInformation("Health Check Job [{jobType}] initiated, [JobId: {jobId}, IsDue: {isDue}]", 
            healthCheckType.ToString(), functionContext.Id, functionContext.IsDue);

        var hcResult = await CheckAsync(functionContext.Request, cancellationToken).ConfigureAwait(false);
        
        logger.LogInformation("Entity Health Checked, Attempting to push to execution Queue for further processing.");

        var unstructuredResult = UnstructuredResult.FromType(functionContext.Request);
        
        await dependencyManager.HealthCheckExecutionQueue
            .PublishAsync(new (healthCheckType, hcResult, unstructuredResult), cancellationToken)
            .ConfigureAwait(false);

        logger.LogInformation("Health Check Job Execution event published successfully.");
    }

    public abstract Task<HealthCheckResult> CheckAsync(
        HealthCheckPayloadDefinition<TRequest> jobContext,
        CancellationToken cancellationToken
    );

}