using Ingweland.Fog.Models.Hoh.Entities.Battle;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Abstractions;

[ProtoContract]
[ProtoInclude(100, typeof(CampaignMapBattleLocation))]
[ProtoInclude(101, typeof(PvpBattleLocation))]
[ProtoInclude(102, typeof(TreasureHuntEncounterLocation))]
[ProtoInclude(103, typeof(HistoricBattleLocation))]
[ProtoInclude(104, typeof(PvpRevengeBattleLocation))]
[ProtoInclude(105, typeof(BattleEventBattleLocation))]
public abstract class BattleLocationBase
{
}
