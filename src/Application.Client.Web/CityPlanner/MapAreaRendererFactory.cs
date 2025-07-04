using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Rendering;
using Ingweland.Fog.Application.Core.CityPlanner;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner;

public class MapAreaRendererFactory(IMapGrid grid) : IMapAreaRendererFactory
{
    public MapAreaRenderer Create(MapArea mapArea)
    {
        return new MapAreaRenderer(mapArea, grid, new MapStyle());
    }
}
