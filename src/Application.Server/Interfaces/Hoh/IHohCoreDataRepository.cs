using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Entities.Research;
using Ingweland.Fog.Models.Hoh.Entities.Units;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Server.Interfaces.Hoh;

public interface IHohCoreDataRepository
{
    Task<Hero?> GetHeroAsync(string id);
    Task<HeroProgressionCost?> GetHeroProgressionCostsAsync(HeroProgressionCostId id);
    Task<HeroBattleAbilityComponent?> GetHeroAbilityComponentAsync(string heroAbilityId);
    Task<HeroAwakeningComponent?> GetHeroAwakeningComponentAsync(string awakeningId);
    Task<Unit?> GetUnitAsync(string id);
    Task<IReadOnlyCollection<Unit>> GetUnitsAsync(UnitType unitType);
    Task<IEnumerable<Hero>> GetHeroesAsync();
    Task<HeroAscensionCost?> GetHeroAscensionCostsAsync(string ascensionCostId);
    Task<HeroAbility?> GetHeroAbilityAsync(string abilityId);
    Task<UnitBattleConstants> GetUnitBattleConstants();
    Task<HeroUnitType> GetHeroUnitType(UnitType unitType);
    Task<IReadOnlyCollection<UnitStatFormulaData>> GetUnitStatFormulaData();
    Task<World?> GetWorldAsync(WorldId id);
    Task<IReadOnlyCollection<Building>> GetBuildingsAsync(CityId cityId);
    Task<IReadOnlyCollection<Technology>> GetTechnologiesAsync(CityId cityId);
    Task<Building?> GetBuildingAsync(string id);
    Task<IReadOnlyCollection<Building>> GetGroupBuildingsAsync(CityId cityId, BuildingGroup group);
    Task<IReadOnlyCollection<TreasureHuntDifficultyData>> GetTreasureHuntDifficultiesAsync();
    Task<TreasureHuntStage?> GetTreasureHuntStageAsync(int difficulty, int stageIndex);
    Task<Wonder?> GetWonderAsync(WonderId id);
    Task<IReadOnlyCollection<Age>> GetAges();

    Task<IReadOnlyCollection<Expansion>> GetExpansions(CityId cityId);
    Task<IReadOnlyCollection<BuildingCustomization>> GetBuildingCustomizations(CityId cityId);
    Task<CityDefinition?> GetCity(CityId id);
    Task<Hero?> GetHeroByUnitIdAsync(string id);
}
