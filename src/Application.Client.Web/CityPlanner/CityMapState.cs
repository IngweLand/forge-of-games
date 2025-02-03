using Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Stats.BuildingTypedStats;
using Ingweland.Fog.Application.Client.Web.Factories;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Hoh.Constants;
using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner;

public class CityMapState(IBuildingLevelRangesFactory buildingLevelRangesFactory, ILogger<CityMapState> logger)
{
    private readonly IList<CityMapEntity> _cityMapEntities = new List<CityMapEntity>();
    private readonly IList<CityMapEntity> _happinessConsumers = new List<CityMapEntity>();
    private readonly IList<CityMapEntity> _happinessProviders = new List<CityMapEntity>();

    private readonly IDictionary<BuildingType, IList<CityMapEntity>> _typedEntities =
        new Dictionary<BuildingType, IList<CityMapEntity>>();

    private IReadOnlyDictionary<BuildingGroup, BuildingLevelRange>? _buildingLevelRanges;

    private AgeDto _cityAge;
    private CityPlannerCityPropertiesViewModel? _cityPropertiesViewModel;

    private CityStats _cityStats = new ();
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
    public required string CityName { get; init; }

    public IReadOnlyList<CityMapEntity> CityMapEntities => _cityMapEntities.AsReadOnly();

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

    public IReadOnlyList<CityMapEntity> HappinessConsumers => _happinessConsumers.AsReadOnly();
    public IReadOnlyList<CityMapEntity> HappinessProviders => _happinessProviders.AsReadOnly();
    public required CityId InGameCityId { get; init; }

    public IReadOnlyList<CityMapEntity>? SelectedCityMapEntities { get; set; }

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

    public IReadOnlyDictionary<BuildingType, IList<CityMapEntity>> TypedEntities => _typedEntities.AsReadOnly();

    public event Action? StateChanged;

    public bool IsHappinessProvider(CityMapEntity entity) => _happinessProviders.Contains(entity);
    public bool IsHappinessConsumer(CityMapEntity entity) => _happinessConsumers.Contains(entity);

    public void AddRange(IEnumerable<CityMapEntity> cityMapEntities)
    {
        foreach (var entity in cityMapEntities)
        {
            Add(entity);
        }
    }

    public void Add(CityMapEntity cityMapEntity)
    {
        if (Buildings.TryGetValue(cityMapEntity.CityEntityId, out var building))
        {
            if (building.Group == BuildingGroup.CityHall)
            {
                _cityAge = building.Age!;
            }

            _cityMapEntities.Add(cityMapEntity);
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

    public void Remove(CityMapEntity cityMapEntity)
    {
        _cityMapEntities.Remove(cityMapEntity);
        _happinessProviders.Remove(cityMapEntity);
        _happinessConsumers.Remove(cityMapEntity);
        var building = Buildings[cityMapEntity.CityEntityId];
        _typedEntities[building.Type].Remove(cityMapEntity);
        if (cityMapEntity == _selectedCityMapEntity)
        {
            _selectedCityMapEntity = null;
        }
    }
}
