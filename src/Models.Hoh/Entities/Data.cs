using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Entities.Equipment;
using Ingweland.Fog.Models.Hoh.Entities.Relics;
using Ingweland.Fog.Models.Hoh.Entities.Research;
using Ingweland.Fog.Models.Hoh.Entities.Units;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities;

[ProtoContract]
public class Data
{
    [ProtoMember(1)]
    public required IReadOnlyCollection<World> Worlds { get; init; }
    [ProtoMember(2)]
    public required IReadOnlyCollection<Building> Buildings { get; init; }
    [ProtoMember(3)]
    public required IReadOnlyCollection<Unit> Units { get; init; }
    [ProtoMember(4)]
    public required IReadOnlyCollection<Hero> Heroes { get; init; }
    [ProtoMember(5)]
    public required IReadOnlyCollection<HeroProgressionCost> ProgressionCosts { get; init; }
    [ProtoMember(6)]
    public required IReadOnlyCollection<HeroAscensionCost> AscensionCosts { get; init; }
    [ProtoMember(7)]
    public required UnitBattleConstants UnitBattleConstants { get; init; }
    [ProtoMember(8)]
    public required IReadOnlyCollection<UnitStatFormulaData> UnitStatFormulaData { get; init; }
    [ProtoMember(9)]
    public required IReadOnlyCollection<TreasureHuntDifficultyData> TreasureHuntBattles { get; init; }
    [ProtoMember(10)]
    public required IReadOnlyCollection<BattleAbility> HeroAbilities { get; init; }

    [ProtoMember(11)]
    public required IReadOnlyCollection<HeroBattleAbilityComponent> HeroBattleAbilityComponents { get; init; }

    [ProtoMember(12)]
    public required IReadOnlyCollection<Wonder> Wonders { get; init; }

    [ProtoMember(13)]
    public required IReadOnlyCollection<HeroAwakeningComponent> HeroAwakeningComponents { get; init; }
    
    [ProtoMember(14)]
    public required IReadOnlyCollection<Expansion> Expansions { get; init; }
    
    [ProtoMember(15)]
    public required IReadOnlyCollection<Technology> Technologies { get; init; }
    
    [ProtoMember(16)]
    public required IReadOnlyCollection<Age> Ages { get; init; }

    [ProtoMember(17)]
    public required IReadOnlyCollection<City.CityDefinition> Cities { get; init; }
    
    [ProtoMember(18)]
    public required IReadOnlyCollection<BuildingCustomization> BuildingCustomizations { get; init; }

    [ProtoMember(19)]
    public required IReadOnlyCollection<HeroUnitType> HeroUnitTypes { get; init; }
    
    [ProtoMember(20)]
    public required IReadOnlyCollection<Resource> Resources { get; init; }

    [ProtoMember(21)]
    public required IReadOnlyCollection<Relic> Relics { get; init; }

    [ProtoMember(22)]
    public IReadOnlyDictionary<string, float> RelicBoostAgeModifiers { get; init; }
    
    [ProtoMember(23)]
    public IReadOnlyCollection<EquipmentSetDefinition> EquipmentSetDefinitions { get; init; }
    
    [ProtoMember(24)]
    public required IReadOnlyCollection<Hero> LegacyHeroes { get; init; }
}
