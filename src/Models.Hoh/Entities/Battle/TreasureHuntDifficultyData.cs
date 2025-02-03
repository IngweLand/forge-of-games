using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Battle;

[ProtoContract]
public class TreasureHuntDifficultyData
{
    [ProtoMember(1)]
    public required int Difficulty { get; init; }

    [ProtoMember(2)]
    public required IReadOnlyCollection<TreasureHuntStage> Stages { get; init; } = new List<TreasureHuntStage>();
}