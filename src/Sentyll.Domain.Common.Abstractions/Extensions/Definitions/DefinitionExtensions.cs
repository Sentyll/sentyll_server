namespace Sentyll.Domain.Common.Abstractions.Extensions.Definitions;

public static class DefinitionExtensions
{
    public static Dictionary<string, string> ExtractKeyValueTags(this string[] keyValues)
    {
        var response = new Dictionary<string, string>();
        
        foreach (var tag in keyValues)
        {
            var headerDetails = tag.Split(":");
            response.Add(headerDetails[0], headerDetails[1]);
        }
    
        return response;
    }
}