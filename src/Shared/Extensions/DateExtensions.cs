namespace Ingweland.Fog.Shared.Extensions;

public static class DateExtensions
{
    public static DateOnly ToDateOnly(this DateTime src)
    {
        return DateOnly.FromDateTime(src);
    }
}