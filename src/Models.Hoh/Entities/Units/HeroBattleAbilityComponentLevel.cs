using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Units;

[ProtoContract]
public class HeroBattleAbilityComponentLevel
{
    [ProtoMember(1)]
    public required IList<string> AbilityIds { get; set; } = new List<string>();
    [ProtoMember(2)]
    public int Cost { get; init; }
    [ProtoMember(3)]
    public bool IsKeyLevel { get; init; }

    public string AbilityId => AbilityIds[0];
}