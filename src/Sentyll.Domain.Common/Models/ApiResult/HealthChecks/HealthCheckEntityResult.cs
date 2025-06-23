namespace Sentyll.Domain.Common.Models.ApiResult.HealthChecks;

public sealed record HealthCheckEntityResult(
    Guid Id,
    string Name,
    string? Description,
    HealthCheckType Type,
    string[]? Tags
    );