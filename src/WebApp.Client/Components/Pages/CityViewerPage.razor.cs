using Ingweland.Fog.Application.Client.Web.Analytics;
using Ingweland.Fog.Application.Client.Web.Analytics.Interfaces;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.Localization;
using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.WebApp.Client.Components.Pages.Abstractions;
using Ingweland.Fog.WebApp.Client.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Ingweland.Fog.WebApp.Client.Components.Pages;

public partial class CityViewerPage : FogPageBase
{
    private HohCity? _city;
    private bool _isInitialized;
    private bool _isSmallScreen;

    [Inject]
    private IAnalyticsService AnalyticsService { get; set; }

    [Inject]
    public AppBarService AppBarService { get; set; }

    [Inject]
    private IBrowserViewportService BrowserViewportService { get; set; }

    [Inject]
    protected ICityPlanner CityPlanner { get; set; }

    [Inject]
    private CityPlannerNavigationState CityPlannerNavigationState { get; set; }

    [Inject]
    private IDialogService DialogService { get; set; }

    [Inject]
    private IJSInteropService JsInteropService { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; }

    [Inject]
    private IPersistenceService PersistenceService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (!OperatingSystem.IsBrowser())
        {
            return;
        }

        _city = CityPlannerNavigationState.City;
        if (_city == null)
        {
            NavigationManager.NavigateTo(FogUrlBuilder.PageRoutes.BASE_CITY_PLANNER_PATH, false, true);
            return;
        }

        StateHasChanged();

        await CityPlanner.InitializeAsync(_city);

        await JsInteropService.ResetScrollPositionAsync();

        var size = await BrowserViewportService.GetCurrentBrowserWindowSizeAsync();
        _isSmallScreen = size.Width < FogConstants.CITY_PLANNER_REQUIRED_SCREEN_WIDTH;

        _isInitialized = true;

        var settings = await PersistenceService.GetUiSettingsAsync();

        if (!settings.CityViewerIntroMessageViewed && _isSmallScreen)
        {
            _ = DialogService.ShowMessageBox(
                null,
                Loc[FogResource.CityViewer_FirstTimeMessage],
                Loc[FogResource.Common_Ok],
                options: new DialogOptions
                {
                    BackdropClick = false,
                    Position = DialogPosition.TopCenter,
                    NoHeader = true,
                });

            settings.CityViewerIntroMessageViewed = true;
            await PersistenceService.SaveUiSettingsAsync(settings);
        }

        TrackOpening();
    }

    private void TrackOpening()
    {
        var eventParams = new Dictionary<string, object>
        {
            {AnalyticsParams.CITY_ID, _city!.InGameCityId.ToString()},
        };
        if (_city.WonderId != WonderId.Undefined)
        {
            eventParams.Add(AnalyticsParams.WONDER_ID, _city.WonderId.ToString());
        }

        _ = AnalyticsService.TrackEvent(AnalyticsEvents.OPEN_CITY_VIEWER, eventParams);
    }

    private void OnEdit()
    {
        CityPlannerNavigationState.City = _city;
        NavigationManager.NavigateTo(FogUrlBuilder.PageRoutes.CITY_PLANNER_APP_PATH);
    }
}
