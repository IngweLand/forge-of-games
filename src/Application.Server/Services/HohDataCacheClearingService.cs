using Ingweland.Fog.Application.Core.Interfaces;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Application.Server.Services.Interfaces;
using Ingweland.Fog.Shared.Localization;
using LazyCache;

namespace Ingweland.Fog.Application.Server.Services;

public class HohDataCacheClearingService(
    IAppCache appCache,
    ICacheKeyFactory cacheKeyFactory,
    IHohDataCache hohDataCache) : IHohDataCacheClearingService
{
    public void Clear()
    {
        appCache.Remove(cacheKeyFactory.HohData);
        foreach (var culture in HohSupportedCultures.AllCultures)
        {
            appCache.Remove(cacheKeyFactory.HohLocalizationData(culture));
            appCache.Remove(cacheKeyFactory.HeroAbilityFeatures(culture));
        }

        hohDataCache.Clear();
    }
}
