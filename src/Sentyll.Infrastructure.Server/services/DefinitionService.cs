using Sentyll.Domain.Common.Abstractions.Extensions.Results;
using Sentyll.Domain.Common.Abstractions.Extensions.Serialization;
using Microsoft.AspNetCore.Routing;
using UnStructuredResultExtensions = Sentyll.Domain.Common.Abstractions.Extensions.Results.UnStructuredResultExtensions;

namespace Sentyll.Infrastructure.Server.services;

internal sealed class DefinitionService : IDefinitionService
{

    private readonly IEnumerable<EndpointDataSource> _endpointSources;
    
    public DefinitionService(
        IEnumerable<EndpointDataSource> endpointSources)
    {
        _endpointSources = endpointSources;
    }
    
    public Result<IList<SchemaDefinition>> GetEndpointDefinitions()
        => FileReader
            .ReadFiles("Definitions", "Endpoints")
            .Bind(files => files.DeserializeCollection<SchemaDefinition>());
    
    public Result<IList<SchemaDefinition>> GetHealthCheckDefinitions()
        => FileReader
            .ReadFiles("Definitions", "HealthChecks")
            .Bind(files => files.DeserializeCollection<SchemaDefinition>());
    
    /// <summary>
    /// TODO: MOVE DEFINITION GET REQUESTS TO TARGET LIBRARIES.
    /// </summary>
    /// <returns></returns>
    public Result<SchemaDefinition> GetHealthCheckCategoryDefinitions()
        => FileReader
            .ReadFile("Definitions", "HealthCheckCategories", "healthCheckCategoryDefinition.v1.json")
            .Bind(files => files.Deserialize<SchemaDefinition>());
    
    public Result<IList<SchemaDefinition>> GetWebhookDefinitions()
        => FileReader
            .ReadFiles("Definitions", "Webhooks")
            .Bind(files => files.DeserializeCollection<SchemaDefinition>());
    
    /// <summary>
    /// TODO: Move to Routing Service rather in the Server library?
    /// </summary>
    /// <returns></returns>
    public Result<List<string>> GetDeployedEndpoints() =>
        _endpointSources
            .SelectMany(es => es.Endpoints)
            .Where(e => e.DisplayName == "Health checks")
            .Select(e =>
                {
                    //TODO: THIS FEELS LIKE A MISTAKE TO GET THE ROUTE DICTIONARY ITEM USING AN INDEX
                    //TODO: Refactor to make use of Bind Functions and errors
                    
                    return e.Metadata[1].ToString().Replace("Route: ", "");
                }
            ).ToList();
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    public Result<IList<SchemaDefinition>> GetOrderedHealthCheckDefinitions()
        => GetHealthCheckCategoryDefinitions()
            .Map(definition =>
            {
                return definition;
                
                // return definition.Categories
                //     .OrderBy(_ => _.SortOrder)
                //     .ToList();
            })
            .Bind(orderedCategories =>
            {
                return GetHealthCheckDefinitions()
                    .Bind(definitions =>
                    {
                        // var result = new List<SchemaDefinition>();
                        // foreach (var category in orderedCategories)
                        // {
                        //     var groupHealthChecks = definitions
                        //         .Where(def => UnStructuredResultExtensions.Contains(def.Meta, "healthCheckCategory", category.Id))
                        //         .ToList();
                        //
                        //     result.AddRange(groupHealthChecks);
                        // }

                        return Result.Success<IList<SchemaDefinition>>(definitions);
                    });
            });

    public Result<SchemaDefinition> GetHealthCheckDefinition(HealthCheckType type)
        => GetHealthCheckDefinitions()
            .Bind(definitions =>
            {
                return definitions
                    .FirstOrDefault(def => def.Meta.Contains("healthCheckType", type))
                    .AsMaybe()
                    .ToResult(DefinitionFailures.NotFound.ToString());
            });
    
    // public Result<IList<string>> GetDeployedHealthCheckNames() =>
    //     _healthChecks.Registrations
    //         .Select(s => s.Name)
    //         .ToList();
    
}
