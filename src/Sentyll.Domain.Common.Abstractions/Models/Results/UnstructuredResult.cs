using System.Text.Json;
using System.Text.Json.Nodes;
using Sentyll.Domain.Common.Abstractions.Contracts.Models.Results;
using Sentyll.Domain.Common.Abstractions.Extensions.Serialization;
using Sentyll.Domain.Common.Abstractions.Failures;

namespace Sentyll.Domain.Common.Abstractions.Models.Results;

public class UnstructuredResult : Dictionary<string, object?>, IUnstructuredResult
{
    
    private static readonly JsonSerializerOptions _defaultJsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
    };
    
    public UnstructuredResult() : base(StringComparer.OrdinalIgnoreCase)
    {
    }
    
    #region CREATE_FROM

    public static IUnstructuredResult FromType<T>(T source) where T : new()
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }
        
        // Serialize the object to JSON then parse it into a dictionary
        var flatDict = Serialize<T>(source).ConvertToFlatDictionary();
        return FromDictionary(flatDict);
    }
    
    public static IUnstructuredResult FromDictionary(IDictionary<string, object?> source)
    {
        var result = new UnstructuredResult();
        
        foreach (var kvp in source)
        {
            result.Add(kvp.Key, kvp.Value);
        }

        return result;
    }

    #endregion

    #region MUTATIONS

    public new void Add(string key, string value)
    {
        JsonDictionaryExtensions.ConvertToFlatDictionary(this, value.ConvertToJsonNode(), key);
    }
    
    public new void Add<T>(string key, T value) where T : new()
    {
        var json = Serialize(value);
        var token = JsonNode.Parse(json);
        JsonDictionaryExtensions.ConvertToFlatDictionary(this, token, key);
    }
    
    public void Compress()
    {
        var flatDict = ToString().ConvertToFlatDictionary();
        
        Clear();
        
        foreach (var kvp in flatDict)
        {
            Add(kvp.Key, kvp.Value);
        }
    }

    #endregion
    
    #region SERIALIZATION

    private static string Serialize<T>(T content) where T : new() 
        => JsonSerializer.Serialize(content, _defaultJsonSerializerOptions);

    private static T? Deserialize<T>(string json) where T : new() 
        => JsonSerializer.Deserialize<T>(json, _defaultJsonSerializerOptions);

    public override string ToString() 
        => this.ConvertToJsonObject().ToJsonString();

    #endregion

    #region BINDING

    public Result<T> Bind<T>() where T : new()
    {
        var json = ToString();
        if (string.IsNullOrWhiteSpace(json))
        {
            return Result.Failure<T>(UnstructuredResultFailures.EmptySerializationResult);
        }
        
        var deserializedResult = Deserialize<T>(json);
        if (deserializedResult is null)
        {
            return Result.Failure<T>(UnstructuredResultFailures.DeserializedToNull);
        }

        return deserializedResult;
    }
    
    public bool TryBind<T>(out T? result) where T : new()
    {
        var bindResult = Bind<T>();
        result = bindResult.IsSuccess ? bindResult.Value : default;
        return bindResult.IsSuccess;
    }

    public List<KeyValuePair<string, object?>> ToKeyValuePair()
        => this.ToList();

    #endregion
    
}