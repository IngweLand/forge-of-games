using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Inspirations;
using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Application.Core.Interfaces;
using Ingweland.Fog.Dtos.Hoh.PlayerCity;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Fog.Enums;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.WebApp.Client.Components.Pages.Abstractions;
using Ingweland.Fog.WebApp.Client.Models;
using Microsoft.AspNetCore.Components;

namespace Ingweland.Fog.WebApp.Client.Components.Pages;

public partial class InspirationsPage : FogPageBase, IAsyncDisposable
{
    private CancellationTokenSource _cts = new();
    private IReadOnlyCollection<PlayerCitySnapshotBasicViewModel> _inspirations = [];
    private bool _isDisposed;
    private bool _isLoading;
    private CityInspirationsSearchFormViewModel? _searchFormViewModel;
    private CityInspirationsSearchFormRequest? _searchRequest;

    [Inject]
    public ICityInspirationsUiService CityInspirationsUiService { get; set; }

    [Inject]
    private CityPlannerNavigationState CityPlannerNavigationState { get; set; }

    [Inject]
    public ICityExpansionsHasher ExpansionsHasher { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; }

    [Inject]
    public IPersistenceService PersistenceService { get; set; }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore();
        GC.SuppressFinalize(this);
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (!OperatingSystem.IsBrowser())
        {
            return;
        }

        _searchFormViewModel = await CityInspirationsUiService.GetSearchFormDataAsync();
        _searchRequest = new CityInspirationsSearchFormRequest
        {
            Age = _searchFormViewModel.Ages.FirstOrDefault(),
            SearchPreference = _searchFormViewModel.SearchPreferences.FirstOrDefault(),
        };
    }

    protected virtual async ValueTask DisposeAsyncCore()
    {
        if (_isDisposed)
        {
            return;
        }

        _isDisposed = true;

        await _cts.CancelAsync();
        _cts.Dispose();
    }

    private async Task SearchFormOnSearchClick()
    {
        string? expansionsHash = null;
        if (_searchRequest?.City != null)
        {
            var city = await PersistenceService.LoadCity(_searchRequest.City.Id);
            if (city != null)
            {
                expansionsHash = ExpansionsHasher.Compute(city.UnlockedExpansions);
            }
        }

        var request = new CityInspirationsSearchRequest
        {
            AgeId = _searchRequest!.Age!.Id,
            CityId = CityId.Capital,
            AllowPremiumEntities = _searchRequest.AllowPremium,
            SearchPreference = _searchRequest.SearchPreference?.Value ?? CitySnapshotSearchPreference.Food,
            OpenedExpansionsHash = expansionsHash,
        };

        await GetBattles(request);
    }

    private async Task GetBattles(CityInspirationsSearchRequest request)
    {
        if (_isDisposed)
        {
            return;
        }

        _isLoading = true;
        _inspirations = [];

        await _cts.CancelAsync();
        _cts.Dispose();

        if (_isDisposed)
        {
            return;
        }

        _cts = new CancellationTokenSource();

        try
        {
            _inspirations = await CityInspirationsUiService.GetInspirationsAsync(request, _cts.Token);
        }
        catch
        {
            // ignored
        }

        if (_isDisposed)
        {
            return;
        }

        _isLoading = false;
    }

    private async Task<HohCity?> GetCity(int snapshotId)
    {
        if (_isDisposed)
        {
            return null;
        }

        _isLoading = true;
        _inspirations = [];

        await _cts.CancelAsync();
        _cts.Dispose();

        if (_isDisposed)
        {
            return null;
        }

        _cts = new CancellationTokenSource();

        HohCity? city = null;
        try
        {
            city = await CityInspirationsUiService.GetPlayerCitySnapshotAsync(snapshotId, _cts.Token);
        }
        catch
        {
            // ignored
        }

        if (_isDisposed)
        {
            return null;
        }

        _isLoading = false;
        return city;
    }

    private async Task OpenCity(int snapshotId)
    {
        var city = await GetCity(snapshotId);
        if (city == null)
        {
            return;
        }

        CityPlannerNavigationState.City = city;
        NavigationManager.NavigateTo(FogUrlBuilder.PageRoutes.CITY_PLANNER_APP_PATH);
    }
}
