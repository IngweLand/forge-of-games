using Ingweland.Fog.Application.Client.Web.Caching.Interfaces;
using Ingweland.Fog.Application.Core.Extensions;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.Equipment;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Entities.Equipment;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Caching;
using Ingweland.Fog.Shared.Helpers;

namespace Ingweland.Fog.Application.Client.Web.Caching;

public class HohCoreDataCache : IHohCoreDataCache
{
    private readonly Lazy<Task<IReadOnlyDictionary<(string unitId, int unitLevel), BuildingDto>>> _barracks;
    private readonly AsyncCache<UnitType, IReadOnlyCollection<BuildingDto>> _barracksCache;
    private readonly ICityService _cityService;
    private readonly Lazy<Task<EquipmentDataDto>> _equipmentData;
    private readonly IEquipmentService _equipmentService;
    private readonly AsyncCache<string, HeroDto> _heroesCache;
    private readonly IRelicCoreDataService _relicCoreDataService;
    private IReadOnlyDictionary<string, RelicDto>? _relicsCache;

    public HohCoreDataCache(ICityService cityService, IUnitService unitService, IRelicCoreDataService relicCoreDataService,
        IEquipmentService equipmentService)
    {
        _cityService = cityService;
        _relicCoreDataService = relicCoreDataService;
        _equipmentService = equipmentService;
        _barracksCache = new AsyncCache<UnitType, IReadOnlyCollection<BuildingDto>>(async x =>
        {
            var result = await cityService.GetBarracks(x);
            return result;
        });
        _heroesCache = new AsyncCache<string, HeroDto>(unitService.GetHeroAsync);
        _barracks =
            new Lazy<Task<IReadOnlyDictionary<(string unitId, int unitLevel), BuildingDto>>>(
                BuildBarracksByUnitMapAsync);
        _equipmentData = new Lazy<Task<EquipmentDataDto>>(FetchEquipmentDataAsync);
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

    public async Task<List<HeroDto>> GetHeroes(HashSet<string> heroIds)
    {
        var heroTasks = heroIds.Select(GetHeroAsync);
        return (await Task.WhenAll(heroTasks)).Where(x => x != null).ToList()!;
    }

    public async Task<IReadOnlyDictionary<string, RelicDto>> GetRelicsAsync()
    {
        if (_relicsCache == null)
        {
            var relics = await _relicCoreDataService.GetRelicsAsync();
            _relicsCache = relics.ToDictionary(x => HohStringParser.GetConcreteId(x.Id));
        }

        return _relicsCache;
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

    public Task<IReadOnlyDictionary<(string unitId, int unitLevel), BuildingDto>> GetBarracksByUnitMapAsync()
    {
        return _barracks.Value;
    }

    public Task<EquipmentDataDto> GetEquipmentDataAsync()
    {
        return _equipmentData.Value;
    }

    private async Task<EquipmentDataDto> FetchEquipmentDataAsync()
    {
        return await _equipmentService.GetEquipmentData();
    }

    private async Task<IReadOnlyDictionary<(string unitId, int unitLevel), BuildingDto>> BuildBarracksByUnitMapAsync()
    {
        var barracks = await _cityService.GetAllBarracks();
        var result = new Dictionary<(string unitId, int unitLevel), BuildingDto>();
        foreach (var b in barracks)
        {
            var component = b.Components.OfType<BuildingUnitProviderComponent>().FirstOrDefault();
            if (component == null)
            {
                continue;
            }

            result.Add((component.BuildingUnit.Unit.Id, component.BuildingUnit.Level), b);
        }

        return result;
    }
}
