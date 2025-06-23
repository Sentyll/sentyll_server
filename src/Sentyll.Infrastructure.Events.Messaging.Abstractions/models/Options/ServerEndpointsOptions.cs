namespace Sentyll.Infrastructure.Events.Messaging.Abstractions.models.Options;

public sealed class ServerEndpointsOptions
{
    
    /// <summary>
    /// TODO: Get this from AppConfiguration?.
    /// </summary>
    public readonly string ServerAddress = "https://localhost:7184";

    public string HomePage => $"{ServerAddress}/";
    
    public string HealthCheckProfile(Guid healthCheckId)
        => $"{ServerAddress}/HealthChecks/{healthCheckId}";
    
    /// <summary>
    /// TODO: Not implemented, but the idea is to provide the page where a user reasons why it went down.
    /// </summary>
    /// <param name="healthCheckId"></param>
    /// <returns></returns>
    public string HealthCheckAuditAction(Guid healthCheckId)
        => $"{ServerAddress}/HealthChecks/{healthCheckId}/Action";

}