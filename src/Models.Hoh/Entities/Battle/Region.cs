using Ingweland.Fog.Models.Hoh.Entities.Rewards;
using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Battle;

[ProtoContract]
public class Region
{
    [ProtoMember(1)]
    public required IReadOnlyCollection<Encounter> Encounters { get; init; }

    [ProtoMember(2)]
    public RegionId Id { get; init; }

    [ProtoMember(3)]
    public int Index { get; init; }

    [ProtoMember(4)]
    public RegionReward? Reward { get; init; }
}
