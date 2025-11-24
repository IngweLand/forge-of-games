using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Battle;

[ProtoContract]
public class SquadRelic
{
    [ProtoMember(1)]
    public required string Age { get; init; }

    [ProtoMember(2)]
    public int AscensionLevel { get; init; }

    [ProtoMember(3)]
    public required string Id { get; init; }

    [ProtoMember(4)]
    public required int Level { get; init; }
}
