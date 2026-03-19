using System.Diagnostics;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Core.Repository.Abstractions;
using Ingweland.Fog.WebApp.Client.Services.Abstractions;

namespace Ingweland.Fog.WebApp.Client.Services;

public class HohDataInitializationService(
    IHohDataProvider hohDataProvider,
    IHohLocalizationDataProvider hohLocalizationDataProvider,
    IFileCacheInteropService fileCacheInteropService,
    ILogger<HohDataInitializationService> logger) : IHohDataInitializationService
{
    public async Task InitializeAsync()
    {
        var version = await fileCacheInteropService.GetVersionAsync();
        var sw = new Stopwatch();
        sw.Start();
        await ((IDataProvider) hohDataProvider).InitializeAsync(version);
        sw.Stop();
        logger.LogDebug("Hoh data loaded: {elapsed}", sw.Elapsed);
        sw.Restart();
        await ((IDataProvider) hohLocalizationDataProvider).InitializeAsync(version);
        sw.Stop();
        logger.LogDebug("Hoh loca data loaded: {elapsed}", sw.Elapsed);
    }
}
