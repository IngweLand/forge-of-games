using Ingweland.Fog.Application.Core.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Core.CityPlanner.Stats;
using Ingweland.Fog.Application.Core.CityPlanner.Stats.BuildingTypedStats;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Core.CityPlanner;

public class CityMapStateCore(IMapArea mapArea)
{
    private readonly Dictionary<int, CityMapEntity> _cityMapEntities = new();
    private readonly IList<CityMapEntity> _happinessConsumers = new List<CityMapEntity>();
    private readonly IList<CityMapEntity> _happinessProviders = new List<CityMapEntity>();

    private readonly IDictionary<BuildingType, IList<CityMapEntity>> _typedEntities =
        new Dictionary<BuildingType, IList<CityMapEntity>>();

    private AgeDto _cityAge;

    public required IReadOnlyDictionary<string, BuildingDto> Buildings { get; init; }

    public required AgeDto CityAge
    {
        get => _cityAge;
        init => _cityAge = value;
    }

    public IReadOnlyDictionary<int, CityMapEntity> CityMapEntities => _cityMapEntities.AsReadOnly();
    public required string CityName { get; set; }

    public WonderDto? CityWonder { get; init; }
    public int CityWonderLevel { get; set; }

    public IReadOnlyList<CityMapEntity> HappinessConsumers => _happinessConsumers.AsReadOnly();
    public IReadOnlyList<CityMapEntity> HappinessProviders => _happinessProviders.AsReadOnly();
    public IEnumerable<CityMapExpansion> OpenExpansions => mapArea.OpenExpansions;
    public int PremiumExpansionCount { get; init; }

    public IReadOnlyDictionary<BuildingType, IList<CityMapEntity>> TypedEntities => _typedEntities.AsReadOnly();

    public void Add(CityMapEntity cityMapEntity)
    {
        if (Buildings.TryGetValue(cityMapEntity.CityEntityId, out var building))
        {
            if (building.Group == BuildingGroup.CityHall)
            {
                _cityAge = building.Age!;
            }

            _cityMapEntities.Add(cityMapEntity.Id, cityMapEntity);
            if (cityMapEntity.HasStat<HappinessProvider>())
            {
                _happinessProviders.Add(cityMapEntity);
            }
            else if (cityMapEntity.HasStat<HappinessConsumer>())
            {
                _happinessConsumers.Add(cityMapEntity);
            }

            if (!_typedEntities.TryGetValue(building.Type, out var typeList))
            {
                typeList = new List<CityMapEntity>();
                _typedEntities.Add(building.Type, typeList);
            }

            typeList.Add(cityMapEntity);
        }
        // log
    }

    public void AddRange(IEnumerable<CityMapEntity> cityMapEntities)
    {
        foreach (var entity in cityMapEntities)
        {
            Add(entity);
        }
    }

    public virtual void Remove(int cityMapEntityId)
    {
        if (!_cityMapEntities.Remove(cityMapEntityId, out var foundEntity))
        {
            return;
        }

        _happinessProviders.Remove(foundEntity);
        _happinessConsumers.Remove(foundEntity);
        var building = Buildings[foundEntity.CityEntityId];
        _typedEntities[building.Type].Remove(foundEntity);

        OnAfterRemove(foundEntity);
    }

    protected virtual void OnAfterRemove(CityMapEntity removedEntity)
    {
    }
}
