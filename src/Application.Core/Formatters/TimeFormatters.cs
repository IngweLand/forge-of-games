using Ingweland.Fog.Application.Core.Formatters.Interfaces;

namespace Ingweland.Fog.Application.Core.Formatters;

public class TimeFormatters : ITimeFormatters
{
    public string FromSeconds(int seconds)
    {
        var duration = TimeSpan.FromSeconds(seconds);
        return duration.ToString("c");
    }
}
