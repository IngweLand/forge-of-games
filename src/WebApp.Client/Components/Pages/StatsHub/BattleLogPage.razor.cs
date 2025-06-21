using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Microsoft.AspNetCore.Components;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.StatsHub;

public partial class BattleLogPage : StatsHubPageBase, IAsyncDisposable
{
    private IReadOnlyCollection<BattleSummaryViewModel> _battles = [];
    private BattleSearchRequest _battleSearchRequest = new();

    private BattleSelectorViewModel? _battleSelectorViewModel;

    private CancellationTokenSource _cts = new CancellationTokenSource();

    private bool _isLoading = true;

    [Inject]
    private IBattleSearchRequestFactory BattleSearchRequestFactory { get; set; }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore();
        GC.SuppressFinalize(this);
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        _battleSelectorViewModel = await LoadWithPersistenceAsync(
            nameof(_battleSelectorViewModel),
            async () => await StatsHubUiService.GetBattleSelectorViewModel()
        );
    }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        await GetBattles(BattleSearchRequestFactory.Create(NavigationManager.Uri));
    }

    protected virtual async ValueTask DisposeAsyncCore()
    {
        await _cts.CancelAsync();
        _cts.Dispose();
    }

    private async Task GetBattles(BattleSearchRequest request)
    {
        _isLoading = true;

        _battles = [];

        await _cts.CancelAsync();
        _cts.Dispose();

        _cts = new CancellationTokenSource();

        _battleSearchRequest = request;

        try
        {
            _battles = await StatsHubUiService.SearchBattles(request, _cts.Token);
        }
        catch
        {
            // ignored
        }

        _isLoading = false;
    }

    private Task BattleSelectorOnValueChanged(BattleSearchRequest newValue)
    {
        return GetBattles(newValue);
    }

    private void OnContributionPromptClick()
    {
        NavigationManager.NavigateTo(FogUrlBuilder.PageRoutes.HELP_STATS_HUB_PATH);
    }
}
