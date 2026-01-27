using System.Diagnostics.CodeAnalysis;
using Ingweland.Fog.Application.Client.Web.Analytics;
using Ingweland.Fog.Application.Client.Web.Analytics.Interfaces;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.Localization;
using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.WebApp.Client.Components.Elements.CityPlanner.Stats;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using SkiaSharp;

namespace Ingweland.Fog.WebApp.Client.Components.Elements.CityPlanner;

public partial class CityViewerComponent : CityViewerBase
{
    private bool _cityPropertiesAreVisible;

    [Inject]
    private ICityPlannerAnalyticsService AnalyticsService { get; set; }

    [Inject]
    private ICityPlanner CityPlanner { get; set; }

    [Inject]
    private CityPlannerNavigationState CityPlannerNavigationState { get; set; }

    [Inject]
    private IPersistenceService PersistenceService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        if (!OperatingSystem.IsBrowser())
        {
            return;
        }

        if (CityPlannerNavigationState.City == null)
        {
            NavigationManager.NavigateTo(FogUrlBuilder.PageRoutes.BASE_CITY_PLANNER_PATH, false, true);
            return;
        }

        await CityPlanner.InitializeAsync(CityPlannerNavigationState.City);

        IsInitialized = true;

        StateHasChanged();

        var settings = await PersistenceService.GetUiSettingsAsync();

        if (!settings.CityViewerIntroMessageViewed)
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
            {AnalyticsParams.CITY_ID, CityPlannerNavigationState.City!.InGameCityId.ToString()},
        };
        if (CityPlannerNavigationState.City.WonderId != WonderId.Undefined)
        {
            eventParams.Add(AnalyticsParams.WONDER_ID, CityPlannerNavigationState.City.WonderId.ToString());
        }

        AnalyticsService.TrackEvent(AnalyticsEvents.OPEN_CITY_VIEWER, eventParams);
    }

    protected override async Task InteractiveCanvasOnPointerUp(PointerEventArgs args)
    {
        await base.InteractiveCanvasOnPointerUp(args);
        if (SkCanvasView == null)
        {
            return;
        }

        await OpenCityMapEntityProperties();
    }

    private async Task OpenCityMapEntityProperties()
    {
        if (CityPlanner.CityMapState.SelectedEntityViewModel == null)
        {
            return;
        }

        var options = GetDefaultDialogOptions();

        var parameters = new DialogParameters<CityMapEntityPropertiesDialog>
            {{d => d.Building, CityPlanner.CityMapState.SelectedEntityViewModel}};
        _ = await DialogService.ShowAsync<CityMapEntityPropertiesDialog>(null, parameters, options);
        CityPlanner.DeselectAll();
    }

    private static DialogOptions GetDefaultDialogOptions()
    {
        return new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            BackgroundClass = "dialog-blur-bg",
            Position = DialogPosition.TopCenter,
            CloseButton = true,
            CloseOnEscapeKey = true,
            NoHeader = true,
        };
    }

    protected override void RenderScene(SKCanvas canvas)
    {
        CityPlanner.RenderScene(canvas);
    }

    private void ToggleCityProperties(bool toggled)
    {
        _cityPropertiesAreVisible = toggled;
    }

    private void NavigateToDashboard()
    {
        NavigationManager.NavigateTo(FogUrlBuilder.PageRoutes.BASE_CITY_PLANNER_PATH, false, true);
    }
}
