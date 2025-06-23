using System.Net;

namespace Sentyll.Infrastructure.Events.WebHooks.Abstractions.Models.Response;

public sealed record WebhookSendResponse(
    bool IsSuccess,
    HttpStatusCode StatusCode,
    string Content
)
{

    public List<(string, string)> ResponseHeaders { get; private set; } = new();

    public void AppendResponseHeaders(IEnumerable<KeyValuePair<string,IEnumerable<string>>> responseHeaders)
    {
        foreach (var responseHeaderCollection in responseHeaders)
        {
            foreach (var collectionHeader in responseHeaderCollection.Value)
            {
                ResponseHeaders.Add((responseHeaderCollection.Key, collectionHeader));
            }
        }
    }
    
}