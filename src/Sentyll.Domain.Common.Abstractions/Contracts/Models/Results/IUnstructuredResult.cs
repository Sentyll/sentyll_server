namespace Sentyll.Domain.Common.Abstractions.Contracts.Models.Results;

public interface IUnstructuredResult
{

    void Add(string key, string value);

    void Add<T>(string key, T value) where T : new();
    
    void Compress();
    
    bool Remove(string key, out object? value);
    
    bool ContainsKey(string key);
    
    bool ContainsValue(object? value);
    
    bool TryGetValue(string key, out object? value);
    
    void Clear();
    
    Result<T> Bind<T>() where T : new();

    bool TryBind<T>(out T? result) where T : new();

    List<KeyValuePair<string, object?>> ToKeyValuePair();

}