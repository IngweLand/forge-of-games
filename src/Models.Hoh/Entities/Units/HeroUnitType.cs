using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Units;

[ProtoContract]
public class HeroUnitType
{
    [ProtoMember(1)]
    public required IReadOnlyCollection<UnitStat> BaseValues { get; init; }
    [ProtoMember(2)]
    public required UnitType UnitType { get; init; }
}
