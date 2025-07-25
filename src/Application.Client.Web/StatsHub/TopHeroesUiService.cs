using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.StatsHub;

public class TopHeroesUiService(
    ICommonUiService commonUiService,
    IHeroProfileUiService heroProfileUiService,
    IStatsHubService statsHubService)
    : ITopHeroesUiService
{
    private readonly IReadOnlyCollection<HeroLevelRange> _levelRanges =
        [new(null, 59), new(60, 79), new(80, 99), new(100, 119), new(120, null)];

    public async Task<TopHeroesSearchFormViewModel> GetTopHeroesSearchFormDataAsync()
    {
        return new TopHeroesSearchFormViewModel
        {
            Ages = (await commonUiService.GetAgesAsync()).Values.Where(x => x.Index is > 2 and < 14).ToList(),
            LevelRanges = _levelRanges.OrderBy(x => x.From).ToList(),
        };
    }

    public async Task<IReadOnlyCollection<HeroBasicViewModel>> GetTopHeroes(string? ageId, HeroLevelRange? levelRange,
        CancellationToken ct)
    {
        var topHeroes = await statsHubService.GetTopHeroesAsync(ageId, levelRange?.From, levelRange?.To, ct);
        var heroes = (await heroProfileUiService.GetHeroes()).ToDictionary(h => h.UnitId);
        return topHeroes.Select(x => heroes[x]).ToList();
    }
}
