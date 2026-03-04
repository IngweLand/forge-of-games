using Ingweland.Fog.Application.Client.Web.CityPlanner;
using Ingweland.Fog.Application.Client.Web.CityStrategyBuilder.Abstractions;
using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.AspNetCore.Components;
using SkiaSharp;

namespace Ingweland.Fog.WebApp.Client.Components.Elements.CityStrategyBuilder.Viewer;

public abstract class CityStrategyViewerComponentBase : LayoutViewerComponentBase
{
    [Inject]
    protected CityPlannerSettings CityPlannerSettings { get; set; }

    [Inject]
    protected ICityStrategyBuilderService CityStrategyBuilderService { get; set; }

    [Parameter]
    [EditorRequired]
    public required CityStrategy Strategy { get; set; }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            CityStrategyBuilderService.StateHasChanged -= CityPlannerOnStateHasHasChanged;
            CityStrategyBuilderService.Dispose();
            CityPlannerSettings.StateChanged -= CityPlannerSettingsOnStateChanged;
        }

        base.Dispose(disposing);
    }

    protected override async Task OnInitializedAsync()
    {
        if (!OperatingSystem.IsBrowser())
        {
            return;
        }

        await CityStrategyBuilderService.InitializeAsync(Strategy, true);
        CityPlannerSettings.StateChanged += CityPlannerSettingsOnStateChanged;
        CityStrategyBuilderService.StateHasChanged += CityPlannerOnStateHasHasChanged;
        IsInitialized = true;
    }

    private void CityPlannerSettingsOnStateChanged()
    {
        if (SkCanvasView == null)
        {
            return;
        }

        SkCanvasView.Invalidate();
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

    protected override void RenderScene(SKCanvas canvas)
    {
        CityStrategyBuilderService.RenderScene(canvas);
    }

    protected virtual async Task OnSelectTimelineItem(string itemId)
    {
        await CityStrategyBuilderService.SelectTimelineItem(itemId);
    }
}
