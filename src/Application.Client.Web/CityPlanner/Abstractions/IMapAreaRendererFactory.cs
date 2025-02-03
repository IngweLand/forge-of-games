using Ingweland.Fog.Application.Client.Web.CityPlanner.Rendering;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

public interface IMapAreaRendererFactory
{
    MapAreaRenderer Create(MapArea mapArea);
}
