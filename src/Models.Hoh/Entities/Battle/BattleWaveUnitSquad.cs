using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Battle;

[ProtoContract]
public class BattleWaveUnitSquad : BattleWaveSquadBase
{
    [ProtoMember(1)]
    public int Size { get; init; }
}
