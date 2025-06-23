using System.Net;
using Sentyll.Core.Services.Abstractions.Contracts.Services.Crud.HealthChecks;
using Sentyll.Domain.Common.Abstractions.Enums;
using Sentyll.Domain.Common.Abstractions.Models.Definitions.HealthChecks.Payload;
using Sentyll.Infrastructure.HealthChecks.Azure.ApplicationInsights.Core.Models.Definitions;
using Sentyll.Infrastructure.HealthChecks.Azure.Compute.FunctionApp.Core.Models.Definitions;
using Sentyll.Infrastructure.HealthChecks.Azure.KeyVault.Core.Models.Definitions;
using Sentyll.Infrastructure.HealthChecks.Azure.ServiceBus.Core.Models.Definitions;
using Sentyll.Infrastructure.HealthChecks.Azure.SignalR.Core.Models.Definitions;
using Sentyll.Infrastructure.HealthChecks.Azure.Storage.Blobs.Core.Models.Definitions;
using Sentyll.Infrastructure.HealthChecks.Azure.Storage.Files.Shares.Core.Models.Definitions;
using Sentyll.Infrastructure.HealthChecks.Azure.Storage.Queues.Core.Models.Definitions;
using Sentyll.Infrastructure.HealthChecks.MongoDb.Core.Models.Definitions;
using Sentyll.Infrastructure.HealthChecks.NpgSql.Core.Models.Definitions;
using Sentyll.Infrastructure.HealthChecks.OpenIdConnectServer.Core.Models.Definitions;
using Sentyll.Infrastructure.HealthChecks.Rabbitmq.Core.Models.Definitions;
using Sentyll.Infrastructure.HealthChecks.Redis.Core.Models.Definitions;
using Sentyll.Infrastructure.HealthChecks.SqlServer.Core.Models.Definitions;
using Sentyll.Infrastructure.HealthChecks.Uris.Core.Models.Definitions;
using Sentyll.UI.Core.Extensions;
using Sentyll.UI.Controllers.Base;

namespace Sentyll.UI.Controllers.V1;

/// <summary>
/// 
/// </summary>
[ApiVersion(1.0)]
[Route("api/HealthChecks/Checks")]
[ProducesResponseType((int)HttpStatusCode.OK)]
public class HealthChecksChecksController(
    IHealthChecksCrudService healthChecksCrudService
    ) : ApiController
{

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpPut("Azure/ApplicationInsights")]
    public async Task<IActionResult> CreateAzureApplicationInsightsHealthCheckAsync(
        [FromBody] HealthCheckRestContentPayloadDefinition<AzureApplicationInsightsV1Parameters> request)
        => await healthChecksCrudService
            .CreateAndQueueAsync(request, HealthCheckType.AZURE_APPLICATIONINSIGHTS_V1, CancellationToken.None)
            .OkOrFailureAsync()
            .ConfigureAwait(false);
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpPut("Azure/Compute/FunctionApp/Endpoint")]
    public async Task<IActionResult> CreateAzureComputeFunctionAppEndpointHealthCheckAsync(
        [FromBody] HealthCheckRestContentPayloadDefinition<AzureFunctionAppHttpEndpointV1Parameters> request)
        => await healthChecksCrudService
            .CreateAndQueueAsync(request, HealthCheckType.AZURE_COMPUTE_FUNCTIONAPP_ENDPOINT_V1, CancellationToken.None)
            .OkOrFailureAsync()
            .ConfigureAwait(false);
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpPut("Azure/KeyVault")]
    public async Task<IActionResult> CreateAzureKeyVaultHealthCheckAsync(
        [FromBody] HealthCheckRestContentPayloadDefinition<AzureKeyVaultV1Parameters> request)
        => await healthChecksCrudService
            .CreateAndQueueAsync(request, HealthCheckType.AZURE_KEYVAULT_V1, CancellationToken.None)
            .OkOrFailureAsync()
            .ConfigureAwait(false);
    
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpPut($"Azure/ServiceBus/Queue")]
    public async Task<IActionResult> CreateAzureServiceBusQueueHealthCheckAsync(
        [FromBody] HealthCheckRestContentPayloadDefinition<AzureServiceBusQueueV1Parameters> request)
        => await healthChecksCrudService
            .CreateAndQueueAsync(request, HealthCheckType.AZURE_SERVICEBUS_QUEUE_V1, CancellationToken.None)
            .OkOrFailureAsync()
            .ConfigureAwait(false);
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpPut($"Azure/ServiceBus/Queue/MessageCountThreshold")]
    public async Task<IActionResult> CreateAzureServiceBusQueueMessageCountThresholdHealthCheckAsync(
        [FromBody] HealthCheckRestContentPayloadDefinition<AzureServiceBusQueueMessageCountThresholdV1Parameters> request)
        => await healthChecksCrudService
            .CreateAndQueueAsync(request, HealthCheckType.AZURE_SERVICEBUS_QUEUE_MESSAGECOUNTTHRESHOLD_V1, CancellationToken.None)
            .OkOrFailureAsync()
            .ConfigureAwait(false);
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpPut($"Azure/ServiceBus/Topic")]
    public async Task<IActionResult> CreateAzureServiceBusTopicHealthCheckAsync(
        [FromBody] HealthCheckRestContentPayloadDefinition<AzureServiceBusTopicV1Parameters> request)
        => await healthChecksCrudService
            .CreateAndQueueAsync(request, HealthCheckType.AZURE_SERVICEBUS_TOPIC_V1, CancellationToken.None)
            .OkOrFailureAsync()
            .ConfigureAwait(false);
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpPut($"Azure/ServiceBus/Subscription")]
    public async Task<IActionResult> CreateAzureServiceBusSubscriptionHealthCheckAsync(
        [FromBody] HealthCheckRestContentPayloadDefinition<AzureServiceBusSubscriptionV1Parameters> request)
        => await healthChecksCrudService
            .CreateAndQueueAsync(request, HealthCheckType.AZURE_SERVICEBUS_SUBSCRIPTION_V1, CancellationToken.None)
            .OkOrFailureAsync()
            .ConfigureAwait(false);
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpPut("Azure/SignalR")]
    public async Task<IActionResult> CreateAzureSignalrHealthCheckAsync(
        [FromBody] HealthCheckRestContentPayloadDefinition<AzureSignalrV1Parameter> request)
        => await healthChecksCrudService
            .CreateAndQueueAsync(request, HealthCheckType.AZURE_SIGNALR_V1, CancellationToken.None)
            .OkOrFailureAsync()
            .ConfigureAwait(false);
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpPut("Azure/Storage/Blobs")]
    public async Task<IActionResult> CreateAzureBlobStorageHealthCheckAsync(
        [FromBody] HealthCheckRestContentPayloadDefinition<AzureBlobStorageV1Parameters> request)
        => await healthChecksCrudService
            .CreateAndQueueAsync(request, HealthCheckType.AZURE_STORAGE_BLOB_V1, CancellationToken.None)
            .OkOrFailureAsync()
            .ConfigureAwait(false);
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpPut("Azure/Storage/Files/Shares")]
    public async Task<IActionResult> CreateAzureStorageFileSharesHealthCheckAsync(
        [FromBody] HealthCheckRestContentPayloadDefinition<AzureFileShareV1Parameters> request)
        => await healthChecksCrudService
            .CreateAndQueueAsync(request, HealthCheckType.AZURE_STORAGE_FILES_SHARES_V1, CancellationToken.None)
            .OkOrFailureAsync()
            .ConfigureAwait(false);
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpPut("Azure/Storage/Queues")]
    public async Task<IActionResult> CreateAzureStorageQueueHealthCheckAsync(
        [FromBody] HealthCheckRestContentPayloadDefinition<AzureQueueStorageV1Parameters> request)
        => await healthChecksCrudService
            .CreateAndQueueAsync(request, HealthCheckType.AZURE_STORAGE_QUEUES_V1, CancellationToken.None)
            .OkOrFailureAsync()
            .ConfigureAwait(false);
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpPut("MongoDb")]
    public async Task<IActionResult> CreateMongoDbHealthCheckAsync(
        [FromBody] HealthCheckRestContentPayloadDefinition<MongoDbV1Parameters> request)
        => await healthChecksCrudService
            .CreateAndQueueAsync(request, HealthCheckType.MONGODB_V1, CancellationToken.None)
            .OkOrFailureAsync()
            .ConfigureAwait(false);
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpPut("NpqSql/Database")]
    public async Task<IActionResult> CreateNpqSqlDatabaseHealthCheckAsync(
        [FromBody] HealthCheckRestContentPayloadDefinition<NpqSqlDatabaseV1Parameters> request)
        => await healthChecksCrudService
            .CreateAndQueueAsync(request, HealthCheckType.NPQSQL_DATABASE_V1, CancellationToken.None)
            .OkOrFailureAsync()
            .ConfigureAwait(false);
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpPut("NpqSql/Database/Query")]
    public async Task<IActionResult> CreateNpqSqlDatabaseQueryHealthCheckAsync(
        [FromBody] HealthCheckRestContentPayloadDefinition<NpqSqlDatabaseQueryV1Parameters> request)
        => await healthChecksCrudService
            .CreateAndQueueAsync(request, HealthCheckType.NPQSQL_DATABASE_QUERY_V1, CancellationToken.None)
            .OkOrFailureAsync()
            .ConfigureAwait(false);
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpPut("OpenIdConnect")]
    public async Task<IActionResult> CreateOpenIdConnectHealthCheckAsync(
        [FromBody] HealthCheckRestContentPayloadDefinition<OpenIdConnectServerV1Parameters> request)
        => await healthChecksCrudService
            .CreateAndQueueAsync(request, HealthCheckType.OPENIDCONNECTSERVER_V1, CancellationToken.None)
            .OkOrFailureAsync()
            .ConfigureAwait(false);
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpPut("RabbitMq")]
    public async Task<IActionResult> CreateRabbitMqHealthCheckAsync(
        [FromBody] HealthCheckRestContentPayloadDefinition<RabbitMqV1Parameters> request)
        => await healthChecksCrudService
            .CreateAndQueueAsync(request, HealthCheckType.RABBITMQ_V1, CancellationToken.None)
            .OkOrFailureAsync()
            .ConfigureAwait(false);
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpPut("Redis")]
    public async Task<IActionResult> CreateRedisHealthCheckAsync(
        [FromBody] HealthCheckRestContentPayloadDefinition<RedisV1Parameters> request)
        => await healthChecksCrudService
            .CreateAndQueueAsync(request, HealthCheckType.REDIS_V1, CancellationToken.None)
            .OkOrFailureAsync()
            .ConfigureAwait(false);
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpPut("SqlServer/Database")]
    public async Task<IActionResult> CreateSqlServerDatabaseHealthCheckAsync(
        [FromBody] HealthCheckRestContentPayloadDefinition<SqlServerDatabaseV1Parameters> request)
        => await healthChecksCrudService
            .CreateAndQueueAsync(request, HealthCheckType.SQLSERVER_DATABASE_V1, CancellationToken.None)
            .OkOrFailureAsync()
            .ConfigureAwait(false);
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpPut("SqlServer/Database/Query")]
    public async Task<IActionResult> CreateSqlServerDatabaseQueryHealthCheckAsync(
        [FromBody] HealthCheckRestContentPayloadDefinition<SqlServerDatabaseQueryV1Parameters> request)
        => await healthChecksCrudService
            .CreateAndQueueAsync(request, HealthCheckType.SQLSERVER_DATABASE_QUERY_V1, CancellationToken.None)
            .OkOrFailureAsync()
            .ConfigureAwait(false);
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpPut("Uris/Ping")]
    public async Task<IActionResult> CreateUriPingHealthCheckAsync(
        [FromBody] HealthCheckRestContentPayloadDefinition<UriPingV1Parameters> request)
        => await healthChecksCrudService
            .CreateAndQueueAsync(request, HealthCheckType.URIS_PING_V1, CancellationToken.None)
            .OkOrFailureAsync()
            .ConfigureAwait(false);
    
}