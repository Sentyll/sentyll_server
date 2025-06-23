namespace Sentyll.Domain.Common.Models.ApiResult.Events;

public sealed record MessagingEventResult(
    Guid Id,
    bool IsEnabled,
    string Name,
    string? Description,
    MessagingEventType Type
    );