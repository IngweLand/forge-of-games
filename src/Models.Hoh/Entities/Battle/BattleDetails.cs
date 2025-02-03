using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Battle;

[ProtoContract]
public class BattleDetails
{
    [ProtoMember(1)]
    public required string Id { get; init; }

    [ProtoMember(2)]
    public required IReadOnlyCollection<BattleWave> Waves { get; init; }

    [ProtoMember(3)]
    public IReadOnlyCollection<int> DisabledPlayerSlotIds { get; init; } = new List<int>();
}
