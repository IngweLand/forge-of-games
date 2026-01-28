using Ingweland.Fog.Application.Client.Web.Analytics.Interfaces;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.WebApp.Client.Models;
using Microsoft.AspNetCore.Components;
using SkiaSharp;

namespace Ingweland.Fog.WebApp.Client.Components.Elements.CityPlanner.Viewer;

public class CityViewerComponentBase : LayoutViewerComponentBase
{
    [Inject]
    protected ICityPlannerAnalyticsService AnalyticsService { get; set; }

    [Inject]
    protected ICityPlanner CityPlanner { get; set; }

    protected override void RenderScene(SKCanvas canvas)
    {
        CityPlanner.RenderScene(canvas);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            CityPlanner.StateHasChanged -= CityPlannerOnStateHasHasChanged;
        }

        base.Dispose(disposing);
    }

    protected override async Task OnInitializedAsync()
    {
        if (!OperatingSystem.IsBrowser())
        {
            return;
        }

        var appBarState = new AppBarState
        {
            Content = AppBarButtons,
        };
        AppBarService.SetState(NavigationManager.Uri, appBarState);

        CityPlanner.StateHasChanged += CityPlannerOnStateHasHasChanged;
        AppBarService.StateHasChanged(NavigationManager.Uri);
        IsInitialized = true;
    }

    private void CityPlannerOnStateHasHasChanged()
    {
        if (SkCanvasView == null)
        {
            return;
        }

        SkCanvasView.Invalidate();
        StateHasChanged();
    }
}
