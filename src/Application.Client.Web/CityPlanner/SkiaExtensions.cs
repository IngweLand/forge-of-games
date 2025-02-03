using System.Drawing;
using SkiaSharp;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner;

public static class SkiaExtensions
{
    public static SKRect ToSKRect(this Rectangle rectangle)
    {
        return new SKRect(
            rectangle.X,
            rectangle.Y, 
            rectangle.X + rectangle.Width,
            rectangle.Y + rectangle.Height
        );
    }

    public static SKRect ToSKRect(this RectangleF rectangle)
    {
        return new SKRect(
            rectangle.X,
            rectangle.Y, 
            rectangle.X + rectangle.Width,
            rectangle.Y + rectangle.Height
        );
    }
    
    public static SKPoint ToSKPoint(this PointF point)
    {
        return new SKPoint(point.X, point.Y);
    }
}
