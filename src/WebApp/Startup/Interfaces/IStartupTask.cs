namespace Ingweland.Fog.WebApp.Startup.Interfaces;

public interface IStartupTask
{
    Task ExecuteAsync(CancellationToken ct);
}
