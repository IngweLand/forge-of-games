using Ingweland.Fog.Models.Hoh.Entities.City;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

public interface IMapAreaFactory
{
    MapArea Create(int expansionSize, IReadOnlyCollection<Expansion> expansions);
}
