using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Units;

[ProtoContract]
public class RelicLevel
{
    [ProtoMember(1)]
    public required IReadOnlyCollection<string> Abilities { get; init; } = new List<string>();

    [ProtoMember(2)]
    public int AscensionLevel { get; init; }

    [ProtoMember(3)]
    public required IReadOnlyCollection<RelicBoost> Boosts { get; init; }

    [ProtoMember(4)]
    public required string CostId { get; init; }

    [ProtoMember(5)]
    public bool IsAscension { get; init; }

    [ProtoMember(6)]
    public required int Level { get; init; }
}
