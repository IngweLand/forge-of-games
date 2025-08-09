using Ingweland.Fog.Application.Client.Web.Analytics;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Models.Fog;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.WebApp.Client.Components.Pages.Abstractions;
using Ingweland.Fog.WebApp.Client.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Refit;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.CommandCenter;

public partial class MyBattlesPage : BattleLogPageBase
{
    private const int DEBOUNCE_INTERVAL = 150;
    private static readonly ItemsProviderResult<BattleSummaryViewModel> EmptyResult = new([], 0);
    private CancellationTokenSource? _debounceCts;
    private bool _isAdvancedFilter;

    private int _itemHeight = 250;
    private ElementReference _itemsContainer;
    private BattleType _selectedBattleType = BattleType.Campaign;
    private Guid? _submissionId;
    private string _submissionIdString = string.Empty;
    private IReadOnlyCollection<UnitBattleTypeViewModel> _unitBattleTypes = [];
    private Virtualize<BattleSummaryViewModel> _virtualizeComponent = null!;

    protected override Dictionary<string, object> DefaultAnalyticsParameters { get; } = new()
    {
        {AnalyticsParams.LOCATION, AnalyticsParams.Values.Locations.MY_BATTLES},
    };

    [Inject]
    private IPersistenceService PersistenceService { get; set; }

    protected override async ValueTask DisposeAsyncCore()
    {
        if (_debounceCts != null)
        {
            await _debounceCts.CancelAsync();
        }

        await base.DisposeAsyncCore();
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (OperatingSystem.IsBrowser())
        {
            _unitBattleTypes = BattleUiService.GetUnitBattleTypes();
            
            _submissionId = await PersistenceService.GetItemAsync<Guid?>(PersistenceKeys.SUBMISSION_ID);
            if (_submissionId != null)
            {
                _submissionIdString = _submissionId.ToString()!;
                UpdateItemHeight(_selectedBattleType);
                StateHasChanged();
            }
        }
    }

    private async Task GetBattles()
    {
        await JsInteropService.ScrollTo(_itemsContainer, 0);
        await _virtualizeComponent.RefreshDataAsync();
        StateHasChanged();
    }

    private async ValueTask<ItemsProviderResult<BattleSummaryViewModel>> GetBattles(ItemsProviderRequest request)
    {
        if (_submissionId == null)
        {
            return EmptyResult;
        }

        if (_debounceCts != null)
        {
            await _debounceCts.CancelAsync();
        }

        IsLoading = true;
        StateHasChanged();

        _debounceCts = new CancellationTokenSource();
        try
        {
            await Task.Delay(DEBOUNCE_INTERVAL, _debounceCts.Token);

            PaginatedList<BattleSummaryViewModel> result;
            if (_isAdvancedFilter)
            {
                result = await BattleUiService.SearchBattles(new UserBattleSearchRequest
                {
                    SubmissionId = _submissionId.Value,
                    BattleType = BattleSearchRequest.BattleType,
                    SearchRequest = BattleSearchRequest,
                    StartIndex = request.StartIndex,
                    Count = request.Count,
                }, request.CancellationToken);
            }
            else
            {
                result = await BattleUiService.SearchBattles(new UserBattleSearchRequest
                {
                    SubmissionId = _submissionId.Value,
                    BattleType = _selectedBattleType,
                    StartIndex = request.StartIndex,
                    Count = request.Count,
                }, request.CancellationToken);
            }

            return new ItemsProviderResult<BattleSummaryViewModel>(result.Items, result.TotalCount);
        }
        catch (OperationCanceledException _)
        {
        }
        catch (ApiException apiEx) when (apiEx.InnerException is TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            await Console.Error.WriteLineAsync($"Unexpected error: {ex}");
        }
        finally
        {
            IsLoading = false;
            StateHasChanged();
        }

        return EmptyResult;
    }

    private async Task OnSubmissionIdInputChanged(string newValue)
    {
        _submissionIdString = newValue;

        if (Guid.TryParse(_submissionIdString, out var id))
        {
            _submissionId = id;
        }
        else
        {
            _submissionId = null;
        }

        StateHasChanged();
        var battleTask = GetBattles();
        await PersistenceService.SetItemAsync(PersistenceKeys.SUBMISSION_ID, _submissionId);
        await battleTask;
    }

    private Task BattleTypeOnChange(BattleType newValue)
    {
        if (_selectedBattleType == newValue)
        {
            return Task.CompletedTask;
        }

        _selectedBattleType = newValue;
        UpdateItemHeight(_selectedBattleType);
        return GetBattles();
    }

    protected override async Task BattleSelectorOnValueChanged(BattleSearchRequest newValue)
    {
        await base.BattleSelectorOnValueChanged(newValue);
        UpdateItemHeight(newValue.BattleType);
        await GetBattles();
    }

    private void UpdateItemHeight(BattleType battleType)
    {
        _itemHeight = battleType switch
        {
            BattleType.Pvp => 475,
            BattleType.HistoricBattle => 215,
            _ => 250,
        };
    }

    private Task OnFilterTypeChanged(bool newValue)
    {
        _isAdvancedFilter = newValue;
        return GetBattles();
    }

    private Task ClearSubmissionId()
    {
        return OnSubmissionIdInputChanged(string.Empty);
    }
}
