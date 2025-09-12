using System.Drawing;
using SkiaSharp;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

public interface IMapTransformationComponent
{
    PointF GetTransformedCoordinates(float x, float y);
    bool CommitTranslate(SKPoint delta);
    bool CommitScale(float x, float y, float deltaY);
    void Transform(SKCanvas canvas);
    void FitToScreen(Rectangle targetBounds, Size containerSize, bool fitHeight = false);
    float Scale { get; }
}
