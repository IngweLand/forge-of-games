using Ingweland.Fog.Application.Client.Web.Localization;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Fog.Enums;
using Ingweland.Fog.Models.Hoh.Constants;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Client.Web.StatsHub;

public class TopHeroesUiService(
    ICommonUiService commonUiService,
    IHeroProfileUiService heroProfileUiService,
    IStatsHubService statsHubService,
    IStringLocalizer<FogResource> localizer,
    ILogger<TopHeroesUiService> logger)
    : UiServiceBase(logger), ITopHeroesUiService
{
    private readonly IReadOnlyCollection<HeroLevelRange> _levelRanges =
        [new(null, 59), new(60, 79), new(80, 99), new(100, 119), new(120, null)];

    public async Task<TopHeroesSearchFormViewModel> GetTopHeroesSearchFormDataAsync()
    {
        var ages = await commonUiService.GetAgesAsync();
        var comingSoonAgeIndex = 15;
        if (ages.TryGetValue(AgeIds.COMING_SOON, out var comingSoonAge))
        {
            comingSoonAgeIndex = comingSoonAge.Index;
        }

        return new TopHeroesSearchFormViewModel
        {
            Ages = ages.Values.Where(x => x.Index > 2 && x.Index < comingSoonAgeIndex).ToList(),
            LevelRanges = _levelRanges.OrderBy(x => x.From).ToList(),
            Modes =
            [
                new HeroInsightsModeViewModel
                {
                    Mode = HeroInsightsMode.MostPopular,
                    Name = localizer[FogResource.StatsHub_HeroInsights_Mode_MostPopular],
                },
                new HeroInsightsModeViewModel
                {
                    Mode = HeroInsightsMode.Top,
                    Name = localizer[FogResource.StatsHub_HeroInsights_Mode_Top],
                },
            ],
        };
    }

    public async Task<IReadOnlyCollection<HeroBasicViewModel>> GetTopHeroes(HeroInsightsMode mode, string? ageId,
        HeroLevelRange? levelRange, CancellationToken ct)
    {
        var topHeroes = await statsHubService.GetTopHeroesAsync(mode, ageId, levelRange?.From, levelRange?.To, ct);
        var heroes = (await heroProfileUiService.GetHeroes()).ToDictionary(h => h.UnitId);
        return topHeroes.Select(x => heroes[x]).ToList();
    }

    public async Task<IReadOnlyCollection<HeroBasicViewModel>> GetTopHeroes(CancellationToken ct)
    {
        var topHeroes = await statsHubService.GetTopHeroesAsync(HeroInsightsMode.MostPopular, ct: ct);
        var heroes = (await heroProfileUiService.GetHeroes()).ToDictionary(h => h.UnitId);
        return topHeroes.Take(6).Select(x => heroes[x]).ToList();
    }
}
