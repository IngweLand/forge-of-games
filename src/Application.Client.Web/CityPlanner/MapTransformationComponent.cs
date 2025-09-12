using System.Drawing;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using SkiaSharp;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner;

public class MapTransformationComponent : IMapTransformationComponent
{
    private const float MAX_SCALE = 3f;
    private const float MIN_SCALE = 0.2f;
    private const float ZOOM_STEP = 0.001f;
    public float Scale { get; private set; } = 1f;
    private SKPoint _location = SKPoint.Empty;

    public PointF GetTransformedCoordinates(float x, float y)
    {
        var canvasX = (x - _location.X) / Scale;
        var canvasY = (y - _location.Y) / Scale;
        return new PointF(canvasX, canvasY);
    }

    public bool CommitTranslate(SKPoint delta)
    {
        _location.Offset(delta);
        return true;
    }

    public bool CommitScale(float scaleOriginX, float scaleOriginY, float deltaY)
    {
        var scale = 1 + deltaY * -ZOOM_STEP;
        var newScale = Scale * (float) scale;
        //TODO: check if _scale has changed
        if (newScale < MIN_SCALE)
        {
            newScale = MIN_SCALE;
        }
        else if (newScale > MAX_SCALE)
        {
            newScale = MAX_SCALE;
        }

        var scaleRatio = newScale / Scale;
        Scale = newScale;

        _location = new SKPoint(scaleOriginX - scaleRatio * (scaleOriginX - _location.X),
            scaleOriginY - scaleRatio * (scaleOriginY - _location.Y));

        return true;
    }

    public void Transform(SKCanvas canvas)
    {
        canvas.Translate(_location);
        canvas.Scale(Scale);
    }

    public void FitToScreen(Rectangle targetBounds, Size containerSize, bool fitHeight = false)
    {
        var targetRect = targetBounds;
        targetRect.Inflate(10, 10);

        if (fitHeight)
        {
            Scale = (float) containerSize.Height / targetRect.Height;
        }
        else
        {
            var scaleX = (float) containerSize.Width / targetRect.Width;
            var scaleY = (float) containerSize.Height / targetRect.Height;
            Scale = Math.Min(scaleX, scaleY);
        }
        
        var x = (containerSize.Width - targetRect.Width * Scale) / 2f - targetRect.Left * Scale;
        var y = (containerSize.Height - targetRect.Height * Scale) / 2f - targetRect.Top * Scale;
        _location = new SKPoint(x, y);
    }
}
