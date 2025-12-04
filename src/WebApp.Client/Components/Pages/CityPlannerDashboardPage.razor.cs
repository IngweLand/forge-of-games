using Ingweland.Fog.Application.Client.Web.Analytics;
using Ingweland.Fog.Application.Client.Web.Analytics.Interfaces;
using Ingweland.Fog.Application.Client.Web.CityPlanner;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.WebApp.Client.Components.Elements.CityPlanner;
using Ingweland.Fog.WebApp.Client.Components.Pages.Abstractions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Ingweland.Fog.WebApp.Client.Components.Pages;

public partial class CityPlannerDashboardPage : FogPageBase
{
    private IReadOnlyCollection<HohCityBasicData> _cities = [];

    [Inject]
    private CityPlannerNavigationState CityPlannerNavigationState { get; set; }

    [Inject]
    private ICityPlannerUiService CityPlannerUiService { get; set; }

    [Inject]
    private IDialogService DialogService { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; }

    [Inject]
    public IPersistenceService PersistenceService { get; set; }
    
    [Inject]
    private ICityPlannerAnalyticsService AnalyticsService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (!OperatingSystem.IsBrowser())
        {
            return;
        }

        _cities = await PersistenceService.GetCities();
        
        AnalyticsService.TrackEvent(AnalyticsEvents.OPEN_CITY_PLANNER_DASHBOARD);
    }

    private void NavigateTo(string path)
    {
        NavigationManager.NavigateTo(path);
    }

    private async Task OpenCity(string cityId)
    {
        var city = await PersistenceService.LoadCity(cityId);
        if (city == null)
        {
            return;
        }

        CityPlannerNavigationState.City = city;
        NavigationManager.NavigateTo(FogUrlBuilder.PageRoutes.CITY_PLANNER_APP_PATH);
    }

    private async Task CreateNewCity()
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

        var city = CityPlannerUiService.CreateNew(newCityRequest);
        await PersistenceService.SaveCity(city);

        CityPlannerNavigationState.City = city;
        
        AnalyticsService.TrackCityCreation(newCityRequest);
        
        NavigationManager.NavigateTo(FogUrlBuilder.PageRoutes.CITY_PLANNER_APP_PATH);
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
