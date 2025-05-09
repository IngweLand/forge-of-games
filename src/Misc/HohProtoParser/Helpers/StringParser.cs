namespace HohProtoParser.Helpers;

public static class StringParser
{
    public static TEnum ParseEnumFromString<TEnum>(string value) where TEnum : struct, Enum
    {
        return ParseEnumFromString<TEnum>(value, '.');
    }

    public static TEnum ParseEnumFromString<TEnum>(string value, char delimiter) where TEnum : struct, Enum
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentException("Value is null or empty");
        }

        var enumValue = GetConcreteId(value, delimiter);

        if (!Enum.TryParse<TEnum>(enumValue, true, out var result))
        {
            throw new ArgumentException($"Cannot parse {enumValue} to enum {typeof(TEnum).Name}");
        }

        return result;
    }

    public static string GetConcreteId(string value)
    {
        return GetConcreteId(value, '.');
    }

    public static string GetConcreteId(string value, char delimiter)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentException("Value is null or empty");
        }

        var lastDotIndex = value.LastIndexOf(delimiter);
        return value[(lastDotIndex + 1)..];
    }
}
