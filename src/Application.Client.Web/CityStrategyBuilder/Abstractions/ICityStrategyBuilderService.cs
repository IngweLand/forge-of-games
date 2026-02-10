using System.Collections.ObjectModel;
using FluentResults;
using Ingweland.Fog.Application.Client.Web.CityPlanner;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;
using SkiaSharp;

namespace Ingweland.Fog.Application.Client.Web.CityStrategyBuilder.Abstractions;

public interface ICityStrategyBuilderService : IDisposable
{
    IReadOnlyCollection<WonderDto>? AllowedWonders { get; }
    CityMapState CityMapState { get; }
    WonderDto? CurrentWonder { get; }
    CityStrategyTimelineItemBase? SelectedTimelineItem { get; }
    CityStrategy Strategy { get; }
    ObservableCollection<CityStrategyTimelineItemBase> TimelineItems { get; }
    Task ChangeWonder(WonderId newWonder);
    Task InitializeAsync(CityStrategy strategy, bool isReadOnly = false);
    public event Action? StateHasChanged;
    bool DeleteSelectedCityMapEntity();
    bool RotateSelectedCityMapEntity();
    bool DuplicateSelectedCityMapEntity();
    void AddNewCityMapEntity(BuildingGroup buildingGroup);
    void SelectCityMapEntityGroup();
    Task Save();
    void RenderScene(SKCanvas canvas);
    Task DeleteTimelineItem(CityStrategyTimelineItemBase item);
    Task CreateTimelineItemAsync(CityStrategyTimelineItemCreateRequest request);
    Task SelectTimelineItem(string id);
    void RequestSaving();
    Task<IReadOnlyCollection<HohCityBasicData>> GetCities();
    Task DeleteStrategy();
    Task Rename(string newName);
    void DeselectAll();
    Task SelectNextItem();
    Task SelectPreviousItem();
    Task MoveTimelineItemDown(CityStrategyTimelineItemBase item);
    Task MoveTimelineItemUp(CityStrategyTimelineItemBase item);
    Task ExportCurrentLayoutItemToCityPlanner();
    HohCity CreateCity(CityStrategyLayoutTimelineItem item);
    Result<byte[]> GenerateCurrentLayoutItemImage();
}
