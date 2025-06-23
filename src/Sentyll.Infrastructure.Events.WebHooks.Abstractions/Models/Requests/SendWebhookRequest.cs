namespace Sentyll.Infrastructure.Events.WebHooks.Abstractions.Models.Requests;

public sealed record SendWebhookRequest(
    HttpMethod HttpMethod,
    Uri Uri,
    string Payload
)
{

    public TimeSpan Timeout { get; private set; } = TimeSpan.FromSeconds(30);
    
    public List<(string Name, string Value)> Headers { get; } = new();
    
    public void AddCustomHeader(string name, string value)
    {
        Headers.Add((name, value));
    }

    public void SetTimeout(TimeSpan timeout)
    {
        Timeout = timeout;
    }

}