using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Dtos.Hoh.Battle;

[ProtoContract]
public class BattleEventRegionDto
{
    [ProtoMember(1)]
    public required IReadOnlyCollection<BattleEventEncounterDto> Encounters { get; init; }

    [ProtoMember(2)]
    public IReadOnlyCollection<HeroDto> Heroes { get; init; } = new List<HeroDto>();

    [ProtoMember(3)]
    public RegionId Id { get; init; }

    [ProtoMember(4)]
    public required string Name { get; init; }

    [ProtoMember(5)]
    public IReadOnlyCollection<UnitDto> Units { get; init; } = new List<UnitDto>();
}
