using System.Diagnostics.CodeAnalysis;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.CityStrategyBuilder.Abstractions;
using Ingweland.Fog.Application.Client.Web.Localization;
using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.WebApp.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using SkiaSharp.Views.Blazor;
using Size = System.Drawing.Size;

namespace Ingweland.Fog.WebApp.Client.Components.Elements.CityStrategyBuilder.Viewer;

[SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
public partial class CityStrategyViewerComponent : ComponentBase, IDisposable
{
    private Size _canvasSize = Size.Empty;
    private bool _fitOnPaint = true;
    private bool _isInitialized;
    private bool _leftPanelIsVisible = true;
    private SKGLView? _skCanvasView;

    [Inject]
    public AppBarService AppBarService { get; set; }

    [Inject]
    private ICityStrategyBuilderService CityStrategyBuilderService { get; set; }

    [Inject]
    public ICityViewerInteractionManager CityViewerInteractionManager { get; set; }

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
        AppBarService.Clear();
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

    private void CityPlannerOnStateHasHasChanged()
    {
        if (_skCanvasView == null)
        {
            return;
        }

        _skCanvasView.Invalidate();
        StateHasChanged();
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

    private void InteractiveCanvasOnPointerUp(PointerEventArgs args)
    {
        if (_skCanvasView == null)
        {
            return;
        }

        CityViewerInteractionManager.OnPointerUp(args.PointerId, (float) args.OffsetX, (float) args.OffsetY);
        _skCanvasView!.Invalidate();
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

    private void ToggleLeftPanel(bool toggled)
    {
        _leftPanelIsVisible = toggled;
    }

    private void ZoomIn()
    {
        if (_skCanvasView == null)
        {
            return;
        }

        CityViewerInteractionManager.Zoom(_canvasSize.Width / 2, _canvasSize.Height / 2, -100);
        _skCanvasView.Invalidate();
    }

    private void ZoomOut()
    {
        if (_skCanvasView == null)
        {
            return;
        }

        CityViewerInteractionManager.Zoom(_canvasSize.Width / 2, _canvasSize.Height / 2, 100);
        _skCanvasView.Invalidate();
    }

    private async Task OnSelectTimelineItem(string itemId)
    {
        await CityStrategyBuilderService.SelectTimelineItem(itemId);
        AppBarService.StateHasChanged();
    }

    private void EditStrategy()
    {
        NavigationManager.NavigateTo(FogUrlBuilder.PageRoutes.CityStrategy(Strategy.Id));
    }
}
