using System.Drawing;
using Ingweland.Fog.Models.Hoh.Entities.City;

namespace Ingweland.Fog.Application.Core.CityPlanner.Abstractions;

public interface IMapArea
{
    Rectangle Bounds { get; }
    IReadOnlyCollection<Expansion> Expansions { get; }
    int ExpansionSize { get; }
    IEnumerable<CityMapExpansion> LockedExpansions { get; }
    IEnumerable<CityMapExpansion> OpenExpansions { get; }
    bool Contains(Rectangle bounds);
    bool IntersectsWithBlocked(Rectangle bounds);
    bool IsOutside(Rectangle bounds);
    bool CanBePlaced(CityMapEntity cityMapEntity);
    CityMapExpansion? GetExpansion(Point coordinates);
}
