namespace Sentyll.Domain.Data.Services.Abstractions.Contracts.Services.Events;

public interface IEventsParameterDataService
{
    Task<Result<Dictionary<string, object?>>> GetEventParametersAsync(
        Guid eventId,
        CancellationToken cancellationToken = default);

    Task<Result<TEventParams>> GetParsedEventParametersAsync<TEventParams>(
        Guid eventId,
        CancellationToken cancellationToken = default)
        where TEventParams : new();
}