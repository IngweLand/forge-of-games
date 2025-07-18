using Ingweland.Fog.Application.Client.Web.Caching.Interfaces;
using Ingweland.Fog.Application.Core.Extensions;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Caching;

namespace Ingweland.Fog.Application.Client.Web.Caching;

public class HohCoreDataCache : IHohCoreDataCache
{
    private readonly AsyncCache<UnitType, IReadOnlyCollection<BuildingDto>> _barracksCache;
    private readonly AsyncCache<string, HeroDto> _heroesCache;

    public HohCoreDataCache(ICityService cityService, IUnitService unitService)
    {
        _barracksCache = new AsyncCache<UnitType, IReadOnlyCollection<BuildingDto>>(async x =>
        {
            var result = await cityService.GetBarracks(x);
            return result;
        });
        _heroesCache = new AsyncCache<string, HeroDto>(unitService.GetHeroAsync);
    }

    public async Task<IReadOnlyCollection<BuildingDto>> GetBarracks(UnitType unitType)
    {
        var result = await _barracksCache.GetAsync(unitType);
        return result ?? Array.Empty<BuildingDto>();
    }

    public Task<HeroDto?> GetHeroAsync(string heroId)
    {
        return _heroesCache.GetAsync(heroId);
    }
    
    public IReadOnlyDictionary<string, HeroDto> GetAllHeroes()
    {
        return _heroesCache.GetAll();
    }
    
    public IReadOnlyDictionary<UnitType, IReadOnlyCollection<BuildingDto>> GetAllBarracks()
    {
        return _barracksCache.GetAll();
    }

    public async Task AddHeroesAsync(IEnumerable<HeroDto> heroes)
    {
        foreach (var heroDto in heroes)
        {
            await _heroesCache.AddAsync(heroDto.Id, heroDto);
        }
    }

    public async Task AddBarracksAsync(IEnumerable<BuildingDto> barracks)
    {
        var unitTypes = new HashSet<UnitType>
            {UnitType.Cavalry, UnitType.HeavyInfantry, UnitType.Infantry, UnitType.Ranged, UnitType.Siege};
        var barracksList = barracks.ToList();
        foreach (var unitType in unitTypes)
        {
            var buildingGroup = unitType.ToBuildingGroup();
            var buildings = barracksList.Where(x => x.Group == buildingGroup).ToList();
            if (buildings.Count == 0)
            {
                continue;
            }

            await _barracksCache.AddAsync(unitType, buildings.ToList());
        }
    }
}
