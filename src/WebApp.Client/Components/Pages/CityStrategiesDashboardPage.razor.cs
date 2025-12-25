using Ingweland.Fog.Application.Client.Web.Analytics;
using Ingweland.Fog.Application.Client.Web.Analytics.Interfaces;
using Ingweland.Fog.Application.Client.Web.CityStrategyBuilder.Abstractions;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Core.CityPlanner;
using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.WebApp.Client.Components.Elements.CityPlanner;
using Ingweland.Fog.WebApp.Client.Components.Pages.Abstractions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Ingweland.Fog.WebApp.Client.Components.Pages;

public partial class CityStrategiesDashboardPage : FogPageBase
{
    private IReadOnlyCollection<HohCityBasicData> _cities = [];

    [Inject]
    private ICityStrategyAnalyticsService AnalyticsService { get; set; }

    [Inject]
    private ICityStrategyUiService CityStrategyUiService { get; set; }

    [Inject]
    private IDialogService DialogService { get; set; }

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

        _cities = await PersistenceService.GetCityStrategies();

        AnalyticsService.TrackEvent(AnalyticsEvents.OPEN_CITY_STRATEGIES_DASHBOARD);
    }

    private void OpenStrategy(string strategyId)
    {
        NavigationManager.NavigateTo(FogUrlBuilder.PageRoutes.CityStrategy(strategyId));
    }

    private async Task CreateStrategy()
    {
        var options = GetDefaultDialogOptions();
        var dialog = await DialogService.ShowAsync<CreateNewCityDialog>(null, options);
        var result = await dialog.Result;
        if (result == null || result.Canceled)
        {
            return;
        }

        if (result.Data is not NewCityRequest newCityRequest)
        {
            return;
        }

        var strategy = CityStrategyUiService.CreateCityStrategy(newCityRequest);
        await PersistenceService.SaveCityStrategy(strategy);

        AnalyticsService.TrackCityStrategyCreation(newCityRequest);

        NavigationManager.NavigateTo(FogUrlBuilder.PageRoutes.CityStrategy(strategy.Id));
    }

    private static DialogOptions GetDefaultDialogOptions(bool closeButton = false)
    {
        return new DialogOptions
        {
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            BackgroundClass = "dialog-blur-bg",
            CloseButton = closeButton,
        };
    }
}
