namespace Sentyll.Domain.Data.Services.Extensions;

internal static class EntityExtensions
{
    
    public static (IList<TSource>, IList<TSource>) MergeParameterEntities<TSource>(
        this IList<TSource> destinationParameters,
        IList<TSource> sourceParameters)
        where TSource : IParameterEntity
    {
        var parameterNames = sourceParameters.Select(p => p.Key).ToList();
        var savedParams = destinationParameters.Where(p => parameterNames.Contains(p.Key)).ToList();
        
        var updatedEndpointParameters = savedParams.Join(
            sourceParameters,
            saved => saved.Key,
            updated => updated.Key,
            (saved, updated) =>
            {
                saved.Value = updated.Value;
                return saved;
            }).ToList();
        
        var savedParameterNames = updatedEndpointParameters
            .Select(p => p.Key)
            .ToList();
        
        var newParams = destinationParameters
            .Where(p => !savedParameterNames.Contains(p.Key))
            .ToList();

        return (updatedEndpointParameters, newParams);
    }
    
}