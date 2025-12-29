using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Entities.Equipment;
using Ingweland.Fog.Models.Hoh.Entities.Relics;
using Ingweland.Fog.Models.Hoh.Entities.Research;
using Ingweland.Fog.Models.Hoh.Entities.Units;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Server.Interfaces.Hoh;

public interface IHohCoreDataRepository
{
    Guid Version { get; }
    Task<Building?> GetBuildingAsync(string id);
    Task<CityDefinition?> GetCity(CityId id);
    Task<Hero?> GetHeroAsync(string id);
    Task<Hero?> GetHeroByUnitIdAsync(string id);
    Task<BattleAbility?> GetHeroAbilityAsync(string abilityId);
    Task<HeroAscensionCost?> GetHeroAscensionCostsAsync(string ascensionCostId);
    Task<HeroAwakeningComponent?> GetHeroAwakeningComponentAsync(string awakeningId);
    Task<HeroBattleAbilityComponent?> GetHeroAbilityComponentAsync(string heroAbilityId);
    Task<HeroProgressionCost?> GetHeroProgressionCostsAsync(HeroProgressionCostId id);
    Task<HeroUnitType> GetHeroUnitType(UnitType unitType);
    Task<IEnumerable<Hero>> GetHeroesAsync();
    Task<IReadOnlyCollection<Age>> GetAges();
    Task<IReadOnlyCollection<Building>> GetBuildingsAsync(CityId cityId);
    Task<IReadOnlyCollection<Building>> GetGroupBuildingsAsync(CityId cityId, BuildingGroup group);
    Task<IReadOnlyCollection<BuildingCustomization>> GetBuildingCustomizations(CityId cityId);
    Task<IReadOnlyCollection<Expansion>> GetExpansions(CityId cityId);
    Task<IReadOnlyCollection<Technology>> GetTechnologiesAsync(CityId cityId);
    Task<IReadOnlyCollection<TreasureHuntDifficultyData>> GetTreasureHuntDifficultiesAsync();
    Task<IReadOnlyCollection<Unit>> GetUnitsAsync(UnitType unitType);
    Task<IReadOnlyCollection<UnitStatFormulaData>> GetUnitStatFormulaData();
    Task<IReadOnlyCollection<Wonder>> GetWondersAsync();
    Task<TreasureHuntStage?> GetTreasureHuntStageAsync(int difficulty, int stageIndex);
    Task<Unit?> GetUnitAsync(string id);
    Task<UnitBattleConstants> GetUnitBattleConstants();
    Task<Wonder?> GetWonderAsync(WonderId id);
    Task<World?> GetWorldAsync(WorldId id);
    Task<IReadOnlyCollection<Resource>> GetResources();
    Task<IReadOnlyCollection<Relic>> GetRelicsAsync();
    Task<IReadOnlyCollection<EquipmentSetDefinition>> GetEquipmentSetDefinitionsAsync();
}
