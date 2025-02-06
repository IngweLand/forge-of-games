using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Entities.Research;
using Ingweland.Fog.Models.Hoh.Entities.Units;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Server.Interfaces.Hoh;

public interface IHohCoreDataRepository
{
    Task<IReadOnlyCollection<Age>> GetAges();
    Task<IReadOnlyCollection<BattleAbility>> GetBattleAbilitiesAsync();
    Task<BattleAbility?> GetBattleAbilityAsync(string abilityId);
    Task<Building?> GetBuildingAsync(string id);
    Task<IReadOnlyCollection<BuildingCustomization>> GetBuildingCustomizations(CityId cityId);
    Task<IReadOnlyCollection<Building>> GetBuildingsAsync(CityId cityId);
    Task<CityDefinition?> GetCity(CityId id);

    Task<IReadOnlyCollection<Expansion>> GetExpansions(CityId cityId);
    Task<IReadOnlyCollection<Building>> GetGroupBuildingsAsync(CityId cityId, BuildingGroup group);
    Task<HeroAscensionCost?> GetHeroAscensionCostsAsync(string ascensionCostId);
    Task<Hero?> GetHeroAsync(string id);
    Task<HeroAwakeningComponent?> GetHeroAwakeningComponentAsync(string awakeningId);
    Task<HeroBattleAbilityComponent?> GetHeroBattleAbilityComponentAsync(string heroAbilityId);
    Task<IReadOnlyCollection<HeroBattleAbilityComponent>> GetHeroBattleAbilityComponentsAsync();
    Task<IEnumerable<Hero>> GetHeroesAsync();
    Task<HeroProgressionCost?> GetHeroProgressionCostsAsync(HeroProgressionCostId id);
    Task<HeroUnitType> GetHeroUnitType(UnitType unitType);
    Task<IReadOnlyCollection<RelicBoostAgeModifier>> GetRelicBoostAgeModifiersAsync();
    Task<IReadOnlyCollection<Relic>> GetRelicsAsync();
    Task<IReadOnlyCollection<Technology>> GetTechnologiesAsync(CityId cityId);
    Task<IReadOnlyCollection<TreasureHuntDifficultyData>> GetTreasureHuntDifficultiesAsync();
    Task<TreasureHuntStage?> GetTreasureHuntStageAsync(int difficulty, int stageIndex);
    Task<Unit?> GetUnitAsync(string id);
    Task<UnitBattleConstants> GetUnitBattleConstants();
    Task<IReadOnlyCollection<Unit>> GetUnitsAsync(UnitType unitType);
    Task<IReadOnlyCollection<UnitStatFormulaData>> GetUnitStatFormulaData();
    Task<Wonder?> GetWonderAsync(WonderId id);
    Task<World?> GetWorldAsync(WorldId id);
}
