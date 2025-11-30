using Ingweland.Fog.WebApp.Startup.Interfaces;

namespace Ingweland.Fog.WebApp.Startup;

public class StartupTaskHostedService(IEnumerable<IStartupTask> startupTasks) : IHostedService
{
    public async Task StartAsync(CancellationToken ct)
    {
        foreach (var task in startupTasks)
        {
            await task.ExecuteAsync(ct);
        }
    }

    public Task StopAsync(CancellationToken _)
    {
        return Task.CompletedTask;
    }
}
