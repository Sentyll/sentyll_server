using Azure.Core;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Sentyll.Infrastructure.HealthChecks.Azure.ServiceBus.Core.Constants;

namespace Sentyll.Infrastructure.HealthChecks.Azure.ServiceBus.Extensions;

internal static class ServiceBusClientExt
{
    
    public static ServiceBusClient CreateClient(string fullyQualifiedNamespace, string? tenantId, string? clientId, string? clientSecret) 
        => !string.IsNullOrWhiteSpace(tenantId) 
           || !string.IsNullOrWhiteSpace(tenantId) 
           || !string.IsNullOrWhiteSpace(tenantId)
            //IF Some form of Client Credentials where available use Client Credentials
            ? CreateClient(fullyQualifiedNamespace, CreateCredentials(tenantId, clientId, clientSecret))
            //If no client credentials were pass don't use Client Credentials
            : CreateClient(fullyQualifiedNamespace);

    public static ServiceBusClient CreateClient(string fullyQualifiedNamespace)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fullyQualifiedNamespace);
        
        return new ServiceBusClient(fullyQualifiedNamespace, new ServiceBusClientOptions()
        {
            ConnectionIdleTimeout = SbConstants.DefaultIdleTime
        });
    }
    
    public static ServiceBusClient CreateClient(string fullyQualifiedNamespace, TokenCredential credentials)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fullyQualifiedNamespace);
        ArgumentNullException.ThrowIfNull(credentials);
        
        return new ServiceBusClient(fullyQualifiedNamespace, credentials, new ServiceBusClientOptions()
        {
            ConnectionIdleTimeout = SbConstants.DefaultIdleTime
        });
    }

    public static ServiceBusAdministrationClient CreateManagementClient(string fullyQualifiedNamespace, string? tenantId, string? clientId, string? clientSecret) 
        => !string.IsNullOrWhiteSpace(tenantId) 
           || !string.IsNullOrWhiteSpace(tenantId) 
           || !string.IsNullOrWhiteSpace(tenantId)
            //IF Some form of Client Credentials where available use Client Credentials
            ? CreateManagementClient(fullyQualifiedNamespace, CreateCredentials(tenantId, clientId, clientSecret))
            //If no client credentials were pass don't use Client Credentials
            : CreateManagementClient(fullyQualifiedNamespace);

    public static ServiceBusAdministrationClient CreateManagementClient(string fullyQualifiedNamespace)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fullyQualifiedNamespace);
        
        return new ServiceBusAdministrationClient(fullyQualifiedNamespace, new ServiceBusAdministrationClientOptions()
        {
            
        });
    }

    public static ServiceBusAdministrationClient CreateManagementClient(string fullyQualifiedNamespace, TokenCredential credentials)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fullyQualifiedNamespace);
        ArgumentNullException.ThrowIfNull(credentials);
        
        return new ServiceBusAdministrationClient(fullyQualifiedNamespace, credentials, new ServiceBusAdministrationClientOptions()
        {
            
        });
    }
    
    private static ClientSecretCredential CreateCredentials(string? tenantId, string? clientId, string? clientSecret)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantId);
        ArgumentException.ThrowIfNullOrWhiteSpace(clientId);
        ArgumentException.ThrowIfNullOrWhiteSpace(clientSecret);
        
        return new ClientSecretCredential(
            tenantId: tenantId,
            clientId: clientId,
            clientSecret: clientSecret
        );
    }
}