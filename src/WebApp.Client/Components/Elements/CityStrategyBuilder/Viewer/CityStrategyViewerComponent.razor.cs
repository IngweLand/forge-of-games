namespace Ingweland.Fog.WebApp.Client.Components.Elements.CityStrategyBuilder.Viewer;

public partial class CityStrategyViewerComponent : CityStrategyViewerComponentBase
{
    private bool _leftPanelIsVisible = true;

    private void ToggleLeftPanel(bool toggled)
    {
        _leftPanelIsVisible = toggled;
    }

    private void ZoomIn()
    {
        if (SkCanvasView == null)
        {
            return;
        }

        CityViewerInteractionManager.Zoom(CanvasSize.Width / 2, CanvasSize.Height / 2, -100);
        SkCanvasView.Invalidate();
    }

    private void ZoomOut()
    {
        if (SkCanvasView == null)
        {
            return;
        }

        CityViewerInteractionManager.Zoom(CanvasSize.Width / 2, CanvasSize.Height / 2, 100);
        SkCanvasView.Invalidate();
    }
}
