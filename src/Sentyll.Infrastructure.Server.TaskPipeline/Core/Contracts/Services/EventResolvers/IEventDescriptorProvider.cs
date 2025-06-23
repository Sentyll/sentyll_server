using System.Collections.Immutable;
using Sentyll.Domain.Data.Abstractions.Entities.Events;

namespace Sentyll.Infrastructure.Server.TaskPipeline.Core.Contracts.Services.EventResolvers;

internal interface IEventDescriptorProvider
{
    Task<Result<ImmutableList<EventEntity>>> ResolveMessagingProcessorsAsync(
        Guid healthCheckId,
        CancellationToken cancellationToken = default);
}