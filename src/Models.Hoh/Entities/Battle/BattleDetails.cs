using Ingweland.Fog.Models.Hoh.Entities.Units;
using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Battle;

[ProtoContract]
public class BattleDetails
{
    [ProtoMember(1)]
    public required string Id { get; init; }

    [ProtoMember(2)]
    public required IReadOnlyCollection<BattleWave> Waves { get; init; }

    [ProtoMember(3)]
    public IReadOnlyCollection<int> DisabledPlayerSlotIds { get; init; } = new List<int>();
    
    [ProtoMember(4)]
    public IReadOnlyCollection<HeroClassId> RequiredHeroClasses { get; init; } =  new List<HeroClassId>();
    
    [ProtoMember(5)]
    public IReadOnlyCollection<UnitType> RequiredHeroTypes { get; init; } =  new List<UnitType>();
}
