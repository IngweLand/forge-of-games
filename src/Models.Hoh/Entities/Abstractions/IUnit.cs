using Ingweland.Fog.Models.Hoh.Entities.Units;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Models.Hoh.Entities.Abstractions;

public interface IUnit
{
    UnitColor Color { get; }
    IReadOnlyCollection<UnitStat> Stats { get; }
    UnitType Type { get; }
}
