using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Dtos.Hoh.Battle;

[ProtoContract]
public class EncounterDetailsDto
{
    [ProtoMember(1)]
    public required IReadOnlyCollection<RewardBase> FirstTimeCompletionBonus { get; init; }

    [ProtoMember(2)]
    public required IReadOnlyCollection<RewardBase> Rewards { get; init; }

    [ProtoMember(3)]
    public IReadOnlyCollection<BattleWave> Waves { get; init; } =  new List<BattleWave>();
    
    [ProtoMember(4)]
    public int AvailableHeroSlots { get; init; } = 5;
    
    [ProtoMember(5)]
    public IReadOnlyCollection<HeroClassId> RequiredHeroClasses { get; init; } =  new List<HeroClassId>();
    
    [ProtoMember(6)]
    public IReadOnlyCollection<UnitType> RequiredHeroTypes { get; init; } =  new List<UnitType>();
}