using Ingweland.Fog.Models.Hoh.Entities.Battle;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Abstractions;

[ProtoContract]
[ProtoInclude(100, typeof(CampaignMapBattleLocation))]
[ProtoInclude(101, typeof(PvpBattleLocation))]
[ProtoInclude(102, typeof(TreasureHuntEncounterLocation))]
[ProtoInclude(103, typeof(HistoricBattleLocation))]
public abstract class BattleLocationBase
{
    
}
