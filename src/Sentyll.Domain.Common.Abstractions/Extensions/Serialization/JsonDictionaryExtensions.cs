using System.Text.Json.Nodes;

namespace Sentyll.Domain.Common.Abstractions.Extensions.Serialization;

internal static class JsonDictionaryExtensions
{
    
    /// <summary>
    /// Recursively builds a Flattened Dictionary for every json node provided in <see cref="json"/> string.
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>
    public static Dictionary<string, object?> ConvertToFlatDictionary(this string json)
    {
        var dict = new Dictionary<string, object?>();
        var token = JsonNode.Parse(json);
        var initialPrefix = ""; //Intentionally leaving as empty string to indicate it's going to start building keys from the root.
        ConvertToFlatDictionary(dict, token, initialPrefix);
        return dict;
    }

    /// <summary>
    /// a Recursive function that will flatten a <see cref="JsonNode"/> into an existing Dictionary.
    /// </summary>
    /// <remarks>
    /// The prefix key is used to append onto an existing "object" key, essentially inferring the current node is a
    /// continuation of a previous <see cref="JsonNode"/>
    /// </remarks>
    /// <param name="dict"></param>
    /// <param name="token"></param>
    /// <param name="prefix"></param>
    public static void ConvertToFlatDictionary(Dictionary<string, object?> dict, JsonNode? token, string prefix)
    {
        if (token is JsonObject jobj)
        {
            foreach (var item in token.AsObject())
            {
                ConvertToFlatDictionary(dict, item.Value, Join(prefix, item.Key));
            }
        }
        else if (token is JsonArray jarr)
        {
            int index = 0;
            foreach (var value in jarr)
            {
                ConvertToFlatDictionary(dict, value, Join(prefix, index.ToString()));
                index++;
            }
        }
        else if (token is JsonValue jval)
        {
            dict.Add(prefix, jval);
        }
    }

    /// <summary>
    /// Creates a new Prefix key by combining the previous prefix and the current Node name.
    /// </summary>
    /// <param name="prefix"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    private static string Join(string prefix, string name)
    {
        return (string.IsNullOrEmpty(prefix) ? name : prefix + "." + name);
    }
}