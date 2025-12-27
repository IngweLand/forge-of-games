using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Infrastructure.Repositories.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Entities.Relics;
using Ingweland.Fog.Models.Hoh.Entities.Research;
using Ingweland.Fog.Models.Hoh.Entities.Units;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Infrastructure.Repositories;

public class HohCoreDataRepository(IHohDataProvider dataProvider) : IHohCoreDataRepository
{
    public async Task<Hero?> GetHeroAsync(string id)
    {
        var data = await dataProvider.GetDataAsync();
        return data.Heroes.FirstOrDefault(h => h.Id == id);
    }

    public async Task<Hero?> GetHeroByUnitIdAsync(string id)
    {
        var data = await dataProvider.GetDataAsync();
        return data.Heroes.FirstOrDefault(h => h.UnitId == id);
    }

    public async Task<HeroProgressionCost?> GetHeroProgressionCostsAsync(HeroProgressionCostId id)
    {
        var data = await dataProvider.GetDataAsync();
        return data.ProgressionCosts.FirstOrDefault(hpc => hpc.Id == id);
    }

    public async Task<HeroAwakeningComponent?> GetHeroAwakeningComponentAsync(string awakeningId)
    {
        var data = await dataProvider.GetDataAsync();
        return data.HeroAwakeningComponents.FirstOrDefault(hac => hac.Id == awakeningId);
    }

    public async Task<Unit?> GetUnitAsync(string id)
    {
        var data = await dataProvider.GetDataAsync();
        return data.Units.FirstOrDefault(u => u.Id == id);
    }

    public async Task<IReadOnlyCollection<Unit>> GetUnitsAsync(UnitType unitType)
    {
        var data = await dataProvider.GetDataAsync();
        return data.Units.Where(u => u.Type == unitType).ToList();
    }

    public async Task<IEnumerable<Hero>> GetHeroesAsync()
    {
        var data = await dataProvider.GetDataAsync();
        return data.Heroes;
    }

    public async Task<HeroAscensionCost?> GetHeroAscensionCostsAsync(string ascensionCostId)
    {
        var data = await dataProvider.GetDataAsync();
        return data.AscensionCosts.FirstOrDefault(hpc => hpc.Id == ascensionCostId);
    }

    public async Task<BattleAbility?> GetHeroAbilityAsync(string abilityId)
    {
        var data = await dataProvider.GetDataAsync();
        return data.HeroAbilities.FirstOrDefault(ha => ha.Id == abilityId);
    }

    public async Task<UnitBattleConstants> GetUnitBattleConstants()
    {
        var data = await dataProvider.GetDataAsync();
        return data.UnitBattleConstants;
    }

    public async Task<HeroUnitType> GetHeroUnitType(UnitType unitType)
    {
        var data = await dataProvider.GetDataAsync();
        return data.HeroUnitTypes.First(src => src.UnitType == unitType);
    }

    public async Task<IReadOnlyCollection<UnitStatFormulaData>> GetUnitStatFormulaData()
    {
        var data = await dataProvider.GetDataAsync();
        return data.UnitStatFormulaData;
    }

    public async Task<IReadOnlyCollection<Wonder>> GetWondersAsync()
    {
        var data = await dataProvider.GetDataAsync();
        return data.Wonders;
    }

    public async Task<World?> GetWorldAsync(WorldId id)
    {
        var data = await dataProvider.GetDataAsync();
        return data.Worlds.FirstOrDefault(w => w.Id == id);
    }

    public async Task<IReadOnlyCollection<Resource>> GetResources()
    {
        var data = await dataProvider.GetDataAsync();
        return data.Resources;
    }

    public async Task<IReadOnlyCollection<Relic>> GetRelicsAsync()
    {
        var data = await dataProvider.GetDataAsync();
        return data.Relics;
    }

    public async Task<IReadOnlyCollection<Building>> GetBuildingsAsync(CityId cityId)
    {
        var data = await dataProvider.GetDataAsync();
        return data.Buildings.Where(b => b.CityIds.Contains(cityId)).ToList();
    }

    public async Task<IReadOnlyCollection<Technology>> GetTechnologiesAsync(CityId cityId)
    {
        var data = await dataProvider.GetDataAsync();
        return data.Technologies.Where(b => b.CityId == cityId)
            .ToList();
    }

    public Guid Version => dataProvider.Version;

    public async Task<Building?> GetBuildingAsync(string id)
    {
        var data = await dataProvider.GetDataAsync();
        return data.Buildings.FirstOrDefault(b => b.Id == id);
    }

    public async Task<IReadOnlyCollection<Building>> GetGroupBuildingsAsync(CityId cityId, BuildingGroup group)
    {
        var data = await dataProvider.GetDataAsync();
        return data.Buildings
            .Where(b => b.CityIds.Contains(cityId) && b.Group == group).ToList();
    }

    public async Task<IReadOnlyCollection<TreasureHuntDifficultyData>> GetTreasureHuntDifficultiesAsync()
    {
        var data = await dataProvider.GetDataAsync();
        return data.TreasureHuntBattles;
    }

    public async Task<TreasureHuntStage?> GetTreasureHuntStageAsync(int difficulty, int stageIndex)
    {
        var data = await dataProvider.GetDataAsync();
        return data.TreasureHuntBattles.FirstOrDefault(thb => thb.Difficulty == difficulty)?.Stages
            .FirstOrDefault(ths => ths.Index == stageIndex);
    }

    public async Task<HeroBattleAbilityComponent?> GetHeroAbilityComponentAsync(string heroAbilityId)
    {
        var data = await dataProvider.GetDataAsync();
        return data.HeroBattleAbilityComponents.FirstOrDefault(ha => ha.HeroAbilityId == heroAbilityId);
    }

    public async Task<Wonder?> GetWonderAsync(WonderId id)
    {
        var data = await dataProvider.GetDataAsync();
        return data.Wonders.FirstOrDefault(src => src.Id == id);
    }

    public async Task<IReadOnlyCollection<Age>> GetAges()
    {
        var data = await dataProvider.GetDataAsync();
        return data.Ages;
    }

    public async Task<IReadOnlyCollection<Expansion>> GetExpansions(CityId cityId)
    {
        var data = await dataProvider.GetDataAsync();
        return data.Expansions.Where(src => src.CityId == cityId).ToList();
    }

    public async Task<IReadOnlyCollection<BuildingCustomization>> GetBuildingCustomizations(CityId cityId)
    {
        var data = await dataProvider.GetDataAsync();
        return data.BuildingCustomizations.Where(src => src.CityId == cityId).ToList();
    }

    public async Task<CityDefinition?> GetCity(CityId id)
    {
        var data = await dataProvider.GetDataAsync();
        return data.Cities.FirstOrDefault(src => src.Id == id);
    }
}
