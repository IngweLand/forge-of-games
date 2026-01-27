using Ingweland.Fog.Application.Client.Web.CityStrategyBuilder.Abstractions;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.WebApp.Client.Models;
using Microsoft.AspNetCore.Components;
using SkiaSharp;

namespace Ingweland.Fog.WebApp.Client.Components.Elements.CityStrategyBuilder.Viewer;

public abstract class CityStrategyViewerComponentBase : LayoutViewerComponentBase
{
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
        }

        base.Dispose(disposing);
    }

    protected override async Task OnInitializedAsync()
    {
        if (!OperatingSystem.IsBrowser())
        {
            return;
        }

        var size = await BrowserViewportService.GetCurrentBrowserWindowSizeAsync();
        var appBarState = new AppBarState
        {
            Content = AppBarButtons,
            ShouldHideTitle = size.Width < 720,
        };
        AppBarService.SetState(NavigationManager.Uri, appBarState);

        await CityStrategyBuilderService.InitializeAsync(Strategy, true);

        CityStrategyBuilderService.StateHasChanged += CityPlannerOnStateHasHasChanged;
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

    protected override void RenderScene(SKCanvas canvas)
    {
        CityStrategyBuilderService.RenderScene(canvas);
    }

    protected virtual async Task OnSelectTimelineItem(string itemId)
    {
        await CityStrategyBuilderService.SelectTimelineItem(itemId);
        AppBarService.StateHasChanged(NavigationManager.Uri);
    }
}
