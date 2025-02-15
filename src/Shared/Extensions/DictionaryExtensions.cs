namespace Ingweland.Fog.Shared.Extensions;

public static class DictionaryExtensions
{
    public static T GetRequiredItem<T>(this IDictionary<string, object> src, string key)
    {
        if (!src.TryGetValue(key, out var value) || value is not T typedValue)
        {
            throw new ArgumentException($"The mandatory {key} item of type {typeof(T).FullName} was not found.");
        }

        return typedValue;
    }
}
