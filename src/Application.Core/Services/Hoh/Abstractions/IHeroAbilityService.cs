using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Dtos.Hoh.Units;
using Refit;

namespace Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;

public interface IHeroAbilityService
{
    [Get(FogUrlBuilder.ApiRoutes.HERO_ABILITY_FEATURES)]
    Task<IReadOnlyCollection<HeroAbilityFeaturesDto>> GetHeroAbilityFeaturesAsync();
}
