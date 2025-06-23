using Sentyll.Infrastructure.HealthChecks.Uris.Core.Constants;

namespace Sentyll.Infrastructure.HealthChecks.Uris.Core.Models.Options;

internal sealed class UriOptions
{
    
    public HttpMethod HttpMethod { get; private set; }

    public TimeSpan Timeout { get; private set; }

    public (int Min, int Max) ExpectedHttpCodes { get; private set; }

    public string? ExpectedContent { get; private set; }

    public Uri Uri { get; }

    public readonly List<(string Name, string Value)> Headers = new();

    public UriOptions(Uri uri)
    {
        Uri = uri;
        
        ExpectedHttpCodes = (UriConstants.MinSuccessStatusCode, UriConstants.MaxSuccessStatusCode);
        Timeout = UriConstants.DefaultTimeout;
        HttpMethod = HttpMethod.Get;
    }

    public UriOptions AddCustomHeader(string name, string value)
    {
        Headers.Add((name, value));
        return this;
    }

    UriOptions UseGet()
    {
        HttpMethod = HttpMethod.Get;
        return this;
    }

    UriOptions UsePost()
    {
        HttpMethod = HttpMethod.Post;
        return this;
    }

    UriOptions ExpectHttpCode(int codeToExpect)
    {
        ExpectedHttpCodes = (codeToExpect, codeToExpect);
        return this;
    }

    UriOptions ExpectHttpCodes(int minCodeToExpect, int maxCodeToExpect)
    {
        ExpectedHttpCodes = (minCodeToExpect, maxCodeToExpect);
        return this;
    }

    UriOptions UseHttpMethod(HttpMethod methodToUse)
    {
        HttpMethod = methodToUse;
        return this;
    }

    UriOptions UseTimeout(TimeSpan timeout)
    {
        Timeout = timeout;
        return this;
    }

    UriOptions ExpectContent(string expectedContent)
    {
        ExpectedContent = expectedContent;
        return this;
    }
}