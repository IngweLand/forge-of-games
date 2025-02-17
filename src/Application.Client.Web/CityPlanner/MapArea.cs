using System.Drawing;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner;

public class MapArea : IMapArea
{
    private IList<Rectangle> _blockedBounds;
    private IList<Rectangle> _nonWaterSubtypeOpenExpansionBounds;
    private IList<Rectangle> _waterSubtypeOpenExpansionBounds;

    public MapArea(int expansionSize, IReadOnlyCollection<Expansion> expansions)
    {
        ExpansionSize = expansionSize;
        Expansions = expansions;
        BlockedExpansions = expansions.Where(e => e.Type == ExpansionType.Blocker).ToList();
        var allBlockedExpansions = BlockedExpansions
            .Concat(expansions.Where(e => e.Type == ExpansionType.Connector))
            .Concat(expansions.Where(e => e.Type == ExpansionType.DetachedConnector));
        _blockedBounds = allBlockedExpansions.Select(e => new Rectangle(e.X, e.Y, expansionSize, expansionSize))
            .ToList();
        ActiveExpansions = expansions.Where(e => e.Type != ExpansionType.Blocker).ToList();
        var openExpansions = expansions.Where(e => e.Type == ExpansionType.Undefined).ToList();
        _nonWaterSubtypeOpenExpansionBounds = openExpansions.Where(e => e.SubType != ExpansionSubType.Water)
            .Select(e => new Rectangle(e.X, e.Y, expansionSize, expansionSize))
            .ToList();
        _waterSubtypeOpenExpansionBounds = expansions.Where(e => e.Type != ExpansionType.Blocker && e.SubType == ExpansionSubType.Water)
            .Select(e => new Rectangle(e.X, e.Y, expansionSize, expansionSize))
            .ToList();

        var minX = 999999;
        var minY = 999999;
        var maxX = -999999;
        var maxY = -999999;
        foreach (var expansion in expansions)
        {
            minX = Math.Min(minX, expansion.X);
            minY = Math.Min(minY, expansion.Y);
            maxX = Math.Max(maxX, expansion.X);
            maxY = Math.Max(maxY, expansion.Y);
        }

        var height = maxY - minY + expansionSize;
        var width = maxX - minX + expansionSize;
        Bounds = new Rectangle(minX, minY, width, height);
    }

    public IReadOnlyCollection<Expansion> ActiveExpansions { get; }
    public IReadOnlyCollection<Expansion> BlockedExpansions { get; }
    public Rectangle Bounds { get; }
    public int ExpansionSize { get; }
    public IReadOnlyCollection<Expansion> Expansions { get; }

    public bool Contains(Rectangle bounds)
    {
        return Bounds.Contains(bounds);
    }

    public bool IsOutside(Rectangle bounds)
    {
        return !Bounds.Contains(bounds) && !Bounds.IntersectsWith(bounds);
    }

    public bool IntersectsWithBlocked(Rectangle bounds)
    {
        return _blockedBounds.Any(blockedExpansionBounds => blockedExpansionBounds.IntersectsWith(bounds));
    }

    public bool CanBePlaced(CityMapEntity cityMapEntity)
    {
        if (IsOutside(cityMapEntity.Bounds))
        {
            return true;
        }

        if (!Contains(cityMapEntity.Bounds))
        {
            return false;
        }

        if (IntersectsWithBlocked(cityMapEntity.Bounds))
        {
            return false;
        }

        if (cityMapEntity.ExpansionSubType == ExpansionSubType.Undefined &&
            _waterSubtypeOpenExpansionBounds.Any(eb => eb.IntersectsWith(cityMapEntity.Bounds)))
        {
            return false;
        }

        if (cityMapEntity.ExpansionSubType == ExpansionSubType.Water &&
            _nonWaterSubtypeOpenExpansionBounds.Any(eb => eb.IntersectsWith(cityMapEntity.Bounds)))
        {
            return false;
        }

        return true;
    }
}
