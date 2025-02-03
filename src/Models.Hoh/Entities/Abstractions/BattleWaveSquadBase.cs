using Ingweland.Fog.Models.Hoh.Entities.Battle;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Abstractions;

[ProtoContract]
[ProtoInclude(100, typeof(BattleWaveHeroSquad))]
[ProtoInclude(101, typeof(BattleWaveUnitSquad))]
public abstract class BattleWaveSquadBase
{
    [ProtoMember(1)]
    public required string UnitId { get; init; }

    [ProtoMember(2)]
    public int UnitLevel { get; init; }
}
