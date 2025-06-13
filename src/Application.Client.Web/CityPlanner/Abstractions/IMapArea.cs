using System.Drawing;
using Ingweland.Fog.Models.Hoh.Entities.City;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

public interface IMapArea
{
    Rectangle Bounds { get; }
    IReadOnlyCollection<Expansion> Expansions { get; }
    int ExpansionSize { get; }
    IEnumerable<CityMapExpansion> LockedExpansions { get; }
    bool Contains(Rectangle bounds);
    bool IntersectsWithBlocked(Rectangle bounds);
    bool IsOutside(Rectangle bounds);
    bool CanBePlaced(CityMapEntity cityMapEntity);
    CityMapExpansion? GetExpansion(Point coordinates);
}