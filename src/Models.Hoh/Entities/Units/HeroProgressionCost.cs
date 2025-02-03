using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Units;

[ProtoContract]
public class HeroProgressionCost
{
    [ProtoMember(1)]
    public HeroProgressionCostId Id { get; init; }

    [ProtoMember(2)]
    public required IDictionary<int, HeroProgressionCostResource> LevelCosts { get; init; }
}
