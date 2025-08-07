using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Caching.Interfaces;

public interface IHohCoreDataCache
{
    Task<IReadOnlyCollection<BuildingDto>> GetBarracks(UnitType unitType);
    Task<HeroDto?> GetHeroAsync(string heroId);
    Task<List<HeroDto>> GetHeroes(HashSet<string> heroIds);
    Task<IReadOnlyDictionary<string, RelicDto>> GetRelicsAsync();
    Task AddBarracksAsync(IEnumerable<BuildingDto> barracks);
    Task AddHeroesAsync(IEnumerable<HeroDto> heroes);
    IReadOnlyDictionary<string, HeroDto> GetAllHeroes();

    IReadOnlyDictionary<UnitType, IReadOnlyCollection<BuildingDto>> GetAllBarracks();
    Task<IReadOnlyDictionary<(string unitId, int unitLevel), BuildingDto>> GetBarracksByUnitMapAsync();
}
