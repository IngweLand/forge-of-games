using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.StatsHub.Abstractions;

public interface ITopHeroesUiService
{
    Task<TopHeroesSearchFormViewModel> GetTopHeroesSearchFormDataAsync();

    Task<IReadOnlyCollection<HeroBasicViewModel>> GetTopHeroes(string? ageId, HeroLevelRange? levelRange,
        CancellationToken ct);
}
