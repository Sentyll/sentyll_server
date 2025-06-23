using System.Net;

namespace Sentyll.Infrastructure.HealthChecks.Redis.Core.Constants;

internal static class RdsConstants
{
    
    public const string DefaultCommand = "CLUSTER";
    
    public static readonly object[] DefaultCommandArgs = ["INFO"];
    
    public const string TimeoutMessage = "Healthcheck timed out";

    public const string ClusterValidState = "cluster_state:ok";
    
    public static string ClusterNotOkMessage(EndPoint endpoint)
        => $"INFO CLUSTER is not on OK state for endpoint {endpoint}";

    public static string ClusterNullMessage(EndPoint endpoint)
        => $"INFO CLUSTER is null or can't be read for endpoint {endpoint}";

}