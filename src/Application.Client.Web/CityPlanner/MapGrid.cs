using System.Drawing;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner;

public class MapGrid : IMapGrid
{
    public int GridSize => 32;

    public Point ScreenToGrid(float screenX, float screenY)
    {
        return new Point((int) Math.Floor(screenX / GridSize), (int) Math.Floor(screenY / GridSize));
    }
    
    public Point SnapToGrid(PointF point)
    {
        return new Point(
            (int) (MathF.Round(point.X / GridSize) * GridSize),
            (int) (MathF.Round(point.Y / GridSize) * GridSize)
        );
    }

    public Point ScreenToGrid(PointF screen)
    {
        return new Point((int) Math.Floor(screen.X / GridSize), (int) Math.Floor(screen.Y / GridSize));
    }

    public int ScreenToGrid(float screen)
    {
        return (int) Math.Floor(screen / GridSize);
    }

    public Rectangle ScreenToGrid(RectangleF screen)
    {
        return new Rectangle((int) Math.Floor(screen.X / GridSize), (int) Math.Floor(screen.Y / GridSize),
            (int) Math.Floor(screen.Width / GridSize), (int) Math.Floor(screen.Height / GridSize));
    }

    public Point GridToScreen(Point grid)
    {
        return new Point(grid.X * GridSize, grid.Y * GridSize);
    }

    public int GridToScreen(int grid)
    {
        return grid * GridSize;
    }

    public Rectangle GridToScreen(Rectangle grid)
    {
        return new Rectangle(
            grid.X * GridSize,
            grid.Y * GridSize,
            grid.Width * GridSize,
            grid.Height * GridSize
        );
    }
}
