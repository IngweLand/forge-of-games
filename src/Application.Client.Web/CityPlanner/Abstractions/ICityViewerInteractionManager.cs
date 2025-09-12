using System.Drawing;
using SkiaSharp;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

public interface ICityViewerInteractionManager
{
    void OnPointerDown(long pointerId, float x, float y);
    void OnPointerUp(long pointerId, float x, float y);
    void OnPointerMove(long pointerId, float x, float y);
    void Zoom(float x, float y, float deltaY);
    void FitToScreen(Size canvasSize);
    void TransformMapArea(SKCanvas canvas);
}
