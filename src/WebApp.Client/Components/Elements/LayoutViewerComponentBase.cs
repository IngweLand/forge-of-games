using System.Diagnostics.CodeAnalysis;
using Ingweland.Fog.Application.Client.Core.Localization;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.WebApp.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using MudBlazor;
using SkiaSharp;
using SkiaSharp.Views.Blazor;
using Size = System.Drawing.Size;

namespace Ingweland.Fog.WebApp.Client.Components.Elements;

[SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
public abstract class LayoutViewerComponentBase : ComponentBase, IDisposable
{
    private bool _fitOnPaint = true;
    protected RenderFragment? AppBarButtons;
    protected Size CanvasSize = Size.Empty;
    protected bool IsInitialized;
    protected SKGLViewComponent? SkCanvasViewComponent;

    [Inject]
    protected AppBarService AppBarService { get; set; }

    [Inject]
    protected IBrowserViewportService BrowserViewportService { get; set; }

    [Inject]
    protected ICityViewerInteractionManager CityViewerInteractionManager { get; set; }

    [Inject]
    protected IDialogService DialogService { get; set; }

    [Inject]
    protected IStringLocalizer<FogResource> Loc { get; set; }

    [Inject]
    protected NavigationManager NavigationManager { get; set; }

    protected SKGLView? SkCanvasView => SkCanvasViewComponent?.SkCanvasView;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            SkCanvasView?.Dispose();
            AppBarService.RemoveState(NavigationManager.Uri);
        }
    }

    protected void FitToScreen()
    {
        if (SkCanvasView == null)
        {
            return;
        }

        CityViewerInteractionManager.FitToScreen(CanvasSize);
        SkCanvasView.Invalidate();
    }

    protected void InteractiveCanvasOnPointerDown(PointerEventArgs args)
    {
        CityViewerInteractionManager.OnPointerDown(args.PointerId, (float) args.OffsetX, (float) args.OffsetY);
    }

    protected void InteractiveCanvasOnPointerMove(PointerEventArgs args)
    {
        if (SkCanvasView == null)
        {
            return;
        }

        if (args.Buttons != 1)
        {
            return;
        }

        CityViewerInteractionManager.OnPointerMove(args.PointerId, (float) args.OffsetX, (float) args.OffsetY);
        SkCanvasView.Invalidate();
    }

    protected virtual Task InteractiveCanvasOnPointerUp(PointerEventArgs args)
    {
        if (SkCanvasView == null)
        {
            return Task.CompletedTask;
        }

        CityViewerInteractionManager.OnPointerUp(args.PointerId, (float) args.OffsetX, (float) args.OffsetY);
        SkCanvasView.Invalidate();
        return Task.CompletedTask;
    }

    protected void InteractiveCanvasOnWheel(WheelEventArgs e)
    {
        if (SkCanvasView == null)
        {
            return;
        }

        CityViewerInteractionManager.Zoom((float) e.OffsetX, (float) e.OffsetY, (float) e.DeltaY);
        SkCanvasView.Invalidate();
    }

    protected void SkCanvasView_OnPaintSurface(SKPaintGLSurfaceEventArgs e)
    {
        var surface = e.Surface;
        var canvas = surface.Canvas;

        CanvasSize = new Size(e.Info.Width, e.Info.Height);

        if (_fitOnPaint)
        {
            CityViewerInteractionManager.FitToScreen(CanvasSize);
            _fitOnPaint = false;
        }

        CityViewerInteractionManager.TransformMapArea(canvas);

        RenderScene(canvas);
    }

    protected abstract void RenderScene(SKCanvas canvas);
}
