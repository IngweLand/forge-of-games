using Ingweland.Fog.Models.Hoh.Entities.Units;
using ProtoBuf;

namespace Ingweland.Fog.Dtos.Hoh.Units;

[ProtoContract]
public class RelicBoostDto
{
    [ProtoMember(1)]
    public required IReadOnlyDictionary<string, float> AgeModifiers { get; init; } = new Dictionary<string, float>();

    [ProtoMember(2)]
    public required UnitStat StatBoost { get; init; }
}
