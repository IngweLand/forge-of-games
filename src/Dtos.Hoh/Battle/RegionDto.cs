using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Dtos.Hoh.Battle;

[ProtoContract]
public class RegionDto
{
    [ProtoMember(1)]
    public required IReadOnlyCollection<EncounterDto> Encounters { get; init; }
    [ProtoMember(2)]
    public RegionId Id { get; init; }
    [ProtoMember(3)]
    public int Index { get; init; }
    [ProtoMember(4)]
    public required string Name { get; init; }
    [ProtoMember(5)]
    public required IReadOnlyCollection<RewardBase> Rewards { get; init; }
    [ProtoMember(6)]
    public IReadOnlyCollection<UnitDto> Units { get; set; }
}