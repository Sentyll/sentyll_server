using Sentyll.Domain.Common.Abstractions.Models.Definitions.HealthChecks.Payload;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Sentyll.Infrastructure.Events.Messaging.Abstractions.models.Request;

public sealed record GenerateTemplateRequest(
    HealthCheckOverViewPayloadDefinition HealthCheckProfile,
    HealthCheckResult JobResult,
    DateTime ExecutedOn
);