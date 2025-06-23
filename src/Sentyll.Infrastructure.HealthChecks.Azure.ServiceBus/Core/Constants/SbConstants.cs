namespace Sentyll.Infrastructure.HealthChecks.Azure.ServiceBus.Core.Constants;

internal static class SbConstants
{
    
    public static readonly TimeSpan DefaultIdleTime = TimeSpan.FromSeconds(2);

    public const string DefaultClient = "DEFAULT";
    
    public const string ManagementClient = "MANAGEMENT";

    public const string ReceiverClient = $"{DefaultClient}:RECIEVER";

    public const string NormalQueueType = "queue";

    public const string DeadLetterQueueType = "dead letter queue";

    public static string UnHealthyCountFailureMessage(
        string queueType, 
        string queueName, 
        int? unhealthyThreshold,
        long messagesCount)
        => $"Message in {queueType} {queueName} exceeded the amount of messages allowed for the unhealthy threshold {unhealthyThreshold}/{messagesCount}";

    public static string DegradedCountFailureMessage(
        string queueType, 
        string queueName, 
        int? degradedThreshold, 
        long messagesCount)
        => $"Message in {queueType} {queueName} exceeded the amount of messages allowed for the degraded threshold {degradedThreshold}/{messagesCount}";
    
}