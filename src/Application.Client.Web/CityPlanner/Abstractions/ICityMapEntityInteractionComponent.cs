using System.Drawing;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

public interface ICityMapEntityInteractionComponent
{
    bool Start(PointF screenCoordinates);
    bool Drag(PointF screenDelta);
    void End();
}
