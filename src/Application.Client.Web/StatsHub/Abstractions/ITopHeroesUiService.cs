using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Fog.Enums;

namespace Ingweland.Fog.Application.Client.Web.StatsHub.Abstractions;

public interface ITopHeroesUiService
{
    Task<TopHeroesSearchFormViewModel> GetTopHeroesSearchFormDataAsync();

    Task<IReadOnlyCollection<HeroBasicViewModel>> GetTopHeroes(HeroInsightsMode mode, string? ageId,
        HeroLevelRange? levelRange, CancellationToken ct);
}
