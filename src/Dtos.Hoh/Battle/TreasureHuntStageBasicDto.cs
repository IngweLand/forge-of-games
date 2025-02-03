using ProtoBuf;

namespace Ingweland.Fog.Dtos.Hoh.Battle;

[ProtoContract]
public class TreasureHuntStageBasicDto
{
    [ProtoMember(1)]
    public int Index { get; init; }

    [ProtoMember(2)]
    public required string Name { get; init; }
}
