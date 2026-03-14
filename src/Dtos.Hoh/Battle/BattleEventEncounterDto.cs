using Ingweland.Fog.Models.Hoh.Entities.Battle;
using ProtoBuf;

namespace Ingweland.Fog.Dtos.Hoh.Battle;

[ProtoContract]
public class BattleEventEncounterDto
{
    [ProtoMember(1)]
    public required string Id { get; init; }

    [ProtoMember(2)]
    public required int Index { get; init; }

    [ProtoMember(3)]
    public IReadOnlyCollection<BattleWave> Waves { get; init; } = new List<BattleWave>();
}
