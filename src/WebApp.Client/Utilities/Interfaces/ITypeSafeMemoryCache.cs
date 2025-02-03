namespace Ingweland.Fog.WebApp.Client.Utilities;

public interface ITypeSafeMemoryCache
{
    void Set<T>(object key, T value);
    bool TryGetValue<T>(object key, out T? value);
    void Remove<T>(object key);
    void Clear();
}
