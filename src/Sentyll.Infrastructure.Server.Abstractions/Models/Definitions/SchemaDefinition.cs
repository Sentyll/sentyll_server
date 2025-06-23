using System.Text.Json.Serialization;

namespace Sentyll.Infrastructure.Server.Abstractions.Models.Definitions;

public sealed class SchemaDefinition
{
    [JsonPropertyName("schema")] 
    public string Schema { get; set; }
    
    [JsonPropertyName("version")] 
    public int Version { get; set; }

    [JsonPropertyName("meta")] 
    public UnstructuredResult Meta { get; set; } = new();
    
    [JsonPropertyName("restContent")] 
    public UnstructuredResult RestContent { get; set; } = new();
    
    [JsonPropertyName("restContentMeta")]
    public UnstructuredResult RestContentMeta { get; set; } = new();

}