using Ingweland.Fog.Application.Client.Web.Localization;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Models.Fog;
using Ingweland.Fog.Shared.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.StatsHub;

public partial class WorldStatsPage : StatsHubPageBase
{
    private CancellationTokenSource? _cts;
    private bool _isLoading = true;
    private bool _isNavigatingAway;
    private int _pageNumber = 1;
    private MudTextField<string> _playerNameSearchField;
    private string? _playerNameSearchString;
    private PaginatedList<PlayerViewModel>? _players;
    private bool _searchFormIsExpanded;
    private string _title;

    protected override async Task OnParametersSetAsync()
    {
        if (_isNavigatingAway)
        {
            return;
        }

        await base.OnParametersSetAsync();

        _isLoading = true;

        _title = WorldId == "zz1"
            ? Loc[FogResource.StatsHub_Worlds_PlayerListTitle, FogResource.StatsHub_Worlds_BetaWorld]
            : Loc[FogResource.StatsHub_Worlds_PlayerListTitle, FogResource.StatsHub_Worlds_MainWorld];

        var pageNumber = 1;
        if (PageNumber is > 1)
        {
            pageNumber = PageNumber.Value;
        }
        
        try
        {
            _players = await LoadWithPersistenceAsync(GetPersistenceKey(), async () => await GetData(pageNumber));
        }
        catch (OperationCanceledException _)
        {
            return;
        }

        _pageNumber = pageNumber;
        _playerNameSearchString = PlayerName;
        if (!string.IsNullOrWhiteSpace(_playerNameSearchString))
        {
            _searchFormIsExpanded = true;
        }

        _isLoading = false;

        if (OperatingSystem.IsBrowser())
        {
            IsInitialized = true;
        }
    }

    private Task<PaginatedList<PlayerViewModel>> GetData(int pageNumber)
    {
        _cts?.Cancel();
        _cts = new CancellationTokenSource();

        return StatsHubUiService.GetStatsAsync(WorldId, pageNumber, PlayerName, _cts.Token);
    }

    private string GetPersistenceKey()
    {
        return $"{nameof(_players)}_{PageNumber}_{PlayerName}";
    }

    private void NavigateWithQuery(int pageNumber)
    {
        _cts?.Cancel();

        var query = new Dictionary<string, object?>()
        {
            {nameof(PageNumber), pageNumber},
            {nameof(PlayerName), _playerNameSearchString},
        };

        NavigationManager.NavigateTo(NavigationManager.GetUriWithQueryParameters(query), false);
    }

    private void OnPageChanged(int pageNumber)
    {
        if (pageNumber == PageNumber || (pageNumber == 1 && !PageNumber.HasValue))
        {
            return;
        }

        NavigateWithQuery(pageNumber);
    }

    private void OnPlayerClicked(PlayerViewModel player)
    {
        _cts?.Cancel();
        _isNavigatingAway = true;
        NavigationManager.NavigateTo(FogUrlBuilder.PageRoutes.Player(player.Key.WorldId, player.Key.InGamePlayerId));
    }

    private async Task Search()
    {
        if (await JsInteropService.IsMobileAsync())
        {
            await _playerNameSearchField.BlurAsync();
        }

        NavigateWithQuery(1);
    }

    private Task SearchTextFieldOnKeyDown(KeyboardEventArgs args)
    {
        if (args.Key == KeyboardKeys.Enter)
        {
            return Search();
        }

        return Task.CompletedTask;
    }

    private async Task ToggleSearchForm()
    {
        _searchFormIsExpanded = !_searchFormIsExpanded;
        if (_searchFormIsExpanded)
        {
            await _playerNameSearchField.FocusAsync();
        }
    }
}
