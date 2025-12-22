using NodaTime;

namespace Ingweland.Fog.Shared.Utils;

public static class DateTimeUtils
{
    public static ZonedDateTime GetCurrentTime(string ianaTimeZoneId)
    {
        var timeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(ianaTimeZoneId) ?? DateTimeZoneProviders.Tzdb["UTC"];
        // TODO log warning
        var now = SystemClock.Instance.GetCurrentInstant();
        return now.InZone(timeZone);
    }

    public static ZonedDateTime GetTime(DateTime dateTime, string ianaTimeZoneId)
    {
        var utcInstant = Instant.FromDateTimeUtc(dateTime);
        var timeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(ianaTimeZoneId) ?? DateTimeZoneProviders.Tzdb["UTC"];
        return utcInstant.InZone(timeZone);
    }

    public static DateTime GetNextMidnightUtc()
    {
        return DateTime.UtcNow.Date.AddDays(1);
    }

    public static DateTime GetNextNoonUtc()
    {
        var now = DateTime.UtcNow;
        var todayNoon = now.Date.AddHours(12);

        return now < todayNoon
            ? todayNoon
            : todayNoon.AddDays(1);
    }

    public static DateTime? StripToHour(this DateTime? dt)
    {
        if (dt == null)
        {
            return null;
        }

        return new DateTime(dt.Value.Year, dt.Value.Month, dt.Value.Day, dt.Value.Hour, 0, 0, dt.Value.Kind);
    }
}
