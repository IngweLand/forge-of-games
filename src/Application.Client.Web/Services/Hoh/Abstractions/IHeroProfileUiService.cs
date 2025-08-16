using Ingweland.Fog.Application.Client.Web.CommandCenter.Models;
using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;

public interface IHeroProfileUiService
{
    Task<IReadOnlyCollection<HeroBasicViewModel>> GetHeroes(HeroFilterRequest request);
    Task<IReadOnlyCollection<HeroBasicViewModel>> GetHeroes(string searchString);
    Task<IReadOnlyCollection<HeroBasicViewModel>> GetHeroes();
    Task<HeroProfileIdentifier?> GetHeroProfileIdentifierAsync(string heroId);
    Task<HeroProfileViewModel?> GetHeroProfileAsync(HeroProfileIdentifier identifier);

    Task<IReadOnlyCollection<IconLabelItemViewModel>> CalculateHeroProgressionCost(
        HeroProgressionCostRequest request);
    
    Task<IconLabelItemViewModel> CalculateAbilityCostAsync(AbilityCostRequest request);

    void SaveHeroProfile(HeroProfileIdentifier identifier);
    Task<HeroDto?> GetHeroAsync(string heroId);
}
