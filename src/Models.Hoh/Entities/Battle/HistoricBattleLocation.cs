using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Battle;

[ProtoContract]
public class HistoricBattleLocation : BattleLocationBase, IBattleLocationWithDifficulty
{
    [ProtoMember(1)]
    public required Difficulty Difficulty { get; set; }

    [ProtoMember(2)]
    public required string Encounter { get; set; }
}