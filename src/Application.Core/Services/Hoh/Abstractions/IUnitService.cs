using Ingweland.Fog.Dtos.Hoh.Units;
using Refit;

namespace Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;

public interface IUnitService
{
    [Get("/heroes/{id}")]
    Task<HeroDto?> GetHeroAsync(string id);

    [Get("/heroes/basic")]
    Task<IReadOnlyCollection<HeroBasicDto>> GetHeroesBasicDataAsync();

    [Get("/heroes/{heroId}/ability")]
    Task<BattleAbilityDto?> GetHeroAbilityAsync(string heroId);
}
