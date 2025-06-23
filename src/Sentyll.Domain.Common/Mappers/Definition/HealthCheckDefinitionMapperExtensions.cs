using Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;
using Sentyll.Domain.Common.Abstractions.Models.Definitions.HealthChecks.Payload;
using Sentyll.Domain.Common.Mappers.Parameters;
using Sentyll.Domain.Data.Abstractions.Entities.HealthChecks;

namespace Sentyll.Domain.Common.Mappers.Definition;

public static class HealthCheckDefinitionMapperExtensions
{

    public static Result<List<HealthCheckParameterEntity>> ToHealthCheckParameterEntities<T>(this T parameterObj)
        where T : IValidatable, new()
        => parameterObj
            .ToConfigurationEntities()
            .Map((configurations) => configurations
                .Select(config => new HealthCheckParameterEntity()
                {
                    Configuration = config
                })
                .ToList()
            );
    
    public static Result<HealthCheckEntity> ToHealthCheckEntity<T>(
        this HealthCheckRestContentPayloadDefinition<T> definition,
        HealthCheckType type)
        where T : IValidatable, new()
        => definition.HealthCheck
            .ToHealthCheckParameterEntities()
            .Map(parameters => new HealthCheckEntity()
            {
                Name = definition.Name,
                Description = definition.Description,
                Tags = definition.Tags,
                Type = type,
                IsEnabled = definition.IsEnabled,
                Parameters = parameters
            });

    public static Result<HealthCheckPayloadDefinition<T>> ToHealthCheckPayloadDefinition<T>(
        this HealthCheckRestContentPayloadDefinition<T> definition,
        Guid healthCheckId,
        HealthCheckType type)
        where T : IValidatable, new()
        => definition
            .Validate()
            .Map(() => new HealthCheckPayloadDefinition<T>()
            {
                Id = healthCheckId,
                Type = type,
                Name = definition.Name,
                Description = definition.Description,
                Tags = definition.Tags,
                IsEnabled = definition.IsEnabled,
                Scheduler = definition.Scheduler,
                HealthCheck = definition.HealthCheck
            });

}