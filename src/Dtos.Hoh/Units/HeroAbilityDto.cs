using ProtoBuf;

namespace Ingweland.Fog.Dtos.Hoh.Units;

[ProtoContract]
public class HeroAbilityDto
{
    [ProtoMember(1)]
    public required string Id { get; init; }
    
    [ProtoMember(2)]
    public required IReadOnlyCollection<HeroAbilityLevelDto> Levels { get; init; } =
        new List<HeroAbilityLevelDto>();

    [ProtoMember(3)]
    public required string Name { get; init; }
}