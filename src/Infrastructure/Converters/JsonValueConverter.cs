using System.Text.Json;
using System.Text.Json.Serialization;
using Ingweland.Fog.Shared.Utils;
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
    private static readonly JsonSerializerOptions DefaultOptions = new()
    {
        WriteIndented = false,
        Converters = {new JsonStringEnumConverter()},
    };

    private static string Serialize(T value)
    {
        return JsonSerializer.Serialize(value, DefaultOptions);
    }

    private static T Deserialize(string value)
    {
        return JsonSerializer.Deserialize<T>(value, DefaultOptions)!;
    }
}

public class CompressedJsonValueConverter<T>()
    : ValueConverter<T, byte[]>(v => Serialize(v), v => Deserialize(v))
{
    private static byte[] Serialize(T value)
    {
        var json = JsonSerializer.Serialize(value);
        return CompressionUtils.CompressString(json);
    }

    private static T Deserialize(byte[] compressedBytes)
    {
        var json = CompressionUtils.DecompressToString(compressedBytes);
        if (string.IsNullOrWhiteSpace(json))
        {
            return default!;
        }
        return JsonSerializer.Deserialize<T>(json)!;
    }
}
