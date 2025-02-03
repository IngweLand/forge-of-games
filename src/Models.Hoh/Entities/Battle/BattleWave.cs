using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Battle;

[ProtoContract]
public class BattleWave
{
    [ProtoMember(1)]
    public required string Id { get; init; }

    [ProtoMember(2)]
    public required IReadOnlyCollection<BattleWaveSquadBase> Squads { get; init; } = new List<BattleWaveSquadBase>();
}
