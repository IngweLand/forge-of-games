using System.Drawing;
using SkiaSharp;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

public interface ICityPlannerInteractionManager
{
    void OnPointerDown(float x, float y);
    bool OnPointerUp(float x, float y);
    bool OnPointerMove(float x, float y);
    bool Zoom(float x, float y, float deltaY);
    void FitToScreen(Size canvasSize);
    void TransformMapArea(SKCanvas canvas);
}
