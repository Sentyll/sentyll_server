using Sentyll.Domain.Common.Abstractions.Models.Definitions.HealthChecks.Payload;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Sentyll.Infrastructure.Server.TaskPipeline.Abstraction.Models.EventProcessors;

public readonly record struct HealthCheckEventRequest(
    Guid EventId,
    HealthCheckOverViewPayloadDefinition HealthCheckProfile,
    HealthCheckResult JobResult,
    DateTime ExecutedOn
    );