using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;

namespace Ingweland.Fog.Application.Client.Web.Services.Hoh;

public class UnitUiService(
    IUnitService unitService,
    IMapper mapper,
    IHeroAbilityViewModelFactory abilityViewModelFactory) : IUnitUiService
{
    public async Task<IReadOnlyCollection<HeroBasicViewModel>> GetHeroListAsync()
    {
        return mapper.Map<IReadOnlyCollection<HeroBasicViewModel>>(await unitService.GetHeroesBasicDataAsync());
    }

    public async Task<HeroViewModel?> GetHeroAsync(string id)
    {
        var hero = await unitService.GetHeroAsync(id);
        return hero == null ? null : mapper.Map<HeroViewModel>(hero);
    }

    public async Task<HeroAbilityViewModel?> GetHeroAbilityAsync(string heroId)
    {
        try
        {
            var heroAbility = await unitService.GetHeroAbilityAsync(heroId);
            if (heroAbility == null)
            {
                return null;
            }

            var heroAbilityVm = abilityViewModelFactory.Create(heroAbility);
            return heroAbilityVm;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return null;
    }
}
