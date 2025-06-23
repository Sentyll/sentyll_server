namespace Sentyll.UI.Core.Models.Failure;

internal sealed record BadRequestFailureResult(
    string Type,
    string Title,
    int Status,
    object Errors,
    string TraceId
);