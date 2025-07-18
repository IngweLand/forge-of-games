using Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;
using Ingweland.Fog.Application.Core.CityPlanner;
using Ingweland.Fog.Application.Core.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Core.CityPlanner.Stats;
using Ingweland.Fog.Application.Core.Factories.Interfaces;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner;

public class CityMapState(
    IBuildingLevelRangesFactory buildingLevelRangesFactory, IMapArea mapArea) : CityMapStateCore(mapArea)
{
    private readonly List<CityMapEntity> _inventoryBuildings = [];
    private readonly IList<HohCitySnapshot> _snapshots = new List<HohCitySnapshot>();

    private IReadOnlyDictionary<BuildingGroup, BuildingLevelRange>? _buildingLevelRanges;
    private readonly IReadOnlyCollection<BuildingSelectorTypesViewModel> _buildingSelectorItems = [];

    private CityPlannerCityPropertiesViewModel? _cityPropertiesViewModel;

    private CityStats _cityStats = new();
    private Dictionary<BuildingGroup, BuildingSelectorItemViewModel> _flattenedBuildingSelectorItems = [];
    private CityMapBuildingGroupViewModel? _selectedCityMapBuildingGroupViewModel;
    private IReadOnlyList<CityMapEntity>? _selectedCityMapEntities;
    private CityMapEntity? _selectedCityMapEntity;
    private CityMapEntityViewModel? _selectedEntityViewModel;
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
        private set => _buildingLevelRanges = value;
    }

    public required IReadOnlyCollection<BuildingSelectorTypesViewModel> BuildingSelectorItems
    {
        get => _buildingSelectorItems;
        init
        {
            _buildingSelectorItems = value;
            _flattenedBuildingSelectorItems = _buildingSelectorItems
                .SelectMany(x => x.BuildingGroups)
                .ToDictionary(x => x.BuildingGroup);
        }
    }

    public required string CityId { get; init; }

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

    public required CityId InGameCityId { get; init; }

    public IReadOnlyCollection<CityMapEntity> InventoryBuildings => _inventoryBuildings.AsReadOnly();

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

    public IReadOnlyCollection<HohCitySnapshot> Snapshots
    {
        get => _snapshots.AsReadOnly();
        init => _snapshots = value.OrderBy(src => src.CreatedDateUtc).ToList();
    }

    public void AddToInventory(CityMapEntity entity)
    {
        _inventoryBuildings.Add(entity);
        _flattenedBuildingSelectorItems[entity.BuildingGroup].Count++;
        StateChanged?.Invoke();
    }
    
    public void AddToInventory(IEnumerable<CityMapEntity> entities)
    {
        foreach (var entity in entities)
        {
            _inventoryBuildings.Add(entity);
            _flattenedBuildingSelectorItems[entity.BuildingGroup].Count++;
        }
        
        StateChanged?.Invoke();
    }
    
    public CityMapEntity? RemoveFromInventory(int entityId)
    {
        var entity = _inventoryBuildings.FirstOrDefault(e => e.Id == entityId);
        if (entity == null)
        {
            return null;
        }
        _inventoryBuildings.Remove(entity);
        _flattenedBuildingSelectorItems[entity.BuildingGroup].Count--;
        StateChanged?.Invoke();
        return entity;
    }
    
    public void PurgeInventory()
    {
        _inventoryBuildings.Clear();
        foreach (var item in _flattenedBuildingSelectorItems.Values)
        {
            item.Count = 0;
        }
        StateChanged?.Invoke();   
    }

    public event Action? StateChanged;

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

    protected override void OnAfterRemove(CityMapEntity removedEntity)
    {
        if (removedEntity == _selectedCityMapEntity)
        {
            _selectedCityMapEntity = null;
        }
    }
}
