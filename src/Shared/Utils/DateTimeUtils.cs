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
}
