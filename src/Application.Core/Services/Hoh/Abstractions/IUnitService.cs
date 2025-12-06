using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Dtos.Hoh.Units;
using Refit;

namespace Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;

public interface IUnitService
{
    [Get(FogUrlBuilder.ApiRoutes.HERO_TEMPLATE)]
    Task<HeroDto?> GetHeroAsync(string id);

    [Get(FogUrlBuilder.ApiRoutes.HEROES_BASICS)]
    Task<IReadOnlyCollection<HeroBasicDto>> GetHeroesBasicDataAsync();
}
