namespace Sentyll.Infrastructure.HealthChecks.Abstractions.Constants.Storage.Cache;

public static class ClientCacheKeys
{
    private static string CleanJobId(Guid jobId)
        => jobId.ToString();
    
    private const string HealthCheck = "HEALTHCHECK";
    
    private const string HealthCheckClient = $"{HealthCheck}:CLIENT";
    
    private const string HealthCheckClientKeyVault = $"{HealthCheckClient}:KEYVAULT";

    public static string KeyVaultClient(string clientType, Uri kvUri)
        => $"{HealthCheckClientKeyVault}:{clientType}:{kvUri.ToString()}";
    
    private const string HealthCheckClientAzureServiceBus = $"{HealthCheckClient}:AZURESERVICEBUS";
    
    public static string AzureServiceBus(string clientType, Guid jobId)
        => $"{HealthCheckClientAzureServiceBus}:{clientType}:{CleanJobId(jobId)}";
    
    private const string HealthCheckClientAzureQueueStorage = $"{HealthCheckClient}:AZUREQUEUESTORAGE";
    
    public static string AzureQueueStorage(string queueName)
        => $"{HealthCheckClientAzureQueueStorage}:{queueName}";
    
    private const string HealthCheckClientAzureFileShare = $"{HealthCheckClient}:AZUREFILESHARE";
    
    public static string AzureFileShare(string shareName)
        => $"{HealthCheckClientAzureFileShare}:{shareName}";
    
    private const string HealthCheckClientAzureBlob = $"{HealthCheckClient}:AZUREBLOB";
    
    public static string AzureBlob(string containerName)
        => $"{HealthCheckClientAzureBlob}:{containerName}";
    
    private const string HealthCheckClientRabbitMq = $"{HealthCheckClient}:RABBITMQ";
    
    public static string RabbitMq(Uri endpointUri)
        => $"{HealthCheckClientRabbitMq}:{endpointUri}";
    
    private const string HealthCheckClientRedis = $"{HealthCheckClient}:REDIS";
    
    //TODO: I DON"T LIKE THE IDEA OF USING THE CONNECTION STRING AS CACHE KEYS
    public static string Redis(string connectionString)
        => $"{HealthCheckClientRedis}:{connectionString}";
    
}