using System.Drawing;
using Ingweland.Fog.Models.Hoh.Entities.City;
using CityMapEntity = Ingweland.Fog.Application.Core.CityPlanner.CityMapEntity;
using CityMapExpansion = Ingweland.Fog.Application.Core.CityPlanner.CityMapExpansion;

namespace Ingweland.Fog.Application.Core.CityPlanner.Abstractions;

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