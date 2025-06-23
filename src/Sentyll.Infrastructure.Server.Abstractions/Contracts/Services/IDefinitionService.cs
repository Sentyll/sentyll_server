using Sentyll.Infrastructure.Server.Abstractions.Models.Definitions;

namespace Sentyll.Infrastructure.Server.Abstractions.Contracts.Services;

public interface IDefinitionService
{
    
    Result<IList<SchemaDefinition>> GetEndpointDefinitions();
    
    Result<IList<SchemaDefinition>> GetHealthCheckDefinitions();
    
    Result<SchemaDefinition> GetHealthCheckCategoryDefinitions();

    Result<IList<SchemaDefinition>> GetWebhookDefinitions();
    
    Result<List<string>> GetDeployedEndpoints();
    
    Result<IList<SchemaDefinition>> GetOrderedHealthCheckDefinitions();
    
    Result<SchemaDefinition> GetHealthCheckDefinition(
        HealthCheckType type
    );
    
    // Result<IList<string>> GetDeployedHealthCheckNames();
    
}