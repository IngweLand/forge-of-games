using Ingweland.Fog.Infrastructure.Repositories.Abstractions;
using Ingweland.Fog.WebApp.Startup.Interfaces;

namespace Ingweland.Fog.WebApp.Startup;

public class PreloadDataStartupTask(
    IHohDataProvider hohDataProvider,
    IHohLocalizationDataProvider hohLocalizationDataProvider) : IStartupTask
{
    public async Task ExecuteAsync(CancellationToken ct)
    {
        _ = await hohDataProvider.GetDataAsync();
        _ = hohLocalizationDataProvider.GetData();
    }
}
