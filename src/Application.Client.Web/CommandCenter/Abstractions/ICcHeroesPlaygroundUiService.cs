using Ingweland.Fog.Application.Client.Web.CommandCenter.Models;

namespace Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;

public interface ICcHeroesPlaygroundUiService
{
    event Action? StateHasChanged;
    Task<IReadOnlyCollection<HeroProfileViewModel>> GetHeroes();
    Task<HeroProfileViewModel?> GetHeroProfileAsync(string heroProfileId);
    HeroProfileViewModel? UpdateHeroProfile(HeroProfileStatsUpdateRequest request);
}
