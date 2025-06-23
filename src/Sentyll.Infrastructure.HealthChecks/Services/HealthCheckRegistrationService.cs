using CSharpFunctionalExtensions;
using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;
using Sentyll.Domain.Common.Abstractions.Models.Definitions.HealthChecks.Payload;
using Sentyll.Domain.Data.Abstractions.Entities.Scheduler;
using Sentyll.Infrastructure.HealthChecks.Abstractions.Services;
using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Extensions;
using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Services.Scheduler;

namespace Sentyll.Infrastructure.HealthChecks.Services;

internal sealed class HealthCheckRegistrationService(
    ISchedulerJobStateManager schedulerJobStateManager
    ) : IHealthCheckRegistrationService
{
    
    public async Task<Result> ScheduleHealthCheckAsync<T>(
        HealthCheckPayloadDefinition<T> payloadDefinition,
        CancellationToken cancellationToken = default)
        where T : IValidatable, new()
        => await payloadDefinition
            .Validate()
            .Bind(() => payloadDefinition.CreateSchedulerJobRequest())
            .Bind(jobRequest => schedulerJobStateManager
                .AddCronJobAsync(new CronJobEntity()
                {
                    Request = jobRequest,
                    Expression = payloadDefinition.Scheduler.Schedule,
                    Function = payloadDefinition.Type.ToString(),
                    Description = payloadDefinition.Description,
                    // TODO:
                    // These retry values should potentially be configurable from the UI? I would assume some users would like
                    // to retry job invocation in the slight chance some "Health Checks" are unstable? Which we don't hope for :) 
                    Retries = 3,
                    RetryIntervals = [20, 60, 100]
                }, cancellationToken))
            .ConfigureAwait(false);
}