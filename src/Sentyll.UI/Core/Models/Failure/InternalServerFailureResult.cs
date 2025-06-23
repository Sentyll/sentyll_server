namespace Sentyll.UI.Core.Models.Failure;

internal sealed record InternalServerFailureResult(
    string Code,
    string Title,
    int Status,
    string? Reason,
    string TraceId
        );