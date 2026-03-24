using System.Diagnostics;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Core.Repository.Abstractions;
using Ingweland.Fog.WebApp.Client.Services.Abstractions;

public class HohDataInitializationService(
    IHohDataProvider hohDataProvider,
    IHohDataService hohDataService,
    IHohLocalizationDataProvider hohLocalizationDataProvider,
    ILogger<HohDataInitializationService> logger) : IHohDataInitializationService
{
    public async Task InitializeAsync()
    {
        var version = await hohDataService.GetHohCoreDataVersionAsync();
        var sw = new Stopwatch();
        sw.Start();
        await ((IDataProvider) hohDataProvider).InitializeAsync(version.Version);
        sw.Stop();
        logger.LogDebug("Hoh core data loaded. Total elapsed: {elapsed}", sw.Elapsed);
        sw.Restart();
        await ((IDataProvider) hohLocalizationDataProvider).InitializeAsync(version.Version);
        sw.Stop();
        logger.LogDebug("Hoh localization data loaded. Total elapsed: {elapsed}", sw.Elapsed);
    }
}
