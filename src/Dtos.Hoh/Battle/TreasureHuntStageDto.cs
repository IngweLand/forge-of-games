using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using ProtoBuf;

namespace Ingweland.Fog.Dtos.Hoh.Battle;

[ProtoContract]
public class TreasureHuntStageDto
{
    [ProtoMember(1)]
    public required IReadOnlyCollection<BattleDetails> Battles { get; init; } = new List<BattleDetails>();

    [ProtoMember(2)]
    public required int Difficulty { get; init; }

    [ProtoMember(3)]
    public required string DifficultyName { get; init; }

    [ProtoMember(4)]
    public required int Index { get; init; }

    [ProtoMember(5)]
    public required string Name { get; init; }

    [ProtoMember(6)]
    public required IReadOnlyCollection<UnitDto> Units { get; init; } = new List<UnitDto>();
    
    [ProtoMember(7)]
    public required IReadOnlyCollection<HeroDto> Heroes { get; init; } = new List<HeroDto>();
}
