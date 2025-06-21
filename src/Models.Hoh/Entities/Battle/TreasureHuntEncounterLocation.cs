using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Battle;

[ProtoContract]
public class TreasureHuntEncounterLocation : BattleLocationBase
{
    [ProtoMember(1)]
    public int Difficulty { get; set; }

    [ProtoMember(2)]
    public int Encounter { get; set; }

    [ProtoMember(3)]
    public int Stage { get; set; }

    [ProtoMember(4)]
    public int TreasureHuntEventId { get; set; }
}
