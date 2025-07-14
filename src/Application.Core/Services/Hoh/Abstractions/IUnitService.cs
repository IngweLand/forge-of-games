using Ingweland.Fog.Dtos.Hoh.Units;

namespace Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;

public interface IUnitService
{
    Task<HeroDto?> GetHeroAsync(string id);

    Task<IReadOnlyCollection<HeroBasicDto>> GetHeroesBasicDataAsync();

    Task<HeroAbilityDto?> GetHeroAbilityAsync(string heroId);
}
