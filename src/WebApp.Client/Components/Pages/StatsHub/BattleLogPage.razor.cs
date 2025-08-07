using Ingweland.Fog.Application.Client.Web.Analytics;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Localization;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.WebApp.Client.Components.Elements.StatsHub;
using Ingweland.Fog.WebApp.Client.Components.Pages.Abstractions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Refit;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.StatsHub;

public partial class BattleLogPage : BattleLogPageBase
{
    private IReadOnlyCollection<BattleSummaryViewModel> _battles = [];

    private CancellationTokenSource? _battlesCts;

    [Inject]
    private IBattleSearchRequestFactory BattleSearchRequestFactory { get; set; }

    protected override Dictionary<string, object> DefaultAnalyticsParameters { get; } = new()
    {
        {AnalyticsParams.LOCATION, AnalyticsParams.Values.Locations.BATTLE_LOG},
    };

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        BattleSearchRequest = BattleSearchRequestFactory.Create(NavigationManager.Uri);
        await GetBattles();
    }

    protected override async Task BattleSelectorOnValueChanged(BattleSearchRequest newValue)
    {
        await base.BattleSelectorOnValueChanged(newValue);
        await GetBattles();
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        if (_battlesCts != null)
        {
            await _battlesCts.CancelAsync();
        }

        await base.DisposeAsyncCore();
    }

    protected virtual async Task GetBattles()
    {
        IsLoading = true;
        _battles = [];

        if (_battlesCts != null)
        {
            await _battlesCts.CancelAsync();
        }

        _battlesCts = new CancellationTokenSource();

        try
        {
            _battles = await StatsHubUiService.SearchBattles(BattleSearchRequest, _battlesCts.Token);
        }
        catch (OperationCanceledException _)
        {
        }
        catch (ApiException apiEx) when (apiEx.InnerException is TaskCanceledException)
        {
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e);
        }

        IsLoading = false;
    }

    private void OnContributionPromptClick()
    {
        NavigationManager.NavigateTo(FogUrlBuilder.PageRoutes.HELP_BATTLE_LOG_PATH);
    }

    private async Task ShareLink()
    {
        var options = GetDefaultDialogOptions();
        var parameters = new DialogParameters<ShareBattleLogLinkDialog>
        {
            {x => x.Request, BattleSearchRequest},
        };
        await DialogService.ShowAsync<ShareBattleLogLinkDialog>(Loc[FogResource.Common_Share],
            parameters, options);
    }
}
