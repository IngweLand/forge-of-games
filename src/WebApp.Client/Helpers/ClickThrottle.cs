namespace Ingweland.Fog.WebApp.Client.Helpers;

public class ClickThrottle(TimeSpan? interval = null)
{
    private DateTime _lastClick = DateTime.MinValue;
    private readonly TimeSpan _interval = interval ?? TimeSpan.FromMilliseconds(600);

    public async Task Run(Func<Task> action)
    {
        var now = DateTime.UtcNow;
        if (now - _lastClick < _interval) return;
        _lastClick = now;
        await action();
    }
}
