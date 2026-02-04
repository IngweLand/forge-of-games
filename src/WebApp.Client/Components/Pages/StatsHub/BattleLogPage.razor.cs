using Ingweland.Fog.Application.Client.Core.Localization;
using Ingweland.Fog.Application.Client.Web.Analytics;
using Ingweland.Fog.Application.Client.Web.Constants;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
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

    [Inject]
    private IPersistenceService PersistenceService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (OperatingSystem.IsBrowser())
        {
            var savedRequest =
                await PersistenceService.GetItemAsync<BattleSearchRequest?>(PersistenceKeys.BATTLE_LOG_REQUEST);
            BattleSearchRequest = savedRequest ?? new BattleSearchRequest();
        }
    }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        if (BattleSearchRequestFactory.TryCreate(NavigationManager.Uri, out var request))
        {
            BattleSearchRequest = request;
        }

        await GetBattles();
    }

    protected override async Task BattleSelectorOnValueChanged(BattleSearchRequest newValue)
    {
        await base.BattleSelectorOnValueChanged(newValue);
        await PersistenceService.SetItemAsync(PersistenceKeys.BATTLE_LOG_REQUEST, newValue);
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
        StateHasChanged();
        
        if (_battlesCts != null)
        {
            await _battlesCts.CancelAsync();
        }
        
        _battlesCts = new CancellationTokenSource();

        try
        {
            _battles = await StatsHubUiService.SearchBattles(BattleSearchRequest, _battlesCts.Token);
            IsLoading = false;
        }
        catch (OperationCanceledException _)
        {
        }
        catch (ApiException apiEx) when (apiEx.InnerException is TaskCanceledException)
        {
            IsLoading = false;
        }
        catch (Exception e)
        {
            IsLoading = false;
            Console.Error.WriteLine(e);
        }
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
