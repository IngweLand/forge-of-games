using Ingweland.Fog.WebApp.Client.Components.Elements.CityPlanner.Stats;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace Ingweland.Fog.WebApp.Client.Components.Elements.CityStrategyBuilder.Viewer;

public partial class CityStrategyMobileViewerComponent : CityStrategyViewerComponentBase
{
    private ViewerState _currentState = ViewerState.Main;

    protected override async Task InteractiveCanvasOnPointerUp(PointerEventArgs args)
    {
        await base.InteractiveCanvasOnPointerUp(args);
        if (SkCanvasView == null)
        {
            return;
        }

        await OpenCityMapEntityProperties();
    }

    private void ToggleTimeline(bool toggled)
    {
        _currentState = _currentState == ViewerState.Timeline ? ViewerState.Main : ViewerState.Timeline;
        AppBarService.StateHasChanged(NavigationManager.Uri);
    }

    protected override async Task OnSelectTimelineItem(string itemId)
    {
        await CityStrategyBuilderService.SelectTimelineItem(itemId);
        _currentState = ViewerState.Main;
    }

    private void ToggleCityProperties(bool toggled)
    {
        _currentState = _currentState == ViewerState.CityProperties ? ViewerState.Main : ViewerState.CityProperties;
        AppBarService.StateHasChanged(NavigationManager.Uri);
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

    private async Task PreviousBtnOnClicked()
    {
        await CityStrategyBuilderService.SelectPreviousItem();
        AppBarService.StateHasChanged(NavigationManager.Uri);
    }

    private async Task NextBtnOnClicked()
    {
        await CityStrategyBuilderService.SelectNextItem();
        AppBarService.StateHasChanged(NavigationManager.Uri);
    }

    private enum ViewerState
    {
        Main,
        Timeline,
        CityProperties,
    }
}
