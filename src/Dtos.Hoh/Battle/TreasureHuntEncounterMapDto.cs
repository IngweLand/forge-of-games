using System.Collections.Concurrent;
using ProtoBuf;

namespace Ingweland.Fog.Dtos.Hoh.Battle;

[ProtoContract]
public class TreasureHuntEncounterMapDto
{
    [ProtoMember(1)]
    public IReadOnlyDictionary<(int difficulty, int stage), IReadOnlyDictionary<int, int>>
        BattleEncounterMap { get; set; } =new Dictionary<(int difficulty, int stage), IReadOnlyDictionary<int, int>>();
}
