using System.Collections.Immutable;
using Sentyll.Domain.Data.Abstractions.Entities.Events;
using Sentyll.Domain.Data.Services.Abstractions.Contracts.Repositories.HealthChecks;
using Sentyll.Infrastructure.Server.TaskPipeline.Core.Contracts.Services.EventResolvers;

namespace Sentyll.Infrastructure.Server.TaskPipeline.Services.EventResolvers;

internal sealed class EventDescriptorProvider(
    IHealthCheckEventEntityRepository healthCheckEventEntityRepository
    ) : IEventDescriptorProvider
{

    public async Task<Result<ImmutableList<EventEntity>>> ResolveMessagingProcessorsAsync(
        Guid healthCheckId,
        CancellationToken cancellationToken = default) 
        => await healthCheckEventEntityRepository
            .GetMessagingEventsAsync(healthCheckId, cancellationToken)
            .Map(events => events.ToImmutableList())
            .ConfigureAwait(false);
}