using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Battle;

[ProtoContract]
public class BattleWaveHeroSquad : BattleWaveSquadBase
{
    [ProtoMember(1)]
    public required string AbilityId { get; init; }
}
