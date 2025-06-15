using System.Drawing;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using SkiaSharp;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner;

public class CityPlannerInteractionManager(
    ICityPlanner cityPlanner,
    IMapTransformationComponent transformationComponent,
    IMapGrid grid,
    ICityMapEntityInteractionComponent cityMapEntityInteractionComponent) : ICityPlannerInteractionManager
{
    private const int TAP_SENSITIVITY = 5;
    private bool _canBeTapEvent;
    private bool _isDragging;
    private bool _isPanning;
    private SKPoint _lastPointerLocation;
    private bool _shouldProcessPointerMove;
    private bool _shouldProcessPointerUp;

    public void OnPointerDown(float x, float y)
    {
        _shouldProcessPointerUp = true;
        _shouldProcessPointerMove = true;
        _lastPointerLocation = new SKPoint(x, y);
        var transformedCoordinates = transformationComponent.GetTransformedCoordinates(x, y);
        if (cityMapEntityInteractionComponent.Start(transformedCoordinates))
        {
            _isDragging = true;
            return;
        }

        _canBeTapEvent = true;
    }

    public bool OnPointerUp(float x, float y)
    {
        if (!_shouldProcessPointerUp)
        {
            return false;
        }

        _shouldProcessPointerMove = false;
        _shouldProcessPointerUp = false;
        _isPanning = false;
        if (_isDragging)
        {
            cityMapEntityInteractionComponent.End();
            _isDragging = false;
        }

        if (_canBeTapEvent)
        {
            _canBeTapEvent = false;
            var location = grid.ScreenToGrid(transformationComponent.GetTransformedCoordinates(x, y));
            var consumed = cityPlanner.TrySelectCityMapEntity(location);
            return consumed || cityPlanner.TryToggleExpansion(location);
        }

        _canBeTapEvent = false;
        return false;
    }

    public bool OnPointerMove(float x, float y)
    {
        if (!_shouldProcessPointerMove)
        {
            return false;
        }

        var coordinates = new SKPoint(x, y);

        if (_isDragging)
        {
            var commited =
                cityMapEntityInteractionComponent.Drag(transformationComponent.GetTransformedCoordinates(x, y));
            if (commited)
            {
                _lastPointerLocation = coordinates;
            }

            // some hackish solution
            // return false even though the state has changed, because we fire StateHasChanged in the CityPlanner
            // this should be changed
            return false;
        }

        if (_canBeTapEvent && SKPoint.Distance(_lastPointerLocation, coordinates) > TAP_SENSITIVITY)
        {
            _canBeTapEvent = false;
        }

        if (_canBeTapEvent)
        {
            return false;
        }

        _isPanning = true;
        var delta = coordinates - _lastPointerLocation;
        _lastPointerLocation = coordinates;
        return transformationComponent.CommitTranslate(delta);
    }

    public bool Zoom(float x, float y, float deltaY)
    {
        return !_isPanning && transformationComponent.CommitScale(x, y, deltaY);
    }

    public void TransformMapArea(SKCanvas canvas)
    {
        transformationComponent.Transform(canvas);
    }

    public void FitToScreen(Size canvasSize)
    {
        transformationComponent.FitToScreen(cityPlanner.Bounds, canvasSize);
    }
}
