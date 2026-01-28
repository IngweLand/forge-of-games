namespace Ingweland.Fog.WebApp.Client.Components.Elements.CityPlanner.Viewer;

public partial class CityViewerComponent : CityViewerComponentBase
{
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
