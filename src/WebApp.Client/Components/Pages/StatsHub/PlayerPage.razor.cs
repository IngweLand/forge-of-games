using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Core.Helpers;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.StatsHub;

public partial class PlayerPage : StatsHubPageBase
{
    private bool _canShowChart;
    private PlayerWithRankingsViewModel? _player;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (OperatingSystem.IsBrowser())
        {
            _canShowChart = true;
            IsInitialized = true;
        }
    }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        if (_player == null || _player.Player.Id != PlayerId)
        {
            IsInitialized = false;
            _player = await LoadWithPersistenceAsync(nameof(_player),
                () => StatsHubUiService.GetPlayerAsync(PlayerId));
            IsInitialized = true;
        }
    }

    private void SearchAlliance()
    {
        NavigationManager.NavigateTo(
            FogUrlBuilder.PageRoutes.SearchAlliance(_player!.Player.WorldId, _player.Player.AllianceName!));
    }
    
    private void OnPlayerClicked(int playerId)
    {
        NavigationManager.NavigateTo(FogUrlBuilder.PageRoutes.Player(playerId));
    }
    
    private void OnHeroClicked(string heroId)
    {
        NavigationManager.NavigateTo(FogUrlBuilder.PageRoutes.HeroPlayground(heroId));
    }
}