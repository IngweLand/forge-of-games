using System.Drawing;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Snapshots;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;
using SkiaSharp;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

public interface ICityPlanner
{
    Rectangle Bounds { get; }
    CityMapState CityMapState { get; }
    public event Action? StateHasChanged;
    CityMapEntity AddEntity(BuildingGroup buildingGroup);
    void AddEntity(CityMapEntity entity);
    bool CanBePlaced(CityMapEntity cityMapEntity);
    SnapshotsComparisonViewModel CompareSnapshots();
    HohCity CreateNew(string cityName);
    Task CreateSnapshot();
    void DeleteEntity(CityMapEntity entity);
    Task DeleteSnapshot(string id);
    Task InitializeAsync();
    Task InitializeAsync(HohCity city);
    Task LoadSnapshot(string id);
    void MoveEntity(CityMapEntity entity, Point location);
    void RenderScene(SKCanvas canvas);
    void RotateEntity(CityMapEntity entity);
    Task SaveCityAsync();
    Task SaveCityAsync(string newCityName);
    void SelectGroup();
    bool TrySelectCityMapEntity(Point coordinates, out CityMapEntity? cityMapEntity);
    void UpdateCustomization(BuildingCustomizationDto customization);
    void UpdateEntityState(CityMapEntity entity);
    CityMapEntity UpdateLevel(CityMapEntity entity, int level);
    void UpdateProduct(string productId);
    Task DeleteCityAsync();
}