using Ingweland.Fog.Application.Client.Web.StatsHub.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Microsoft.AspNetCore.Components;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.Abstractions;

public abstract class BattleLogPageBase : BattlePageBase
{
    protected BattleSearchRequest BattleSearchRequest { get; set; } = new();

    protected bool IsInitialized { get; set; }

    protected bool IsLoading { get; set; } = true;

    [Inject]
    protected NavigationManager NavigationManager { get; set; }

    protected BattleSelectorViewModel? SelectorViewModel { get; private set; }
    protected override bool ShouldHidePageLoadingIndicator => false;

    [Inject]
    protected IStatsHubUiService StatsHubUiService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (OperatingSystem.IsBrowser())
        {
            await JsInteropService.ShowLoadingIndicatorAsync();
            StateHasChanged();
        }

        SelectorViewModel = await LoadWithPersistenceAsync(
            nameof(SelectorViewModel),
            async () => await BattleUiService.GetBattleSelectorViewModel()
        );

        IsInitialized = OperatingSystem.IsBrowser();
        if (IsInitialized)
        {
            await JsInteropService.HideLoadingIndicatorAsync();
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        
        if (firstRender && IsInitialized)
        {
            await JsInteropService.HideLoadingIndicatorAsync();
        }
    }

    protected virtual Task BattleSelectorOnValueChanged(BattleSearchRequest newValue)
    {
        AnalyticsService.TrackFormChange(newValue, DefaultAnalyticsParameters);

        BattleSearchRequest = newValue;

        return Task.CompletedTask;
    }
}
