using System.Diagnostics.CodeAnalysis;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.CityStrategyBuilder.Abstractions;
using Ingweland.Fog.Application.Client.Web.Localization;
using Ingweland.Fog.WebApp.Client.Components.Elements.CityPlanner.Stats;
using Ingweland.Fog.WebApp.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using MudBlazor;
using SkiaSharp.Views.Blazor;
using Size = System.Drawing.Size;

namespace Ingweland.Fog.WebApp.Client.Components.Elements.CityStrategyBuilder.Viewer;

[SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
public partial class CityStrategyMobileViewerComponent : ComponentBase, IDisposable
{
    private Size _canvasSize = Size.Empty;

    private ViewerState _currentState = ViewerState.Main;
    private bool _fitOnPaint = true;
    private bool _isInitialized;
    private SKGLView? _skCanvasView;

    [Inject]
    public AppBarService AppBarService { get; set; }

    [Inject]
    private ICityStrategyBuilderService CityStrategyBuilderService { get; set; }

    [Inject]
    public ICityViewerInteractionManager CityViewerInteractionManager { get; set; }

    [Inject]
    private IDialogService DialogService { get; set; }

    [Inject]
    private IStringLocalizer<FogResource> Loc { get; set; }

    [Inject]
    private ILogger<CityStrategyBuilderComponent> Logger { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; }

    public void Dispose()
    {
        _skCanvasView?.Dispose();
        CityStrategyBuilderService.StateHasChanged -= CityPlannerOnStateHasHasChanged;
        CityStrategyBuilderService.Dispose();
        AppBarService.Reset();
    }

    protected override async Task OnInitializedAsync()
    {
        if (!OperatingSystem.IsBrowser())
        {
            return;
        }

        await CityStrategyBuilderService.InitializeAsync(Strategy, true);

        CityStrategyBuilderService.StateHasChanged += CityPlannerOnStateHasHasChanged;
        AppBarService.StateHasChanged();
        _isInitialized = true;
    }

    private void FitToScreen()
    {
        if (_skCanvasView == null)
        {
            return;
        }

        CityViewerInteractionManager.FitToScreen(_canvasSize);
        _skCanvasView!.Invalidate();
    }

    private void InteractiveCanvasOnPointerDown(PointerEventArgs args)
    {
        CityViewerInteractionManager.OnPointerDown(args.PointerId, (float) args.OffsetX, (float) args.OffsetY);
    }

    private void InteractiveCanvasOnPointerMove(PointerEventArgs args)
    {
        if (_skCanvasView == null)
        {
            return;
        }

        if (args.Buttons != 1)
        {
            return;
        }

        CityViewerInteractionManager.OnPointerMove(args.PointerId, (float) args.OffsetX, (float) args.OffsetY);
        _skCanvasView!.Invalidate();
    }

    private Task InteractiveCanvasOnPointerUp(PointerEventArgs args)
    {
        if (_skCanvasView == null)
        {
            return Task.CompletedTask;
        }

        CityViewerInteractionManager.OnPointerUp(args.PointerId, (float) args.OffsetX, (float) args.OffsetY);
        _skCanvasView!.Invalidate();
        return OpenCityMapEntityProperties();
    }

    private void InteractiveCanvasOnWheel(WheelEventArgs e)
    {
        if (_skCanvasView == null)
        {
            return;
        }

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
        CityStrategyBuilderService.RenderScene(canvas);
    }

    private void ToggleTimeline(bool toggled)
    {
        _currentState = _currentState == ViewerState.Timeline ? ViewerState.Main : ViewerState.Timeline;
        AppBarService.StateHasChanged();
    }

    private async Task OnSelectTimelineItem(string itemId)
    {
        await CityStrategyBuilderService.SelectTimelineItem(itemId);
        _currentState = ViewerState.Main;
        AppBarService.StateHasChanged();
    }

    private void ToggleCityProperties(bool toggled)
    {
        _currentState = _currentState == ViewerState.CityProperties ? ViewerState.Main : ViewerState.CityProperties;
        AppBarService.StateHasChanged();
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

    private async Task OpenCityMapEntityProperties()
    {
        if (CityStrategyBuilderService.CityMapState.SelectedEntityViewModel == null)
        {
            return;
        }

        var options = GetDefaultDialogOptions();

        var parameters = new DialogParameters<CityMapEntityPropertiesDialog>
        {
            {d => d.Building, CityStrategyBuilderService.CityMapState.SelectedEntityViewModel},
            {d => d.CityId, CityStrategyBuilderService.CityMapState.InGameCityId},
        };
        _ = await DialogService.ShowAsync<CityMapEntityPropertiesDialog>(null, parameters, options);
        CityStrategyBuilderService.DeselectAll();
    }

    private void CityPlannerOnStateHasHasChanged()
    {
        if (_skCanvasView == null)
        {
            return;
        }

        _skCanvasView.Invalidate();
        StateHasChanged();
    }

    private async Task PreviousBtnOnClicked()
    {
        await CityStrategyBuilderService.SelectPreviousItem();
        AppBarService.StateHasChanged();
    }

    private async Task NextBtnOnClicked()
    {
        await CityStrategyBuilderService.SelectNextItem();
        AppBarService.StateHasChanged();
    }

    private enum ViewerState
    {
        Main,
        Timeline,
        CityProperties,
    }
}
