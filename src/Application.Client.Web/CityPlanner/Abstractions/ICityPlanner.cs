using System.Drawing;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Snapshots;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.CityPlanner;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;
using SkiaSharp;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

public interface ICityPlanner
{
    Rectangle Bounds { get; }
    CityMapState CityMapState { get; }
    IReadOnlyCollection<NewCityDialogItemDto> NewCityDialogItems { get; }
    public event Action? StateHasChanged;
    CityMapEntity AddEntity(BuildingGroup buildingGroup);
    void AddEntity(CityMapEntity entity);
    bool CanBePlaced(CityMapEntity cityMapEntity);
    SnapshotsComparisonViewModel CompareSnapshots();
    HohCity CreateNew(NewCityRequest newCityRequest);
    Task CreateSnapshot();
    void DeleteEntity(int entityId);
    Task DeleteSnapshot(string id);
    Task InitializeAsync();
    Task InitializeAsync(HohCity city);
    Task LoadSnapshot(string id);
    void MoveEntity(int entityId, Point location);
    void RenderScene(SKCanvas canvas);
    void RotateEntity(int entityId);
    Task SaveCityAsync();
    Task SaveCityAsync(string newCityName);
    void SelectGroup();
    bool TrySelectCityMapEntity(Point coordinates);
    void UpdateCustomization(BuildingCustomizationDto customization);
    void UpdateEntityState(CityMapEntity entity);
    CityMapEntity UpdateLevel(CityMapEntity entity, int level);

    void UpdateLevels(IReadOnlyDictionary<int, int> mapEntityIdToOldLevelMap);

    void UpdateProduct(string productId);
    Task DeleteCityAsync();

    bool TryToggleExpansion(Point coordinates);
    void UpdateWonderLevel(int level);
}