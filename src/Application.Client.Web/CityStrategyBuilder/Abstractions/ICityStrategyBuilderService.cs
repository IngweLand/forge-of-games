using System.Collections.ObjectModel;
using Ingweland.Fog.Application.Client.Web.CityPlanner;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;
using SkiaSharp;

namespace Ingweland.Fog.Application.Client.Web.CityStrategyBuilder.Abstractions;

public interface ICityStrategyBuilderService : IDisposable
{
    CityMapState CityMapState { get; }
    CityStrategyTimelineItemBase? SelectedTimelineItem { get; }
    CityStrategy Strategy { get; }
    ObservableCollection<CityStrategyTimelineItemBase> TimelineItems { get; }
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
}
