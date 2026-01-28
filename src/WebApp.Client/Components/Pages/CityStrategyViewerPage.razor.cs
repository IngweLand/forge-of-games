using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.WebApp.Client.Components.Pages.Abstractions;
using Ingweland.Fog.WebApp.Client.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Ingweland.Fog.WebApp.Client.Components.Pages;

public partial class CityStrategyViewerPage : FogPageBase
{
    private bool _isSmallScreen;
    private CityStrategy? _strategy;

    [Inject]
    public AppBarService AppBarService { get; set; }

    [Inject]
    private IBrowserViewportService BrowserViewportService { get; set; }

    [Inject]
    public CityStrategyNavigationState CityStrategyNavigationState { get; set; }

    [Inject]
    private IJSInteropService JsInteropService { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; }

    [Inject]
    public IPersistenceService PersistenceService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (!OperatingSystem.IsBrowser())
        {
            return;
        }

        _strategy = CityStrategyNavigationState.Data?.Strategy;
        if (_strategy == null)
        {
            NavigationManager.NavigateTo(FogUrlBuilder.PageRoutes.CITY_STRATEGIES_DASHBOARD_PATH, false, true);
            return;
        }

        await JsInteropService.ResetScrollPositionAsync();
        await Task.Delay(30);

        var size = await BrowserViewportService.GetCurrentBrowserWindowSizeAsync();
        _isSmallScreen = size.Width < 1000;
    }

    private async Task OnEdit()
    {
        if (CityStrategyNavigationState.Data!.IsRemote)
        {
            CityStrategyNavigationState.Data.Strategy.Id = Guid.NewGuid().ToString();
            await PersistenceService.SaveCityStrategy(CityStrategyNavigationState.Data.Strategy);
        }

        NavigationManager.NavigateTo(FogUrlBuilder.PageRoutes.CITY_STRATEGY_BUILDER_APP_PATH);
    }
}
