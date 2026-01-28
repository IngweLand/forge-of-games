using System.Collections.ObjectModel;
using System.Drawing;
using System.Timers;
using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Analytics.Interfaces;
using Ingweland.Fog.Application.Client.Web.CityPlanner;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.CityStrategyBuilder.Abstractions;
using Ingweland.Fog.Application.Client.Web.Constants;
using Ingweland.Fog.Application.Client.Web.Localization;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Core.CityPlanner.Abstractions;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using SkiaSharp;
using Timer = System.Timers.Timer;

namespace Ingweland.Fog.Application.Client.Web.CityStrategyBuilder;

// Make sure you inject this service only on the client side.
// There is an issue with SkiSharp (used in CityPlanner) that causes crashes on the server side.
public class CityStrategyBuilderService(
    ICityPlanner cityPlanner,
    ICommandManager commandManager,
    ICityPlannerCommandFactory commandFactory,
    ICityStrategyAnalyticsService analyticsService,
    IHohCityFactory hohCityFactory,
    IPersistenceService persistenceService,
    ICityStrategyFactory cityStrategyFactory,
    IStringLocalizer<FogResource> loc,
    IMapper mapper,
    ILogger<CityStrategyBuilderService> logger) : ICityStrategyBuilderService
{
    private static readonly TimeSpan AutoSaveInterval = TimeSpan.FromSeconds(10);
    private readonly SemaphoreSlim _saveSemaphore = new(1, 1);
    private Timer? _autoSaveTimer;
    private bool _isInitialized;
    private bool _isReadOnly;
    private bool _savingRequested;
    private Dictionary<string, string> _selectionHistory = new();
    public CityStrategy Strategy { get; private set; } = null!;
    public CityStrategyTimelineItemBase? SelectedTimelineItem { get; private set; }
    public ObservableCollection<CityStrategyTimelineItemBase> TimelineItems { get; private set; }
    public CityMapState CityMapState => cityPlanner.CityMapState;

    public Task Rename(string newName)
    {
        Strategy.Name = newName;
        return Save();
    }

    public void DeselectAll()
    {
        cityPlanner.DeselectAll();
    }

    public event Action? StateHasChanged
    {
        add => cityPlanner.StateHasChanged += value;
        remove => cityPlanner.StateHasChanged -= value;
    }

    public bool DeleteSelectedCityMapEntity()
    {
        if (cityPlanner.CityMapState.SelectedCityMapEntity is not {IsMovable: true})
        {
            return false;
        }

        var cmd = commandFactory.CreateDeleteEntityCommand(cityPlanner.CityMapState.SelectedCityMapEntity);
        commandManager.ExecuteCommand(cmd);

        return true;
    }

    public bool RotateSelectedCityMapEntity()
    {
        if (cityPlanner.CityMapState.SelectedCityMapEntity is not {IsMovable: true})
        {
            return false;
        }

        var cmd = commandFactory.CreateRotateEntityCommand(cityPlanner.CityMapState.SelectedCityMapEntity.Id);
        commandManager.ExecuteCommand(cmd);

        return true;
    }

    public bool DuplicateSelectedCityMapEntity()
    {
        if (cityPlanner.CityMapState.SelectedCityMapEntity is not {IsMovable: true})
        {
            return false;
        }

        var cmd = commandFactory.CreateDuplicateEntityCommand(cityPlanner.CityMapState.SelectedCityMapEntity.Id);
        commandManager.ExecuteCommand(cmd);

        return true;
    }

    public void AddNewCityMapEntity(BuildingGroup buildingGroup)
    {
        var cmd = commandFactory.CreateAddBuildingCommand(buildingGroup);
        commandManager.ExecuteCommand(cmd);
    }

    public void SelectCityMapEntityGroup()
    {
        cityPlanner.SelectGroup();
    }

    public Task DeleteStrategy()
    {
        Cleanup();
        return persistenceService.DeleteCityStrategy(Strategy.Id).AsTask();
    }

    public async Task Save()
    {
        if (_isReadOnly)
        {
            return;
        }

        UpdateCurrentLayoutItem();
        Strategy.Timeline = TimelineItems.ToList();
        UpdateStrategyAgeId();
        Strategy.UpdatedAt = DateTime.Now;
        await _saveSemaphore.WaitAsync();
        try
        {
            await persistenceService.SaveCityStrategy(Strategy);
            _savingRequested = false;
            logger.LogDebug("Saved city strategy");
        }
        finally
        {
            _saveSemaphore.Release();
        }
    }

    public void RenderScene(SKCanvas canvas)
    {
        cityPlanner.RenderScene(canvas);
    }

    public async Task CreateTimelineItemAsync(CityStrategyTimelineItemCreateRequest request)
    {
        CityStrategyTimelineItemBase newItem = request.Type switch
        {
            CityStrategyNewTimelineItemType.Description => AddNewDescriptionItem(request.ItemId),
            CityStrategyNewTimelineItemType.Research => AddNewResearchItem(request.ItemId),
            CityStrategyNewTimelineItemType.Layout => await AddNewLayoutItem(request.ItemId, request.ExistingCityId),
            CityStrategyNewTimelineItemType.LayoutImport => await AddNewLayoutItem(request.ItemId,
                request.ExistingCityId),
            _ => throw new ArgumentOutOfRangeException(),
        };

        await SelectTimelineItem(newItem);
    }

    public async Task DeleteTimelineItem(CityStrategyTimelineItemBase item)
    {
        if (TimelineItems.Count < 2)
        {
            return;
        }

        TimelineItems.Remove(item);
        await SelectTimelineItem(TimelineItems.Last());
    }

    public Task SelectTimelineItem(string id)
    {
        var item = TimelineItems.FirstOrDefault(x => x.Id == id);
        if (item == null)
        {
            throw new InvalidOperationException($"Item with id {id} not found.");
        }

        return SelectTimelineItem(item);
    }

    public void RequestSaving()
    {
        if (_isReadOnly)
        {
            return;
        }

        Interlocked.Exchange(ref _savingRequested, true);
    }

    public async Task<IReadOnlyCollection<HohCityBasicData>> GetCities()
    {
        var cities = await persistenceService.GetCities();
        var finalCities = new List<HohCityBasicData>();
        foreach (var basicData in cities.Where(x => x.InGameCityId == Strategy.InGameCityId))
        {
            var city = await persistenceService.LoadCity(basicData.Id);
            if (city!.WonderId == Strategy.WonderId)
            {
                finalCities.Add(basicData);
            }
        }

        return finalCities.ToList();
    }

    public void Dispose()
    {
        Cleanup();

        logger.LogDebug("Disposing CityStrategyUiService");
    }

    public Task SelectNextItem()
    {
        if (SelectedTimelineItem == null)
        {
            return SelectTimelineItem(TimelineItems.First());
        }

        var i = TimelineItems.IndexOf(SelectedTimelineItem);
        return SelectTimelineItem(i < TimelineItems.Count - 1 ? TimelineItems[i + 1] : TimelineItems.First());
    }

    public Task SelectPreviousItem()
    {
        if (SelectedTimelineItem == null)
        {
            return SelectTimelineItem(TimelineItems.Last());
        }

        var i = TimelineItems.IndexOf(SelectedTimelineItem);
        return SelectTimelineItem(i > 0 ? TimelineItems[i - 1] : TimelineItems.Last());
    }

    public async Task InitializeAsync(CityStrategy strategy, bool isReadOnly)
    {
        if (_isInitialized)
        {
            throw new InvalidOperationException("Already initialized.");
        }

        _isReadOnly = isReadOnly;

        Strategy = strategy;

        TimelineItems = new ObservableCollection<CityStrategyTimelineItemBase>(Strategy.Timeline);

        if (_isReadOnly)
        {
            CityStrategyTimelineLayoutItem? previousLayoutItem = null;
            foreach (var item in TimelineItems)
            {
                if (item is not CityStrategyTimelineLayoutItem li)
                {
                    continue;
                }

                if (previousLayoutItem == null)
                {
                    previousLayoutItem = li;
                    continue;
                }

                CreateLayoutDiff(previousLayoutItem, li);
                previousLayoutItem = li;
            }
        }

        await SelectTimelineItem(await GetInitialTimelineItemAsync(), false);

        analyticsService.TrackCityStrategyOpening(Strategy.Id, Strategy.InGameCityId, Strategy.WonderId, !_isReadOnly);

        commandManager.CommandExecuted += OnCommandExecuted;

        if (!_isReadOnly)
        {
            _autoSaveTimer = new Timer(AutoSaveInterval);
            _autoSaveTimer.Elapsed += OnAutoSaveTimerOnElapsed;
            _autoSaveTimer.Start();
        }

        _isInitialized = true;
    }

    private void CreateLayoutDiff(CityStrategyTimelineLayoutItem oldItem, CityStrategyTimelineLayoutItem newItem)
    {
        var oldItemMap = oldItem.Entities.ToDictionary(x => new Point(x.X, x.Y));
        foreach (var newItemEntity in newItem.Entities)
        {
            if (oldItemMap.TryGetValue(new Point(newItemEntity.X, newItemEntity.Y), out var oldItemEntity) &&
                oldItemEntity == newItemEntity)
            {
                newItemEntity.IsUnchanged = true;
            }
        }
    }

    private async Task<CityStrategyTimelineItemBase> GetInitialTimelineItemAsync()
    {
        CityStrategyTimelineItemBase? selectedItem = null;

        try
        {
            _selectionHistory = await persistenceService.GetItemAsync<Dictionary<string, string>>(PersistenceKeys
                .CITY_STRATEGY_LAST_SELECTED_ITEMS) ?? new Dictionary<string, string>();
            if (_selectionHistory.TryGetValue(Strategy.Id, out var selectedTimelineItemId))
            {
                selectedItem = TimelineItems.FirstOrDefault(x => x.Id == selectedTimelineItemId);
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to load last selected items.");
        }

        selectedItem ??= TimelineItems.First();

        return selectedItem;
    }

    private void Cleanup()
    {
        if (_autoSaveTimer != null)
        {
            _autoSaveTimer.Elapsed -= OnAutoSaveTimerOnElapsed;
            _autoSaveTimer.Dispose();
        }

        commandManager.CommandExecuted -= OnCommandExecuted;
        commandManager.Reset();
        _saveSemaphore.Dispose();
    }

    private void OnCommandExecuted()
    {
        if (!cityPlanner.ValidateLayout())
        {
            return;
        }

        RequestSaving();
    }

    private void OnAutoSaveTimerOnElapsed(object? sender, ElapsedEventArgs e)
    {
        _ = SaveIfNeededAsync();
    }

    private async Task SaveIfNeededAsync()
    {
        if (_savingRequested)
        {
            await Save();
        }
    }

    private async Task InitializeLayout(CityStrategyTimelineLayoutItem item)
    {
        var city = hohCityFactory.Create(item.Id, Strategy.InGameCityId, item.AgeId, item.Title, item.Entities,
            item.UnlockedExpansions, Strategy.CityPlannerVersion, Strategy.WonderId, item.WonderLevel);
        await cityPlanner.InitializeAsync(city);
    }

    private async Task SelectTimelineItem(CityStrategyTimelineItemBase item, bool save = true)
    {
        if (save)
        {
            await Save();
        }

        switch (item)
        {
            case CityStrategyTimelineResearchItem ri:
            {
                UpdateOpenedTechnologies(ri);
                break;
            }

            case CityStrategyTimelineLayoutItem li:
            {
                await InitializeLayout(li);
                break;
            }
        }

        SelectedTimelineItem = item;

        foreach (var i in TimelineItems)
        {
            if (i != item)
            {
                i.Selected = false;
            }
            else
            {
                i.Selected = true;
            }
        }

        _selectionHistory[Strategy.Id] = item.Id;
        _ = Task.Run(() =>
            persistenceService.SetItemAsync(PersistenceKeys.CITY_STRATEGY_LAST_SELECTED_ITEMS, _selectionHistory));
    }

    private void UpdateStrategyAgeId()
    {
        if (Strategy.InGameCityId != CityId.Capital)
        {
            return;
        }

        var layoutItem = TimelineItems.OfType<CityStrategyTimelineLayoutItem>().LastOrDefault();
        if (layoutItem != null)
        {
            Strategy.AgeId = layoutItem.AgeId;
        }
    }

    private void UpdateCurrentLayoutItem()
    {
        if (SelectedTimelineItem is not CityStrategyTimelineLayoutItem li)
        {
            return;
        }

        var city = cityPlanner.GetCity();
        var nextId = city.Entities.Max(cme => cme.Id) + 1;
        foreach (var hohCityMapEntity in city.Entities)
        {
            if (hohCityMapEntity.Id < 0)
            {
                hohCityMapEntity.Id = nextId;
                nextId++;
            }
        }

        li.AgeId = city.AgeId;
        li.WonderLevel = city.WonderLevel;
        li.Entities = city.Entities;
        li.UnlockedExpansions = city.UnlockedExpansions;
    }

    private CityStrategyTimelineResearchItem UpdateOpenedTechnologies(CityStrategyTimelineResearchItem item)
    {
        var alreadyOpened = new HashSet<string>();
        foreach (var ti in TimelineItems)
        {
            if (ti is CityStrategyTimelineResearchItem ri)
            {
                if (ti == item)
                {
                    break;
                }

                alreadyOpened.UnionWith(ri.Technologies);
            }
        }

        item.OpenedTechnologies = alreadyOpened;
        return item;
    }

    private CityStrategyTimelineDescriptionItem AddNewDescriptionItem(string id)
    {
        var i = GetInsertionIndex(id);
        var item = cityStrategyFactory.CreateTimelineDescriptionItem();
        TimelineItems.Insert(i, item);
        return item;
    }

    private async Task<CityStrategyTimelineLayoutItem> AddNewLayoutItem(string id, string? existingCityId = null)
    {
        var i = GetInsertionIndex(id);
        CityStrategyTimelineLayoutItem? item = null;
        if (existingCityId != null)
        {
            var city = await persistenceService.LoadCity(existingCityId);
            if (city != null)
            {
                item = cityStrategyFactory.CreateTimelineLayoutItem(city);
            }
            else
            {
                logger.LogError("City with id {ExistingCityId} not found.", existingCityId);
            }
        }
        else
        {
            for (var j = i - 1; j >= 0; j--)
            {
                if (TimelineItems[j] is CityStrategyTimelineLayoutItem ri)
                {
                    item = mapper.Map<CityStrategyTimelineLayoutItem>(ri);
                    item.Title = loc[FogResource.CityStrategy_TimelineLayoutItem_DefaultTitle];
                    break;
                }
            }
        }

        item ??= cityStrategyFactory.CreateTimelineLayoutItem(Strategy.InGameCityId, Strategy.WonderId);

        TimelineItems.Insert(i, item);

        return item;
    }

    private CityStrategyTimelineResearchItem AddNewResearchItem(string id)
    {
        var i = GetInsertionIndex(id);

        var item = cityStrategyFactory.CreateTimelineResearchItem();
        TimelineItems.Insert(i, item);

        return UpdateOpenedTechnologies(item);
    }

    private int GetInsertionIndex(string id)
    {
        int i;
        for (i = 0; i < TimelineItems.Count; i++)
        {
            if (TimelineItems[i].Id == id)
            {
                break;
            }
        }

        return i == TimelineItems.Count ? i : i + 1;
    }
}
