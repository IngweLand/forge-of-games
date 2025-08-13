using System.Text.Json;
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
