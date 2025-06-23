namespace Sentyll.Domain.Common.Abstractions.Enums;

public enum HealthCheckType
{
    /// <summary>
    /// Microsoft SQL Server Database <br />
    /// - https://www.microsoft.com/en-za/sql-server/sql-server-downloads
    /// </summary>
    SQLSERVER_DATABASE_V1 = 0,
    
    /// <summary>
    /// Microsoft SQL Server Database Query <br />
    /// - https://www.microsoft.com/en-za/sql-server/sql-server-downloads
    /// </summary>
    /// <remarks>
    /// Similar too <see cref="SQLSERVER_DATABASE_V1"/> but with this type the user can specify the SQL query. 
    /// </remarks>
    SQLSERVER_DATABASE_QUERY_V1 = 1,
    
    /// <summary>
    /// Azure Function App (HTTP function) <br />
    /// - https://learn.microsoft.com/en-us/azure/azure-functions/functions-bindings-http-webhook-trigger
    /// </summary>
    /// <remarks>
    /// This type is a replicate of <see cref="URIS_PING_V1"/>
    /// </remarks>
    AZURE_COMPUTE_FUNCTIONAPP_ENDPOINT_V1 = 2,
    
    /// <summary>
    /// Azure Key Vault <br />
    /// - https://azure.microsoft.com/en-us/products/key-vault
    /// </summary>
    AZURE_KEYVAULT_V1 = 3,
    
    /// <summary>
    /// Azure SignalR <br />
    /// - https://azure.microsoft.com/en-us/products/signalr-service
    /// </summary>
    AZURE_SIGNALR_V1 = 4,
    
    /// <summary>
    /// Redis <br />
    /// - https://redis.io/
    /// </summary>
    REDIS_V1 = 5,
    
    /// <summary>
    /// Azure Application Insights <br />
    /// - https://azure.microsoft.com/en-us/pricing/details/monitor/
    /// </summary>
    AZURE_APPLICATIONINSIGHTS_V1 = 6,
    
    /// <summary>
    /// Http endpoint (public)
    /// </summary>
    URIS_PING_V1 = 7,
    
    /// <summary>
    /// Azure Queue Storage <br />
    /// - https://azure.microsoft.com/en-us/products/storage/queues
    /// </summary>
    AZURE_STORAGE_QUEUES_V1 = 8,
    
    /// <summary>
    /// Azure File share <br />
    /// - https://azure.microsoft.com/en-us/products/storage/files
    /// </summary>
    AZURE_STORAGE_FILES_SHARES_V1 = 9,
    
    /// <summary>
    /// Azure blob storage
    /// - https://azure.microsoft.com/en-us/products/storage/blobs
    /// </summary>
    AZURE_STORAGE_BLOB_V1 = 10,
    
    /// <summary>
    /// OpenId Connect Server
    /// - https://openid.net/developers/how-connect-works/
    /// </summary>
    OPENIDCONNECTSERVER_V1 = 11,
    
    /// <summary>
    /// Azure Service Bus Queue
    /// - https://azure.microsoft.com/en-us/products/service-bus
    /// </summary>
    AZURE_SERVICEBUS_QUEUE_V1 = 12,
    
    /// <summary>
    /// Azure Service Bus Topic
    /// - https://azure.microsoft.com/en-us/products/service-bus
    /// </summary>
    AZURE_SERVICEBUS_TOPIC_V1 = 13,
    
    /// <summary>
    /// Azure Service Bus Subscription
    /// - https://azure.microsoft.com/en-us/products/service-bus
    /// </summary>
    AZURE_SERVICEBUS_SUBSCRIPTION_V1 = 14,
    
    /// <summary>
    /// Azure Service Bus Queue Message Count Thresholds 
    /// - https://azure.microsoft.com/en-us/products/service-bus
    /// </summary>
    AZURE_SERVICEBUS_QUEUE_MESSAGECOUNTTHRESHOLD_V1 = 15,
    
    /// <summary>
    /// RabbitMq
    /// - https://www.rabbitmq.com/
    /// </summary>
    RABBITMQ_V1 = 16,
    
    /// <summary>
    /// PostgreSQL Database (ADO.NET Data Provider)
    /// - https://www.npgsql.org/
    /// </summary>
    NPQSQL_DATABASE_V1 = 17,
    
    /// <summary>
    /// PostgreSQL Database Query (ADO.NET Data Provider)
    /// - https://www.npgsql.org/
    /// </summary>
    /// <remarks>
    /// Similar too <see cref="NPQSQL_DATABASE_V1"/> but with this type the user can specify the SQL query. 
    /// </remarks>
    NPQSQL_DATABASE_QUERY_V1 = 18,
    
    /// <summary>
    /// MongoDb
    /// - https://www.mongodb.com/
    /// </summary>
    MONGODB_V1 = 19
}