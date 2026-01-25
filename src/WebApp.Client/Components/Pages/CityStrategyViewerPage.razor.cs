using Ingweland.Fog.Application.Client.Web.Analytics;
using Ingweland.Fog.Application.Client.Web.Analytics.Interfaces;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.WebApp.Client.Components.Pages.Abstractions;
using Microsoft.AspNetCore.Components;

namespace Ingweland.Fog.WebApp.Client.Components.Pages;

public partial class CityStrategyViewerPage : FogPageBase
{
    private CityStrategy? _strategy;

    [Inject]
    private ICityStrategyAnalyticsService AnalyticsService { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; }

    [Inject]
    private IPersistenceService PersistenceService { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        if (!OperatingSystem.IsBrowser())
        {
            return;
        }

        var parameters = new Dictionary<string, object>
        {
            {AnalyticsParams.CITY_STRATEGY_ID, StrategyId},
        };
        AnalyticsService.TrackEvent(AnalyticsEvents.VIEW_CITY_STRATEGY_INIT, parameters);

        _strategy = await PersistenceService.LoadCityStrategy(StrategyId);

        if (_strategy == null)
        {
            AnalyticsService.TrackEvent(AnalyticsEvents.VIEW_CITY_STRATEGY_ERROR, parameters);
            NavigationManager.NavigateTo(FogUrlBuilder.PageRoutes.CITY_STRATEGIES_DASHBOARD_PATH, false, true);
        }
    }
}
