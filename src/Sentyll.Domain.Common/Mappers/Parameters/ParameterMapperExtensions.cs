using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;
using Sentyll.Domain.Data.Abstractions.Contracts.Entities;
using Sentyll.Domain.Data.Abstractions.Entities.Settings;

namespace Sentyll.Domain.Common.Mappers.Parameters;

public static class ParameterMapperExtensions
{
    
    public static IDictionary<string, string> MapToDictionary(this IEnumerable<IParameterEntity> parameters) =>
        parameters.ToDictionary(
            param => param.Key,
            param => param.Value
        );
    
    public static IReadOnlyDictionary<string, string> MapToReadOnlyDictionary(this IEnumerable<IParameterEntity> parameters) =>
        parameters.ToDictionary(
            param => param.Key,
            param => param.Value
        );

    public static Result<List<ConfigurationEntity>> ToConfigurationEntities<T>(this T parametersObj)
        where T : IValidatable, new()
        => parametersObj
            .Validate()
            .Map(() => UnstructuredResult.FromType(parametersObj).ToKeyValuePair())
            .Map((valuePairs) => valuePairs
                .Select(pair => new ConfigurationEntity()
                {
                    Key = pair.Key,
                    Value = pair.Value?.ToString() ?? string.Empty
                }).ToList()
            );

}