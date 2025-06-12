using System.Drawing;
using Ingweland.Fog.Models.Hoh.Entities.City;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

public interface IMapArea
{
    IReadOnlyCollection<Expansion> LockedExpansions { get; }
    IReadOnlyCollection<Expansion> BlockedExpansions { get; }
    Rectangle Bounds { get; }
    int ExpansionSize { get; }
    IReadOnlyCollection<Expansion> Expansions { get; }
    bool Contains(Rectangle bounds);
    bool IntersectsWithBlocked(Rectangle bounds);
    bool IsOutside(Rectangle bounds);
    bool CanBePlaced(CityMapEntity cityMapEntity);
}
