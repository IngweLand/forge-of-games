using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;

namespace Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;

public interface IUnitUiService
{
    Task<HeroViewModel?> GetHeroAsync(string id);
    Task<HeroAbilityViewModel?> GetHeroAbilityAsync(string heroId);
    Task<IReadOnlyCollection<HeroBasicViewModel>> GetHeroListAsync();
}
