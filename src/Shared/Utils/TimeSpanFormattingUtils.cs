namespace Ingweland.Fog.Shared.Utils;

public static class TimeSpanUtils
{
    /// <summary>
    ///     Converts a TimeSpan to a string like "2d 4h 15m" or "5h 10m".
    /// </summary>
    public static string ToShortReadableString(this TimeSpan span)
    {
        var parts = new List<string>();

        if (span.Days > 0)
        {
            parts.Add($"{span.Days}d");
        }

        if (span.Hours > 0)
        {
            parts.Add($"{span.Hours}h");
        }

        if (span.Minutes > 0 || parts.Count == 0)
        {
            parts.Add($"{span.Minutes}m");
        }

        return string.Join(" ", parts);
    }
}
