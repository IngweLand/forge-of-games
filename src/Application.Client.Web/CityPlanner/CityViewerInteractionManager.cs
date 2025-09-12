using System.Drawing;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using SkiaSharp;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner;

public class CityViewerInteractionManager(
    ICityPlanner cityPlanner,
    IMapTransformationComponent transformationComponent) : ICityViewerInteractionManager
{
    private readonly Dictionary<long, SKPoint> _activeTouches = new();
    private bool _isPinchToZoom;
    private SKPoint _lastPointerLocation;

    public void OnPointerDown(long pointerId, float x, float y)
    {
        if (_isPinchToZoom)
        {
            return;
        }

        _lastPointerLocation = new SKPoint(x, y);
        if (_activeTouches.Count < 2)
        {
            _activeTouches.Add(pointerId, _lastPointerLocation);
        }

        if (_activeTouches.Count > 1)
        {
            _isPinchToZoom = true;
        }
    }

    public void OnPointerUp(long pointerId, float x, float y)
    {
        _activeTouches.Remove(pointerId);
        if (_activeTouches.Count == 0)
        {
            _isPinchToZoom = false;
        }
    }

    public void OnPointerMove(long pointerId, float x, float y)
    {
        var coordinates = new SKPoint(x, y);
        if (!_isPinchToZoom)
        {
            var delta = coordinates - _lastPointerLocation;
            _lastPointerLocation = coordinates;
            transformationComponent.CommitTranslate(delta);
        }
        else
        {
            var previousDistance = SKPoint.Distance(_activeTouches.First().Value, _activeTouches.Last().Value);
            _activeTouches[pointerId] = coordinates;
            var firstPoint = _activeTouches.First().Value;
            var secondPoint = _activeTouches.Last().Value;
            var currentDistance = SKPoint.Distance(firstPoint, secondPoint);
            var distanceDelta = (currentDistance - previousDistance) * 5 * -1;
            var centerX = (firstPoint.X + secondPoint.X) / 2;
            var centerY = (firstPoint.Y + secondPoint.Y) / 2;
            Zoom(centerX, centerY, distanceDelta);
        }
    }

    public void Zoom(float x, float y, float deltaY)
    {
        Console.Out.WriteLine(deltaY);
        transformationComponent.CommitScale(x, y, deltaY);
    }

    public void TransformMapArea(SKCanvas canvas)
    {
        transformationComponent.Transform(canvas);
    }

    public void FitToScreen(Size canvasSize)
    {
        transformationComponent.FitToScreen(cityPlanner.Bounds, canvasSize, true);
    }
}
