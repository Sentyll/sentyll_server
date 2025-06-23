using System.Text.Json;
using System.Text.Json.Nodes;

namespace Sentyll.Domain.Common.Abstractions.Extensions.Serialization;

internal static class JsonNodeExtensions
{
    
    /// <summary>
    /// Converts a <see cref="object"/> to a <see cref="JsonNode"/>. Internally Type Checking and pattern matching will be performed
    /// To ensure the correct Json node is Constructed.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static JsonNode? ConvertToJsonNode(this object? value)
        => value switch
        {
            null => JsonValue.Create<object>(null),
            JsonElement el => el.ConvertToJsonNode(),
            JsonNode node => node.ConvertToJsonNode(),
            string s => s.ConvertToJsonNode(),
            _ => JsonValue.Create(value)
        };

    /// <summary>
    /// Converts a <see cref="JsonElement"/> to a <see cref="JsonNode"/> based on the <see cref="JsonValueKind"/> of the Json element.
    /// </summary>
    /// <param name="jsonElement"></param>
    /// <returns></returns>
    public static JsonNode? ConvertToJsonNode(this JsonElement jsonElement)
        => jsonElement switch
        {
            { ValueKind: JsonValueKind.Array } => JsonArray.Create(jsonElement)?.Root,
            { ValueKind: JsonValueKind.Object } => JsonObject.Create(jsonElement)?.Root,
            { ValueKind: JsonValueKind.Undefined or JsonValueKind.Null } => JsonValue.Create<object>(jsonElement)?.Root,
            _ => JsonValue.Create(jsonElement)?.Root
        };
    
    /// <summary>
    /// Converts a type that inherits from <see cref="JsonNode"/> to a base <see cref="JsonNode"/>
    /// </summary>
    /// <remarks>
    /// I think you can use .Root property to extract the internal JsonNode but this extension serves as a shortcut to be used inside <see cref="ConvertToJsonNode"/>
    /// </remarks>
    /// <param name="node"></param>
    /// <typeparam name="TJsonNode"></typeparam>
    /// <returns></returns>
    public static JsonNode? ConvertToJsonNode<TJsonNode>(this TJsonNode node) where TJsonNode : JsonNode
        => node switch
        {
            JsonArray jsonArray => jsonArray,
            JsonObject jsonObj => jsonObj,
            JsonValue jsonValue => jsonValue.Deserialize<JsonValue>(),
            _ => JsonValue.Create(node)
        };
    
    /// <summary>
    /// Converts a string value to a <see cref="JsonNode"/>
    /// </summary>
    /// <remarks>
    /// Additional string formatting checks are also performed to determine if it's a normal string or a serialized Array or Object.
    /// </remarks>
    /// <param name="content"></param>
    /// <returns></returns>
    public static JsonNode? ConvertToJsonNode(this string content)
    {
        if (content.StartsWith("{") && content.EndsWith("}"))
        {
            var stringyJsonDoc = JsonDocument.Parse(content);
            return JsonObject.Create(stringyJsonDoc.RootElement);
        }
        
        if (content.StartsWith("[") && content.EndsWith("]"))
        {
            var tempDocument = "{ \"a\" : " + content + "}";
            var stringyJsonDoc = JsonDocument.Parse(tempDocument);
            return JsonArray.Create(stringyJsonDoc.RootElement.GetProperty("a"));
        }
        
        //at this point we assume it's a normal text without special formatting
        return JsonValue.Create(content);
    }
}