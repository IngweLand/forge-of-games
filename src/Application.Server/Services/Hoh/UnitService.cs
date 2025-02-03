using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Factories;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Entities.Units;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Server.Services.Hoh;

public class UnitService(
    IHohCoreDataRepository hohCoreDataRepository,
    ILogger<UnitService> logger,
    IHeroBasicDtoFactory heroBasicDtoFactory,
    IHeroDtoFactory heroDtoFactory,
    IHeroAbilityDtoFactory heroAbilityDtoFactory)
    : IUnitService
{
    public async Task<HeroDto?> GetHeroAsync(string id)
    {
        var hero = await hohCoreDataRepository.GetHeroAsync(id);
        if (hero == null)
        {
            logger.LogError($"Could not find hero with id {id}");
            return null;
        }

        var unit = await hohCoreDataRepository.GetUnitAsync(hero.UnitId);
        if (unit == null)
        {
            logger.LogError($"Could not find unit {hero.UnitId} for the hero with id {id}");
            return null;
        }

        var progressionCosts =
            await hohCoreDataRepository.GetHeroProgressionCostsAsync(hero.ProgressionComponent.CostId);
        if (progressionCosts == null)
        {
            logger.LogError($"Could not find progression costs {hero.ProgressionComponent.CostId} for the hero with id {
                id}");
            return null;
        }

        var ascensionCosts =
            await hohCoreDataRepository.GetHeroAscensionCostsAsync(hero.ProgressionComponent.AscensionCostId);
        if (ascensionCosts == null)
        {
            logger.LogError(
                $"Could not find ascension progression costs {hero.ProgressionComponent.AscensionCostId
                } for the hero with id {
                    id}");
            return null;
        }
        
        var awakeningComponent =
            await hohCoreDataRepository.GetHeroAwakeningComponentAsync(hero.AwakeningId);
        if (awakeningComponent == null)
        {
            logger.LogError(
                $"Could not find awakening component {hero.AwakeningId} for the hero with id {id}");
            return null;
        }

        var ability = await GetHeroAbilityAsync(hero);
        if (ability == null)
        {
            return null;
        }
        
        var sortedProgressionCosts = progressionCosts.LevelCosts.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value)
            .ToList()
            .AsReadOnly();
        var sortedAscensionCosts = ascensionCosts.LevelCosts.OrderBy(kvp => kvp.Key).ToDictionary().AsReadOnly();

        var units = await hohCoreDataRepository.GetUnitsAsync(hero.SupportUnitType);
        var baseSupportUnit = units.Single(u => u.Id.StartsWith("unit.Unit_StoneAge_Player"));
        
        return heroDtoFactory.Create(hero, unit, sortedProgressionCosts, sortedAscensionCosts,
            await hohCoreDataRepository.GetUnitStatFormulaData(),
            await hohCoreDataRepository.GetUnitBattleConstants(),
            baseSupportUnit,
            awakeningComponent,
            ability,
            await hohCoreDataRepository.GetHeroUnitType(unit.Type));
    }

    public async Task<HeroAbilityDto?> GetHeroAbilityAsync(string heroId)
    {
        var hero = await hohCoreDataRepository.GetHeroAsync(heroId);
        if (hero == null)
        {
            logger.LogError($"Could not find hero with id {heroId}");
            return null;
        }

        return await GetHeroAbilityAsync(hero);
    }
    
    public async Task<HeroAbilityDto?> GetHeroAbilityAsync(Hero hero)
    {
        var abilityComponent = await hohCoreDataRepository.GetHeroAbilityComponentAsync(hero.AbilityId);
        if (abilityComponent == null)
        {
            logger.LogError($"Could not find ability component for hero with id {hero.Id} and hero ability id {
                hero.AbilityId}");
            return null;
        }

        var abilities = new List<HeroAbility>();
        foreach (var level in abilityComponent.Levels)
        {
            var ability = await hohCoreDataRepository.GetHeroAbilityAsync(level.AbilityId);
            if (ability == null)
            {
                logger.LogError($"Could not find ability for hero with id {hero.Id} and ability id {level.AbilityId}");
                return null;
            }

            abilities.Add(ability);
        }

        return heroAbilityDtoFactory.Create(abilityComponent, abilities);
    }

    public async Task<IReadOnlyCollection<HeroBasicDto>> GetHeroesBasicDataAsync()
    {
        var heroes = new List<HeroBasicDto>();
        foreach (var hero in await hohCoreDataRepository.GetHeroesAsync())
        {
            var unit = await hohCoreDataRepository.GetUnitAsync(hero.UnitId);
            if (unit == null)
            {
                logger.LogError($"Could not find unit {hero.UnitId} for the hero with id {hero.Id}");
                continue;
            }

            heroes.Add(heroBasicDtoFactory.Create(hero, unit));
        }

        return heroes.OrderBy(h => h.Name).ToList().AsReadOnly();
    }
}
