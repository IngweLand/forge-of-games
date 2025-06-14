using Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Stats.BuildingTypedStats;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner;

public class CityMapState(
    IBuildingLevelRangesFactory buildingLevelRangesFactory)
{
    private readonly Dictionary<int, CityMapEntity> _cityMapEntities = new ();
    private readonly IList<CityMapEntity> _happinessConsumers = new List<CityMapEntity>();
    private readonly IList<CityMapEntity> _happinessProviders = new List<CityMapEntity>();
    private readonly IList<HohCitySnapshot> _snapshots = new List<HohCitySnapshot>();

    private readonly IDictionary<BuildingType, IList<CityMapEntity>> _typedEntities =
        new Dictionary<BuildingType, IList<CityMapEntity>>();

    private IReadOnlyDictionary<BuildingGroup, BuildingLevelRange>? _buildingLevelRanges;

    private AgeDto _cityAge;
    private CityPlannerCityPropertiesViewModel? _cityPropertiesViewModel;

    private CityStats _cityStats = new();
    private CityMapEntity? _selectedCityMapEntity;
    private CityMapEntityViewModel? _selectedEntityViewModel;
    private CityMapBuildingGroupViewModel? _selectedCityMapBuildingGroupViewModel;
    private IReadOnlyList<CityMapEntity>? _selectedCityMapEntities;
    public required IReadOnlyCollection<BuildingCustomizationDto> BuildingCustomizations { get; init; }

    public IReadOnlyDictionary<BuildingGroup, BuildingLevelRange>? BuildingLevelRanges
    {
        get
        {
            if (_buildingLevelRanges == null)
            {
                _buildingLevelRanges = buildingLevelRangesFactory.Create(Buildings.Values.ToList());
            }

            return _buildingLevelRanges;
        }
        private set { _buildingLevelRanges = value; }
    }

    public required IReadOnlyDictionary<string, BuildingDto> Buildings { get; init; }
    public required IReadOnlyCollection<BuildingSelectorTypesViewModel> BuildingSelectorItems { get; init; }

    public required AgeDto CityAge
    {
        get => _cityAge;
        init => _cityAge = value;
    }

    public required string CityId { get; init; }

    public IReadOnlyDictionary<int, CityMapEntity> CityMapEntities => _cityMapEntities.AsReadOnly();
    public required string CityName { get; set; }

    public CityPlannerCityPropertiesViewModel? CityPropertiesViewModel
    {
        get => _cityPropertiesViewModel;
        set
        {
            if (_cityPropertiesViewModel == value)
            {
                return;
            }

            _cityPropertiesViewModel = value;
            StateChanged?.Invoke();
        }
    }

    public CityStats CityStats
    {
        get => _cityStats;
        set
        {
            if (_cityStats == value)
            {
                return;
            }

            _cityStats = value;
            StateChanged?.Invoke();
        }
    }

    public WonderDto? CityWonder { get; init; }
    public int CityWonderLevel { get; set; }

    public IReadOnlyList<CityMapEntity> HappinessConsumers => _happinessConsumers.AsReadOnly();
    public IReadOnlyList<CityMapEntity> HappinessProviders => _happinessProviders.AsReadOnly();
    public required CityId InGameCityId { get; init; }

    public IReadOnlyList<CityMapEntity>? SelectedCityMapEntities
    {
        get => _selectedCityMapEntities;
        set
        {
            if (_selectedCityMapEntities == value)
            {
                return;
            }

            _selectedCityMapEntities = value;
            if (_selectedCityMapEntities != null)
            {
                foreach (var cityMapEntity in _selectedCityMapEntities)
                {
                    cityMapEntity.IsSelected = true;
                }
            }
            else
            {
                _selectedCityMapBuildingGroupViewModel = null;
            }

            StateChanged?.Invoke();
        }
    }

    public CityMapEntity? SelectedCityMapEntity
    {
        get => _selectedCityMapEntity;
        set
        {
            if (_selectedCityMapEntity == value)
            {
                return;
            }

            _selectedCityMapEntity = value;
            if (_selectedCityMapEntity != null)
            {
                _selectedCityMapEntity.IsSelected = true;
            }
            else
            {
                _selectedEntityViewModel = null;
            }

            StateChanged?.Invoke();
        }
    }

    public CityMapEntityViewModel? SelectedEntityViewModel
    {
        get => _selectedEntityViewModel;
        set
        {
            if (_selectedEntityViewModel == value)
            {
                return;
            }

            _selectedEntityViewModel = value;
            StateChanged?.Invoke();
        }
    }
    
    public CityMapBuildingGroupViewModel? SelectedCityMapBuildingGroupViewModel
    {
        get => _selectedCityMapBuildingGroupViewModel;
        set
        {
            if (_selectedCityMapBuildingGroupViewModel == value)
            {
                return;
            }

            _selectedCityMapBuildingGroupViewModel = value;
            StateChanged?.Invoke();
        }
    }

    public IReadOnlyCollection<HohCitySnapshot> Snapshots
    {
        get => _snapshots.AsReadOnly();
        init => _snapshots = value.OrderBy(src => src.CreatedDateUtc).ToList();
    }

    public IReadOnlyDictionary<BuildingType, IList<CityMapEntity>> TypedEntities => _typedEntities.AsReadOnly();
    public event Action? StateChanged;

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
        else
        {
            // log
        }
    }

    public void AddRange(IEnumerable<CityMapEntity> cityMapEntities)
    {
        foreach (var entity in cityMapEntities)
        {
            Add(entity);
        }
    }

    public void AddSnapshot(HohCitySnapshot snapshot)
    {
        _snapshots.Add(snapshot);
        StateChanged?.Invoke();
    }

    public bool DeleteSnapshot(string id)
    {
        var ss = _snapshots.FirstOrDefault(src => src.Id == id);
        if (ss == null)
        {
            return false;
        }

        _snapshots.Remove(ss);
        StateChanged?.Invoke();
        return true;
    }

    public bool IsHappinessConsumer(CityMapEntity entity) => _happinessConsumers.Contains(entity);

    public bool IsHappinessProvider(CityMapEntity entity) => _happinessProviders.Contains(entity);

    public void Remove(int cityMapEntityId)
    {
        if (!_cityMapEntities.Remove(cityMapEntityId, out var foundEntity))
        {
            return;
        }

        _happinessProviders.Remove(foundEntity);
        _happinessConsumers.Remove(foundEntity);
        var building = Buildings[foundEntity.CityEntityId];
        _typedEntities[building.Type].Remove(foundEntity);
        if (foundEntity == _selectedCityMapEntity)
        {
            _selectedCityMapEntity = null;
        }
    }
}
