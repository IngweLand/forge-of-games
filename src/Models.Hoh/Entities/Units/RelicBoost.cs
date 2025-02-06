using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Units;

[ProtoContract]
public class RelicBoost
{
    [ProtoMember(1)]
    public required string RelicBoostAgeModifierId { get; init; }

    [ProtoMember(2)]
    public required UnitStat StatBoost { get; init; }
}
