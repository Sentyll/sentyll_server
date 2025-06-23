using Sentyll.Domain.Data.Services.Abstractions.Contracts.Repositories.Events;
using Sentyll.Domain.Data.Services.Abstractions.Contracts.Services.Events;

namespace Sentyll.Domain.Data.Services.Services.Events;

internal sealed class EventsParameterDataService(
    IEventParameterEntityRepository eventParameterEntityRepository
    ) : IEventsParameterDataService
{

    public async Task<Result<Dictionary<string, object?>>> GetEventParametersAsync(
        Guid eventId,
        CancellationToken cancellationToken = default)
        => await eventParameterEntityRepository
            .GetEventParameters(eventId, cancellationToken)
            .Map(eventParams => eventParams
                .Select(eventParam => eventParam.Configuration)
                .ToDictionary(
                    parameter => parameter.Key,
                    parameter => (object?)parameter.Value
                )
            );
    
    public async Task<Result<TEventParams>> GetParsedEventParametersAsync<TEventParams>(
        Guid eventId,
        CancellationToken cancellationToken = default)
        where TEventParams : new()
        => await GetEventParametersAsync(eventId, cancellationToken)
            .Bind(eventParams => UnstructuredResult
                .FromDictionary(eventParams)
                .Bind<TEventParams>()
            );

}