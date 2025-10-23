using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Ingweland.Fog.Infrastructure.Converters;

public class JsonValueConverter<T>()
    : ValueConverter<T, string>(v => Serialize(v), v => Deserialize(v))
{
    private static string Serialize(T value)
    {
        return JsonSerializer.Serialize(value);
    }

    private static T Deserialize(string value)
    {
        return JsonSerializer.Deserialize<T>(value)!;
    }
}

public class JsonValueWithStringEnumConverter<T>()
    : ValueConverter<T, string>(v => Serialize(v), v => Deserialize(v))
{
    private static string Serialize(T value)
    {
        return JsonSerializer.Serialize(value, DefaultOptions);
    }

    private static T Deserialize(string value)
    {
        return JsonSerializer.Deserialize<T>(value, DefaultOptions)!;
    }
    
    private static readonly JsonSerializerOptions DefaultOptions = new()
    {
        WriteIndented = false,
        Converters = { new JsonStringEnumConverter() }
    };
}
