using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Units;

[ProtoContract]
public class HeroProgressionComponent
{
    [ProtoMember(1)]
    public required HeroStarClass Id { get; init; }
    [ProtoMember(2)]
    public required HeroProgressionCostId CostId { get; init; }
    [ProtoMember(3)]
    public required string AscensionCostId { get; init; }
}
