using ProtoBuf;

namespace Ingweland.Fog.Dtos.Hoh.Battle;

[ProtoContract]
public class TreasureHuntDifficultyDataBasicDto
{
    [ProtoMember(1)]
    public int Difficulty { get; init; }

    [ProtoMember(2)]
    public required string Name { get; init; }

    [ProtoMember(3)]
    public required IReadOnlyCollection<TreasureHuntStageBasicDto> Stages { get; init; } =
        new List<TreasureHuntStageBasicDto>();
}
