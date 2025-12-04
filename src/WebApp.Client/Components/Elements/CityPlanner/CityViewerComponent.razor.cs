using System.Diagnostics.CodeAnalysis;
using Ingweland.Fog.Application.Client.Web.Analytics;
using Ingweland.Fog.Application.Client.Web.Analytics.Interfaces;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.Localization;
using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using MudBlazor;
using SkiaSharp.Views.Blazor;
using Size = System.Drawing.Size;

namespace Ingweland.Fog.WebApp.Client.Components.Elements.CityPlanner;

[SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
public partial class CityViewerComponent : ComponentBase, IDisposable
{
    private Size _canvasSize = Size.Empty;
    private bool _cityPropertiesAreVisible;
    private bool _fitOnPaint = true;

    private bool _isInitialized;
    private SKGLView _skCanvasView;

    [Inject]
    private ICityPlannerAnalyticsService AnalyticsService { get; set; }

    [Inject]
    public ICityPlanner CityPlanner { get; set; }

    [Inject]
    private CityPlannerNavigationState CityPlannerNavigationState { get; set; }

    [Inject]
    public ICityViewerInteractionManager CityViewerInteractionManager { get; set; }

    [Inject]
    private IDialogService DialogService { get; set; }

    [Inject]
    public IStringLocalizer<FogResource> Loc { get; set; }

    [Inject]
    protected NavigationManager NavigationManager { get; set; }

    [Inject]
    public IPersistenceService PersistenceService { get; set; }

    public void Dispose()
    {
        _skCanvasView?.Dispose();
    }

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

        _isInitialized = true;

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

    private void FitToScreen()
    {
        CityViewerInteractionManager.FitToScreen(_canvasSize);
        _skCanvasView!.Invalidate();
    }

    private void InteractiveCanvasOnPointerDown(PointerEventArgs args)
    {
        CityViewerInteractionManager.OnPointerDown(args.PointerId, (float) args.OffsetX, (float) args.OffsetY);
    }

    private void InteractiveCanvasOnPointerMove(PointerEventArgs args)
    {
        if (args.Buttons != 1)
        {
            return;
        }

        CityViewerInteractionManager.OnPointerMove(args.PointerId, (float) args.OffsetX, (float) args.OffsetY);
        _skCanvasView!.Invalidate();
    }

    private void InteractiveCanvasOnPointerUp(PointerEventArgs args)
    {
        CityViewerInteractionManager.OnPointerUp(args.PointerId, (float) args.OffsetX, (float) args.OffsetY);
        _skCanvasView!.Invalidate();
    }

    private void InteractiveCanvasOnWheel(WheelEventArgs e)
    {
        CityViewerInteractionManager.Zoom((float) e.OffsetX, (float) e.OffsetY, (float) e.DeltaY);
        _skCanvasView!.Invalidate();
    }

    private void SkCanvasView_OnPaintSurface(SKPaintGLSurfaceEventArgs e)
    {
        var surface = e.Surface;
        var canvas = surface.Canvas;
        _canvasSize = new Size(e.Info.Width, e.Info.Height);
        if (_fitOnPaint)
        {
            CityViewerInteractionManager.FitToScreen(_canvasSize);
            _fitOnPaint = false;
        }

        CityViewerInteractionManager.TransformMapArea(canvas);
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
