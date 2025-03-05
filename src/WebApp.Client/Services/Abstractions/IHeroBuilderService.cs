using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.WebApp.Client.Services.Abstractions;

public interface IHeroBuilderService
{
    Task<HeroBuilderViewModel?> GetFormData(string heroId);

    CustomHeroViewModel CreateCustomProfile(HeroBuilderViewModel data, HeroLevelSpecs level,
        int abilityLevel, int awakeningLevel, int barracksLevel);
}
