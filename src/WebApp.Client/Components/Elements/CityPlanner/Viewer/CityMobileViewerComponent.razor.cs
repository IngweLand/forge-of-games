using Ingweland.Fog.WebApp.Client.Components.Elements.CityPlanner.Stats;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace Ingweland.Fog.WebApp.Client.Components.Elements.CityPlanner.Viewer;

public partial class CityMobileViewerComponent : CityViewerComponentBase
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
            CloseButton = false,
            CloseOnEscapeKey = true,
            NoHeader = true,
        };
    }

    private async Task OpenCityMapEntityProperties()
    {
        if (CityPlanner.CityMapState.SelectedEntityViewModel == null)
        {
            return;
        }

        var options = GetDefaultDialogOptions();

        var parameters = new DialogParameters<CityMapEntityPropertiesDialog>
        {
            {d => d.Building, CityPlanner.CityMapState.SelectedEntityViewModel},
            {d => d.CityId, CityPlanner.CityMapState.InGameCityId},
        };
        _ = await DialogService.ShowAsync<CityMapEntityPropertiesDialog>(null, parameters, options);
        CityPlanner.DeselectAll();
    }

    private enum ViewerState
    {
        Main,
        CityProperties,
    }
}
