using CSharpFunctionalExtensions;
using Sentyll.Infrastructure.Server.TaskPipeline.Abstraction.Models.EventProcessors;

namespace Sentyll.Infrastructure.Server.TaskPipeline.Abstraction.Contracts.Services.EventProcessors;

public interface IEventProcessor
{
    
    public Task<Result> ProcessAsync(
        HealthCheckEventRequest eventRequest,
        CancellationToken cancellationToken = default
    );
    
}