using System.Drawing;
using AutoMapper;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Rendering;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Snapshots;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Snapshots.Abstractions;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Stats.BuildingTypedStats;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Localization;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;
using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Core.Factories.Interfaces;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.CityPlanner;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using SkiaSharp;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner;

public class CityPlanner(
    ICityService cityService,
    IBuildingSelectorTypesViewModelFactory buildingSelectorTypesViewModelFactory,
    ICityMapEntityFactory cityMapEntityFactory,
    IMapAreaFactory mapAreaFactory,
    IBuildingRenderer buildingRenderer,
    IHohCityFactory hohCityFactory,
    ICityMapEntityViewModelFactory cityMapEntityViewModelFactory,
    ILogger<CityPlanner> logger,
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
    IMapper mapper) : ICityPlanner
{
    private IDictionary<CityId, CityPlannerDataDto>
        _cityPlannerDataCache = new Dictionary<CityId, CityPlannerDataDto>();

    private MapArea _mapArea = null!;
    private MapAreaRenderer _mapAreaRenderer = null!;
    private StatsProcessor _statsProcessor = null!;
    public event Action? StateHasChanged;

    public CityMapState CityMapState { get; private set; } = null!;

    public Task InitializeAsync()
    {
        return DoInitializeAsync(hohCityFactory.CreateNewCapital());
    }

    public SnapshotsComparisonViewModel CompareSnapshots()
    {
        var cityPlannerData = _cityPlannerDataCache[CityMapState.InGameCityId];
        var city = GetCity();
        var stats = new Dictionary<HohCitySnapshot, CityStats>();
        foreach (var snapshot in CityMapState.Snapshots)
        {
            city.Entities = snapshot.Entities;
            var state = cityMapStateFactory.Create(cityPlannerData.Buildings, cityPlannerData.BuildingCustomizations,
                PrepareBuildingSelectorItems(cityPlannerData), cityPlannerData.Ages, city);
            var statsProcessor = cityStatsProcessorFactory.Create(state);
            statsProcessor.UpdateStats();

            stats.Add(snapshot, state.CityStats);
        }

        var currentStateSnapshot =
            snapshotFactory.Create(mapper.Map<IList<HohCityMapEntity>>(CityMapState.CityMapEntities),
                localizer[FogResource.CityPlanner_Snapshots_Current]);
        stats.Add(currentStateSnapshot, CityMapState.CityStats);
        return snapshotsComparisonViewModelFactory.Create(stats);
    }

    public HohCity CreateNew(string cityName)
    {
        return hohCityFactory.CreateNewCapital(cityName);
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
        buildingRenderer.RenderBuildings(canvas, CityMapState.CityMapEntities);
    }

    public Task CreateSnapshot()
    {
        if (CityMapState.Snapshots.Count >= FogConstants.MaxHohCitySnapshots)
        {
            return Task.CompletedTask;
        }

        var snapshot = snapshotFactory.Create(mapper.Map<IList<HohCityMapEntity>>(CityMapState.CityMapEntities));
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
        _statsProcessor.UpdateStats(cme);
        SelectCityMapEntity(cme);
        return cme;
    }

    public void AddEntity(CityMapEntity entity)
    {
        CityMapState.Add(entity);
        _statsProcessor.UpdateStats(entity);
        SelectCityMapEntity(entity);
    }

    public void MoveEntity(CityMapEntity entity, Point location)
    {
        if (entity.Location == location)
        {
            return;
        }

        entity.Location = location;
        UpdateEntityState(entity);
        _statsProcessor.UpdateStats(entity);
        UpdateSelectedEntityViewModel();
        StateHasChanged?.Invoke();
    }

    public void DeleteEntity(CityMapEntity entity)
    {
        DeselectAll();

        CityMapState.Remove(entity);
        _statsProcessor.UpdateStats();
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

    public void RotateEntity(CityMapEntity entity)
    {
        entity.IsRotated = !entity.IsRotated;
        UpdateEntityState(entity);
        _statsProcessor.UpdateStats(entity);
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

    public bool TrySelectCityMapEntity(Point coordinates, out CityMapEntity? cityMapEntity)
    {
        var foundCityMapEntity =
            CityMapState.CityMapEntities.FirstOrDefault(cityMapEntity =>
                cityMapEntity.Bounds.Contains(coordinates));

        if (CityMapState.SelectedCityMapEntity != null && foundCityMapEntity == CityMapState.SelectedCityMapEntity)
        {
            cityMapEntity = null;
            return false;
        }

        SelectCityMapEntity(foundCityMapEntity);

        cityMapEntity = foundCityMapEntity;
        return true;
    }

    public void UpdateEntityState(CityMapEntity entity)
    {
        entity.CanBePlaced = CanBePlaced(entity);
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
        var hasChanged = false;
        var newEntity = DoUpdateLevel(entity, level);
        SelectCityMapEntity(newEntity);
        hasChanged = true;
        // else if (CityMapState.SelectedCityMapEntities != null)
        // {
        //     foreach (var entity in CityMapState.SelectedCityMapEntities)
        //     {
        //         var newEntity = DoUpdateLevel(entity, level);
        //         if (newEntity != null)
        //         {
        //             hasChanged = true;
        //         }
        //     }
        //
        //     if (hasChanged)
        //     {
        //         var building = CityMapState.Buildings[CityMapState.SelectedCityMapEntities.First().CityEntityId];
        //         SelectGroup(building.Group);
        //     }
        // }

        if (hasChanged)
        {
            _statsProcessor.UpdateStats(CityMapState.SelectedCityMapEntity!);
            UpdateSelectedEntityViewModel();
            StateHasChanged?.Invoke();
        }

        return newEntity;
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

        _statsProcessor.UpdateStats(CityMapState.SelectedCityMapEntity!);
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
            _statsProcessor.UpdateStats(CityMapState.SelectedCityMapEntity!);
            UpdateSelectedEntityViewModel();
            StateHasChanged?.Invoke();
        }
    }

    private HohCity GetCity()
    {
        DeselectAll();
        UpdateSelectedEntityViewModel();

        return hohCityFactory.Create(CityMapState.CityId, CityMapState.InGameCityId, CityMapState.CityAge.Id,
            CityMapState.CityName, CityMapState.CityMapEntities, CityMapState.Snapshots);
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

        foreach (var cityMapEntity in CityMapState.CityMapEntities)
        {
            cityMapEntity.IsSelected = false;
        }

        CityMapState.SelectedCityMapEntities = null;
        CityMapState.SelectedCityMapEntity = null;
    }

    private async Task DoInitializeAsync(HohCity city)
    {
        if (!_cityPlannerDataCache.TryGetValue(city.InGameCityId, out var cityPlannerData))
        {
            cityPlannerData = (await cityService.GetCityPlannerDataAsync(city.InGameCityId))!;
            _cityPlannerDataCache.Add(city.InGameCityId, cityPlannerData);
        }

        commandManager.Reset();
        CityMapState = cityMapStateFactory.Create(cityPlannerData.Buildings, cityPlannerData.BuildingCustomizations,
            PrepareBuildingSelectorItems(cityPlannerData), cityPlannerData.Ages, city);
        _mapArea = mapAreaFactory.Create(cityPlannerData.ExpansionSize, cityPlannerData.Expansions);
        _mapAreaRenderer = mapAreaRendererFactory.Create(_mapArea);
        _statsProcessor = cityStatsProcessorFactory.Create(CityMapState);
        _statsProcessor.UpdateStats();
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

        CityMapState.Remove(currentEntity);
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
        return CityMapState.CityMapEntities.Any(cityMapEntity =>
            cityMapEntity != targetEntity && cityMapEntity.Bounds.IntersectsWith(targetEntity.Bounds));
    }

    private List<BuildingSelectorTypesViewModel> PrepareBuildingSelectorItems(CityPlannerDataDto cityPlannerData)
    {
        var buildings = cityPlannerData.Buildings;
        var types = buildings.GroupBy(b => b.Type).ToDictionary(g => g.Key);
        var result = cityPlannerData.BuildMenuTypes.Where(bt => bt != BuildingType.Special).Select(buildingType =>
            buildingSelectorTypesViewModelFactory.Create(buildingType,
                types[buildingType].OrderBy(b => b.Width * b.Length).ThenBy(b => b.Name))).ToList();
        var otherTypes = types.Where(kvp =>
                kvp.Key == BuildingType.Special || !cityPlannerData.BuildMenuTypes.Contains(kvp.Key))
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

        foreach (var cityMapEntity in cityMapEntities)
        {
            cityMapEntity.IsSelected = true;
        }

        CityMapState.SelectedCityMapEntities = cityMapEntities.AsReadOnly();
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
        SelectCityMapEntities(CityMapState.CityMapEntities
            .Where(cme => CityMapState.Buildings[cme.CityEntityId].Group == buildingGroup)
            .ToList());
    }

    private void UpdateCityPropertiesViewModel()
    {
        var age = CityMapState.CityPropertiesViewModel?.Age;
        if (age?.Id != CityMapState.CityAge.Id)
        {
            age = mapper.Map<AgeViewModel>(CityMapState.CityAge);
        }

        CityMapState.CityPropertiesViewModel = cityPropertiesViewModelFactory.Create(CityMapState.InGameCityId,
            CityMapState.CityName, age, CityMapState.CityStats, CityMapState.Buildings.Values);
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
            building, levelRange, customizations, CityMapState.CityAge);
        UpdateCityPropertiesViewModel();
    }
}