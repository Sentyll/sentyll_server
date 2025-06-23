using CSharpFunctionalExtensions;
using Sentyll.Domain.Common.Abstractions.Extensions.CSharpFunctionalExtensions;
using Sentyll.Infrastructure.Server.TaskPipeline.Abstraction.Contracts.Services.EventProcessors;
using Sentyll.Infrastructure.Server.TaskPipeline.Abstraction.Models.EventProcessors;
using Microsoft.Extensions.Logging;

namespace Sentyll.Infrastructure.Server.TaskPipeline.Services.EventProcessors;

/// <summary>
/// Simple Processor for Debugging purposes to debug health check execution results
/// TODO: Delete later?
/// 
/// </summary>
/// <param name="logger"></param>
internal sealed class LoggerServerEventProcessor(
    ILogger<LoggerServerEventProcessor> logger
    ) : IServerEventProcessor
{

    public Task<Result> ProcessAsync(HealthCheckEventRequest eventRequest, CancellationToken cancellationToken = default)
        => Result
            .Success()
            .Tap(() =>
            {
                logger.LogInformation("""
                                      ======================================
                                      Health Check Execution Result:
                                      ======================================
                                      HealthCheckType: {healthCheckType}
                                      Name: {name}
                                      Description: {description}
                                      Health Status: {healthStatus}
                                      Health Description: {healthDescription}
                                      ======================================
                                      """,
                    eventRequest.HealthCheckProfile.Type.ToString(),
                    eventRequest.HealthCheckProfile.Name,
                    eventRequest.HealthCheckProfile.Description,
                    eventRequest.JobResult.Status.ToString(),
                    eventRequest.JobResult.Exception?.Message
                );
            })
            .ToTask();
    
}