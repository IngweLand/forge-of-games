using ProtoBuf;

namespace Ingweland.Fog.Dtos.Hoh.Units;

[ProtoContract]
public class BattleAbilityDto
{
    [ProtoMember(1)]
    public required string Id { get; init; }
    
    [ProtoMember(2)]
    public required IReadOnlyCollection<BattleAbilityLevelDto> Levels { get; init; } =
        new List<BattleAbilityLevelDto>();

    [ProtoMember(3)]
    public required string Name { get; init; }
}