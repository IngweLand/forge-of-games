using System.Drawing;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

public interface IMapGrid
{
    int GridSize { get; }
    Point ScreenToGrid(float screenX, float screenY);
    int ScreenToGrid(float screen);
    Rectangle ScreenToGrid(RectangleF screen);
    Point GridToScreen(Point grid);
    int GridToScreen(int grid);
    Rectangle GridToScreen(Rectangle grid);
    Point ScreenToGrid(PointF screen);
    Point SnapToGrid(PointF point);
}
