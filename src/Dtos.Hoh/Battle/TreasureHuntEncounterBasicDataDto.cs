using ProtoBuf;

namespace Ingweland.Fog.Dtos.Hoh.Battle;

[ProtoContract]
public class TreasureHuntEncounterBasicDataDto
{
    [ProtoMember(1)]
    public required int Difficulty { get; init; }
    [ProtoMember(2)]
    public required int Encounter { get; init; }
    [ProtoMember(3)]
    public required int Index { get; init; }
    [ProtoMember(4)]
    public required int Stage { get; init; }
}
