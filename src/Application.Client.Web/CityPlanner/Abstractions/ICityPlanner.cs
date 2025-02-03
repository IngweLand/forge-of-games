using System.Drawing;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.CityPlanner;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;
using SkiaSharp;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

public interface ICityPlanner
{
    public event Action? StateHasChanged;
    Rectangle Bounds { get; }
    HohCity GetCity();
    Task InitializeAsync();
    HohCity CreateNew(string cityName);
    void RenderScene(SKCanvas canvas);
    CityMapEntity AddEntity(BuildingGroup buildingGroup);
    bool TrySelectCityMapEntity(Point coordinates, out CityMapEntity? cityMapEntity);
    void UpdateEntityState(CityMapEntity entity);
    Task InitializeAsync(HohCity city);
    void SelectGroup();
    void RotateEntity(CityMapEntity entity);
    void DeleteEntity(CityMapEntity entity);
    void AddEntity(CityMapEntity entity);
    void MoveEntity(CityMapEntity entity, Point location);
    bool CanBePlaced(CityMapEntity cityMapEntity);
    CityMapEntity UpdateLevel(CityMapEntity entity, int level);
    void UpdateCustomization(BuildingCustomizationDto customization);
    CityMapState CityMapState { get; }
    void UpdateProduct(string productId);
}
