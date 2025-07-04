using Ingweland.Fog.Application.Client.Web.CityPlanner.Rendering;
using Ingweland.Fog.Application.Core.CityPlanner;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

public interface IMapAreaRendererFactory
{
    MapAreaRenderer Create(MapArea mapArea);
}
