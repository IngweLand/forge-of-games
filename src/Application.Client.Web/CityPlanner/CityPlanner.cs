using System.Drawing;
using AutoMapper;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Rendering;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Snapshots;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Snapshots.Abstractions;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Localization;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;
using Ingweland.Fog.Application.Core.CityPlanner;
using Ingweland.Fog.Application.Core.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Core.CityPlanner.Stats;
using Ingweland.Fog.Application.Core.CityPlanner.Stats.BuildingTypedStats;
using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Core.Factories.Interfaces;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.CityPlanner;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.Extensions.Localization;
using SkiaSharp;
using CityMapEntity = Ingweland.Fog.Application.Core.CityPlanner.CityMapEntity;
using CityMapExpansion = Ingweland.Fog.Application.Core.CityPlanner.CityMapExpansion;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner;

public class CityPlanner(
    IBuildingSelectorTypesViewModelFactory buildingSelectorTypesViewModelFactory,
    ICityMapEntityFactory cityMapEntityFactory,
    IMapAreaFactory mapAreaFactory,
    IBuildingRenderer buildingRenderer,
    IHohCityFactory hohCityFactory,
    ICityMapEntityViewModelFactory cityMapEntityViewModelFactory,
    ICityMapBuildingGroupViewModelFactory cityMapBuildingGroupViewModelFactory,
    ICityMapStateFactory cityMapStateFactory,
    IMapAreaRendererFactory mapAreaRendererFactory,
    ICityStatsProcessorFactory cityStatsProcessorFactory,
    ICommandManager commandManager,
    ICityMapEntityStatsFactory cityMapEntityStatsFactory,
    ICityPlannerCityPropertiesViewModelFactory cityPropertiesViewModelFactory,
    IHohCitySnapshotFactory snapshotFactory,
    IPersistenceService persistenceService,
    ISnapshotsComparisonViewModelFactory snapshotsComparisonViewModelFactory,
    IStringLocalizer<FogResource> localizer,
    ICityPlannerDataService cityPlannerDataService,
    IMapper mapper) : ICityPlanner
{
    private MapArea _mapArea = null!;
    private MapAreaRenderer _mapAreaRenderer = null!;
    private StatsProcessor _statsProcessor = null!;
    public event Action? StateHasChanged;
    public IReadOnlyCollection<NewCityDialogItemDto> NewCityDialogItems { get; private set; }

    public CityMapState CityMapState { get; private set; } = null!;

    public Task InitializeAsync()
    {
        return DoInitializeAsync(hohCityFactory.CreateNewCapital(FogConstants.CITY_PLANNER_VERSION));
    }

    public async Task<SnapshotsComparisonViewModel> CompareSnapshots()
    {
        var cityPlannerData = await cityPlannerDataService.GetCityPlannerDataAsync(CityMapState.InGameCityId);
        var city = GetCity();
        var stats = new Dictionary<HohCitySnapshot, CityStats>();
        foreach (var snapshot in CityMapState.Snapshots)
        {
            city.Entities = snapshot.Entities;
            var state = cityMapStateFactory.Create(cityPlannerData.Buildings, cityPlannerData.BuildingCustomizations,
                PrepareBuildingSelectorItems(cityPlannerData), cityPlannerData.Ages, city,
                cityPlannerData.Wonders.FirstOrDefault(src => src.Id == city.WonderId));
            var statsProcessor = cityStatsProcessorFactory.Create(state,
                cityPlannerData.City.Components.OfType<CityCultureAreaComponent>());

            stats.Add(snapshot, statsProcessor.UpdateStats());
        }

        var currentStateSnapshot =
            snapshotFactory.Create(mapper.Map<IList<HohCityMapEntity>>(CityMapState.CityMapEntities.Values),
                localizer[FogResource.CityPlanner_Snapshots_Current]);
        stats.Add(currentStateSnapshot, CityMapState.CityStats);
        return snapshotsComparisonViewModelFactory.Create(stats);
    }

    public HohCity CreateNew(NewCityRequest newCityRequest)
    {
        return hohCityFactory.Create(newCityRequest, FogConstants.CITY_PLANNER_VERSION);
    }

    public Task InitializeAsync(HohCity city)
    {
        return DoInitializeAsync(city);
    }

    public async Task LoadSnapshot(string id)
    {
        var snapshot = CityMapState.Snapshots.FirstOrDefault(src => src.Id == id);
        if (snapshot == null)
        {
            return;
        }

        var city = await persistenceService.LoadCity(CityMapState.CityId);
        if (city == null)
        {
            return;
        }

        city.Entities = snapshot.Entities.Select(src => src.Clone()).ToList();
        await DoInitializeAsync(city);
        await SaveCityAsync();
    }

    public Rectangle Bounds => _mapAreaRenderer.Bounds;

    public void RenderScene(SKCanvas canvas)
    {
        _mapAreaRenderer.Render(canvas);
        buildingRenderer.RenderBuildings(canvas, CityMapState.CityMapEntities.Values);
    }

    public Task CreateSnapshot()
    {
        if (CityMapState.Snapshots.Count >= FogConstants.MaxHohCitySnapshots)
        {
            return Task.CompletedTask;
        }

        var snapshot = snapshotFactory.Create(mapper.Map<IList<HohCityMapEntity>>(CityMapState.CityMapEntities.Values));
        CityMapState.AddSnapshot(snapshot);
        return SaveCityAsync();
    }

    public CityMapEntity AddEntity(BuildingGroup buildingGroup)
    {
        var buildings = CityMapState.Buildings.Values.Where(b => b.Group == buildingGroup).OrderBy(b => b.Level)
            .ToList();
        var building = buildings.First();
        var levelRange = CityMapState.BuildingLevelRanges![building.Group];
        var cme = cityMapEntityFactory.Create(building, Point.Empty, levelRange);
        FindFreeLocation(cme);
        CityMapState.Add(cme);
        CityMapState.CityStats = _statsProcessor.UpdateStats(cme);
        SelectCityMapEntity(cme);
        return cme;
    }

    public void AddEntity(CityMapEntity entity)
    {
        CityMapState.Add(entity);
        CityMapState.CityStats = _statsProcessor.UpdateStats(entity);
        SelectCityMapEntity(entity);
    }

    public void MoveEntity(int entityId, Point location)
    {
        if (!CityMapState.CityMapEntities.TryGetValue(entityId, out var entity))
        {
            return;
        }

        if (entity.Location == location)
        {
            return;
        }

        entity.Location = location;
        UpdateEntityState(entity);
        CityMapState.CityStats = _statsProcessor.UpdateStats(entity);
        UpdateSelectedEntityViewModel();
        StateHasChanged?.Invoke();
    }

    public void DeleteEntity(int entityId)
    {
        DeselectAll();

        if (!CityMapState.CityMapEntities.TryGetValue(entityId, out var entity))
        {
            return;
        }

        CityMapState.Remove(entity.Id);
        CityMapState.CityStats = _statsProcessor.UpdateStats();
        UpdateSelectedEntityViewModel();
        StateHasChanged?.Invoke();
    }

    public async Task SaveCityAsync()
    {
        await persistenceService.SaveCity(GetCity());
        StateHasChanged?.Invoke();
    }

    public async Task DeleteCityAsync()
    {
        var deleted = await persistenceService.DeleteCity(CityMapState.CityId);
        if (deleted)
        {
            await InitializeAsync();
            StateHasChanged?.Invoke();
        }
    }

    public async Task SaveCityAsync(string newCityName)
    {
        CityMapState.CityName = newCityName;
        await persistenceService.SaveCity(GetCity());
        UpdateCityPropertiesViewModel();
        StateHasChanged?.Invoke();
    }

    public Task DeleteSnapshot(string id)
    {
        return !CityMapState.DeleteSnapshot(id) ? Task.CompletedTask : SaveCityAsync();
    }

    public void RotateEntity(int entityId)
    {
        if (!CityMapState.CityMapEntities.TryGetValue(entityId, out var entity))
        {
            return;
        }

        entity.IsRotated = !entity.IsRotated;
        UpdateEntityState(entity);
        CityMapState.CityStats = _statsProcessor.UpdateStats(entity);
        UpdateSelectedEntityViewModel();
        StateHasChanged?.Invoke();
    }

    public void SelectGroup()
    {
        if (CityMapState.SelectedCityMapEntity == null)
        {
            return;
        }

        var building = CityMapState.Buildings[CityMapState.SelectedCityMapEntity.CityEntityId];
        SelectGroup(building.Group);
    }

    public bool TrySelectCityMapEntity(Point coordinates)
    {
        var foundEntity =
            CityMapState.CityMapEntities.Values.FirstOrDefault(entity => entity.Bounds.Contains(coordinates));

        if (foundEntity != null && foundEntity == CityMapState.SelectedCityMapEntity)
        {
            return false;
        }

        if (foundEntity == null && CityMapState.SelectedCityMapEntity == null &&
            CityMapState.SelectedCityMapEntities == null)
        {
            return false;
        }

        if (foundEntity != null)
        {
            SelectCityMapEntity(foundEntity);
            return true;
        }

        DeselectAll();
        return true;
    }

    public void UpdateEntityState(CityMapEntity entity)
    {
        entity.CanBePlaced = CanBePlaced(entity);
        entity.ExcludeFromStats = _mapArea.IntersectsWithLocked(entity.Bounds);
    }

    public bool CanBePlaced(CityMapEntity cityMapEntity)
    {
        var canBePlaced = true;
        if (!_mapArea.CanBePlaced(cityMapEntity))
        {
            canBePlaced = false;
        }
        else if (IntersectsWithBuilding(cityMapEntity))
        {
            canBePlaced = false;
        }

        return canBePlaced;
    }

    public CityMapEntity UpdateLevel(CityMapEntity entity, int level)
    {
        var newEntity = DoUpdateLevel(entity, level);
        SelectCityMapEntity(newEntity);

        CityMapState.CityStats = _statsProcessor.UpdateStats(CityMapState.SelectedCityMapEntity!);
        UpdateSelectedEntityViewModel();
        StateHasChanged?.Invoke();

        return newEntity;
    }

    public void UpdateLevels(IReadOnlyDictionary<int, int> cityMapEntityIdToLevelMap)
    {
        string? cityEntityId = null;
        foreach (var kvp in cityMapEntityIdToLevelMap)
        {
            if (!CityMapState.CityMapEntities.TryGetValue(kvp.Key, out var entity))
            {
                continue;
            }

            DoUpdateLevel(entity, kvp.Value);
            cityEntityId = entity.CityEntityId;
        }

        if (cityEntityId == null)
        {
            return;
        }

        FinalizeGroupLevelUpdate(cityEntityId);

        StateHasChanged?.Invoke();
    }

    public void UpdateCustomization(BuildingCustomizationDto customization)
    {
        if (CityMapState.SelectedCityMapEntity == null)
        {
            return;
        }

        if (customization == BuildingCustomizationDto.None)
        {
            CityMapState.SelectedCityMapEntity.CustomizationId = null;
        }
        else
        {
            CityMapState.SelectedCityMapEntity.CustomizationId = customization.Id;
        }

        CityMapState.CityStats = _statsProcessor.UpdateStats(CityMapState.SelectedCityMapEntity!);
        UpdateSelectedEntityViewModel();
        StateHasChanged?.Invoke();
    }

    public void UpdateProduct(string productId)
    {
        if (CityMapState.SelectedCityMapEntity == null)
        {
            return;
        }

        var hasChanged = false;
        if (CityMapState.SelectedCityMapEntity.SelectedProductId == productId)
        {
            CityMapState.SelectedCityMapEntity.SelectedProductId = null;
            hasChanged = true;
        }
        else
        {
            var building = CityMapState.Buildings[CityMapState.SelectedCityMapEntity.CityEntityId];
            var productionComponent =
                building.Components.OfType<ProductionComponent>().FirstOrDefault(pc => pc.Id == productId);
            if (productionComponent != null)
            {
                CityMapState.SelectedCityMapEntity.SelectedProductId = productId;
                hasChanged = true;
            }
        }

        if (hasChanged)
        {
            CityMapState.CityStats = _statsProcessor.UpdateStats(CityMapState.SelectedCityMapEntity!);
            UpdateSelectedEntityViewModel();
            StateHasChanged?.Invoke();
        }
    }

    public bool TryToggleExpansion(Point coordinates)
    {
        var expansion = _mapArea.GetExpansion(coordinates);

        if (expansion != null)
        {
            if (IntersectsWithBuilding(expansion))
            {
                expansion = null;
            }
            else
            {
                expansion.IsLocked = !expansion.IsLocked;
                foreach (var cityMapEntity in CityMapState.CityMapEntities.Values)
                {
                    cityMapEntity.ExcludeFromStats = _mapArea.IntersectsWithLocked(cityMapEntity.Bounds);
                }

                CityMapState.CityStats = _statsProcessor.UpdateStats();
                UpdateCityPropertiesViewModel();
                StateHasChanged?.Invoke();
            }
        }

        return expansion != null;
    }

    public void UpdateWonderLevel(int level)
    {
        if (CityMapState.CityWonderLevel == level)
        {
            return;
        }

        CityMapState.CityWonderLevel = level;
        CityMapState.CityStats = _statsProcessor.UpdateStats();
        if (CityMapState.SelectedCityMapEntity == null)
        {
            UpdateCityPropertiesViewModel();
        }
        else
        {
            UpdateSelectedEntityViewModel();
        }

        StateHasChanged?.Invoke();
    }

    private void FinalizeGroupLevelUpdate(string cityEntityId)
    {
        var building = CityMapState.Buildings[cityEntityId];
        SelectGroup(building.Group);
        CityMapState.CityStats = _statsProcessor.UpdateStats();
        UpdateSelectedCityMapBuildingGroupViewModel();
    }

    private HohCity GetCity()
    {
        DeselectAll();

        return hohCityFactory.Create(CityMapState.CityId, CityMapState.InGameCityId, CityMapState.CityAge.Id,
            CityMapState.CityName, CityMapState.CityMapEntities.Values, CityMapState.Snapshots,
            _mapArea.UsableExpansions.Where(e => !e.IsLocked).Select(e => e.Id), FogConstants.CITY_PLANNER_VERSION,
            CityMapState.CityWonder?.Id ?? WonderId.Undefined, CityMapState.CityWonderLevel);
    }

    private void DeselectAll()
    {
        if (CityMapState.SelectedCityMapEntity != null)
        {
            if (!CanBePlaced(CityMapState.SelectedCityMapEntity))
            {
                CityMapState.SelectedCityMapEntity.Location = CityMapState.SelectedCityMapEntity.LastValidLocation;
                CityMapState.SelectedCityMapEntity.IsRotated = CityMapState.SelectedCityMapEntity.LastValidRotation;
                CityMapState.SelectedCityMapEntity.CanBePlaced = true;
            }
            // else if(SelectedCityMapEntity.Location != SelectedCityMapEntity.PreviousLocation)
            // {
            //     var cmd = new PlaceEntityCommand(SelectedCityMapEntity, SelectedCityMapEntity.PreviousLocation);
            //     commandManager.ExecuteCommand(cmd);
            // }
        }

        foreach (var cityMapEntity in CityMapState.CityMapEntities.Values)
        {
            cityMapEntity.IsSelected = false;
        }

        CityMapState.SelectedCityMapEntities = null;
        CityMapState.SelectedCityMapEntity = null;
        CityMapState.SelectedEntityViewModel = null;
        CityMapState.SelectedCityMapBuildingGroupViewModel = null;
    }

    private async Task DoInitializeAsync(HohCity city)
    {
        var cityPlannerData = await cityPlannerDataService.GetCityPlannerDataAsync(city.InGameCityId);
        NewCityDialogItems = cityPlannerData.NewCityDialogItems;

        await buildingRenderer.InitializeAsync();

        commandManager.Reset();
        CityMapState = cityMapStateFactory.Create(cityPlannerData.Buildings, cityPlannerData.BuildingCustomizations,
            PrepareBuildingSelectorItems(cityPlannerData), cityPlannerData.Ages, city,
            cityPlannerData.Wonders.FirstOrDefault(src => src.Id == city.WonderId));
        _mapArea = mapAreaFactory.Create(cityPlannerData.City.InitConfigs.Grid.ExpansionSize,
            cityPlannerData.Expansions, city.UnlockedExpansions,
            cityPlannerData.City.Components.OfType<CityCultureAreaComponent>());
        _mapAreaRenderer = mapAreaRendererFactory.Create(_mapArea);
        var lockedMapEntities = CityMapState.CityMapEntities.Values.Where(e => _mapArea.IntersectsWithLocked(e.Bounds))
            .ToList();
        foreach (var lockedMapEntity in lockedMapEntities)
        {
            lockedMapEntity.ExcludeFromStats = true;
        }

        _statsProcessor = cityStatsProcessorFactory.Create(CityMapState,
            cityPlannerData.City.Components.OfType<CityCultureAreaComponent>());
        CityMapState.CityStats = _statsProcessor.UpdateStats();
        UpdateCityPropertiesViewModel();
        StateHasChanged?.Invoke();
    }

    private CityMapEntity DoUpdateLevel(CityMapEntity currentEntity, int level)
    {
        var currentBuilding = CityMapState.Buildings[currentEntity.CityEntityId];
        CityMapEntity newEntity;
        if (currentBuilding.Type != BuildingType.Evolving)
        {
            var newBuilding = CityMapState.Buildings.FirstOrDefault(kvp =>
                kvp.Value.Group == currentBuilding.Group && kvp.Value.Level == level).Value;
            var newStats = cityMapEntityStatsFactory.Create(newBuilding);
            newEntity = currentEntity.CloneWithLevel(newBuilding.Id, level, newStats);
            if (currentEntity.SelectedProductId != null)
            {
                var lastProduct = newEntity.FirstOrDefaultStat<ProductionProvider>()?.ProductionComponents
                    .LastOrDefault();
                if (lastProduct != null)
                {
                    newEntity.SelectedProductId = lastProduct.Id;
                }
            }
        }
        else
        {
            var levelRange = CityMapState.BuildingLevelRanges![currentBuilding.Group];
            newEntity = cityMapEntityFactory.Create(currentBuilding, currentEntity.Location, levelRange, level);
        }

        CityMapState.Remove(currentEntity.Id);
        CityMapState.Add(newEntity);
        return newEntity;
    }

    private void FindFreeLocation(CityMapEntity cityMapEntity)
    {
        for (var i = _mapArea.Bounds.Y; i < _mapArea.Bounds.Bottom; i++)
        {
            for (var j = _mapArea.Bounds.X; j < _mapArea.Bounds.Right; j++)
            {
                cityMapEntity.Location = new Point(j, i);
                if (CanBePlaced(cityMapEntity))
                {
                    return;
                }
            }
        }

        cityMapEntity.Location = new Point(_mapArea.Bounds.X - cityMapEntity.Bounds.Width - 1, _mapArea.Bounds.Y);
    }

    private bool IntersectsWithBuilding(CityMapEntity targetEntity)
    {
        return CityMapState.CityMapEntities.Values.Any(cityMapEntity =>
            cityMapEntity != targetEntity && cityMapEntity.Bounds.IntersectsWith(targetEntity.Bounds));
    }

    private bool IntersectsWithBuilding(CityMapExpansion expansion)
    {
        return CityMapState.CityMapEntities.Values.Any(cityMapEntity =>
            cityMapEntity.IsMovable && cityMapEntity.Bounds.IntersectsWith(expansion.Bounds));
    }

    private List<BuildingSelectorTypesViewModel> PrepareBuildingSelectorItems(CityPlannerDataDto cityPlannerData)
    {
        var buildings = cityPlannerData.Buildings;
        var types = buildings.GroupBy(b => b.Type).ToDictionary(g => g.Key);
        var result = cityPlannerData.City.BuildMenuTypes.Where(bt => bt != BuildingType.Special).Select(buildingType =>
            buildingSelectorTypesViewModelFactory.Create(buildingType,
                types[buildingType].OrderBy(b => b.Width * b.Length).ThenBy(b => b.Name))).ToList();
        var otherTypes = types.Where(kvp =>
                kvp.Key == BuildingType.Special || !cityPlannerData.City.BuildMenuTypes.Contains(kvp.Key))
            .SelectMany(kvp => kvp.Value.ToList()).ToList();
        if (otherTypes.Count > 0)
        {
            result.Add(buildingSelectorTypesViewModelFactory.Create(BuildingType.Special, otherTypes));
        }

        return result;
    }

    private void SelectCityMapEntities(IList<CityMapEntity> cityMapEntities)
    {
        DeselectAll();

        if (cityMapEntities.Count == 0)
        {
            return;
        }

        CityMapState.SelectedCityMapEntities = cityMapEntities.AsReadOnly();
        UpdateSelectedCityMapBuildingGroupViewModel();
        StateHasChanged?.Invoke();
    }

    private void SelectCityMapEntity(CityMapEntity? cityMapEntity)
    {
        DeselectAll();

        CityMapState.SelectedCityMapEntity = cityMapEntity;
        UpdateSelectedEntityViewModel();
        StateHasChanged?.Invoke();
    }

    private void SelectGroup(BuildingGroup buildingGroup)
    {
        var entities = CityMapState.CityMapEntities.Values
            .Where(cme => CityMapState.Buildings[cme.CityEntityId].Group == buildingGroup)
            .ToList();
        if (entities.Count < 2)
        {
            return;
        }

        SelectCityMapEntities(entities);
    }

    private void UpdateCityPropertiesViewModel()
    {
        var age = CityMapState.CityPropertiesViewModel?.Age;
        if (age?.Id != CityMapState.CityAge.Id)
        {
            age = mapper.Map<AgeViewModel>(CityMapState.CityAge);
        }

        CityMapState.CityPropertiesViewModel = cityPropertiesViewModelFactory.Create(CityMapState.InGameCityId,
            CityMapState.CityName, age, CityMapState.CityStats, CityMapState.Buildings.Values,
            CityMapState.CityWonder?.WonderName, CityMapState.CityWonderLevel);
    }

    private void UpdateSelectedEntityViewModel()
    {
        if (CityMapState.SelectedCityMapEntity == null)
        {
            CityMapState.SelectedEntityViewModel = null;
            return;
        }

        var building = CityMapState.Buildings[CityMapState.SelectedCityMapEntity.CityEntityId];
        var levelRange = CityMapState.BuildingLevelRanges![building.Group];
        var customizations = CityMapState.BuildingCustomizations.Where(bc => bc.BuildingGroup == building.Group)
            .ToList();
        CityMapState.SelectedEntityViewModel = cityMapEntityViewModelFactory.Create(CityMapState.SelectedCityMapEntity,
            building, levelRange, customizations);
        UpdateCityPropertiesViewModel();
    }

    private void UpdateSelectedCityMapBuildingGroupViewModel()
    {
        if (CityMapState.SelectedCityMapEntities == null)
        {
            CityMapState.SelectedCityMapBuildingGroupViewModel = null;
            return;
        }

        var levelGroups = CityMapState.SelectedCityMapEntities.GroupBy(src => src.Level).ToList();
        var building = CityMapState.Buildings[CityMapState.SelectedCityMapEntities[0].CityEntityId];
        var levelRange = CityMapState.BuildingLevelRanges![building.Group];
        CityMapState.SelectedCityMapBuildingGroupViewModel = cityMapBuildingGroupViewModelFactory.Create(
            building.Group, building.Name, levelGroups.Count == 1 ? building.Age : null,
            levelGroups.Count == 1 ? levelGroups[0].Key : null, levelRange);
        UpdateCityPropertiesViewModel();
    }
}
