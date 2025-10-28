using Ingweland.Fog.Application.Client.Web.Caching.Interfaces;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Relic;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;

namespace Ingweland.Fog.Application.Client.Web.Services.Hoh;

public class RelicUiService(
    IRelicService relicService,
    IRelicInsightsViewModelFactory relicInsightsViewModelFactory,
    IHohCoreDataCache coreDataCache) : IRelicUiService
{
    public async Task<IReadOnlyCollection<RelicInsightsViewModel>> GetRelicInsights(string unitId, CancellationToken ct)
    {
        var relicData = await coreDataCache.GetRelicsAsync();
        var dtos = await relicService.GetInsightsAsync(unitId, ct);
        return dtos.OrderBy(x => x.FromLevel).Select(dto => relicInsightsViewModelFactory.Create(dto, relicData))
            .ToList();
    }
}
