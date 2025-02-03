using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Units;

[ProtoContract]
public class HeroAwakeningComponent
{
    [ProtoMember(1)]
    public required string Id { get; init; }
    [ProtoMember(2)]
    public required IReadOnlyCollection<AwakeningLevel> Levels { get; init; } = new List<AwakeningLevel>();
}
