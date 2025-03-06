using AutoMapper;
using HohProtoParser.Converters;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Entities.Research;
using Ingweland.Fog.Models.Hoh.Entities.Units;
using Ingweland.Fog.Shared.Helpers.Interfaces;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.HohProtoParser;

public class GameDesignDataParser(IProtobufSerializer protobufSerializer, ILogger<GameDesignDataParser> logger)
{
    private static readonly HashSet<string> HeroesToSkip = 
    [
        "hero.AlexanderTheGreat", 
        "hero.PhilipIIOfMacedon",
    ];

    public void Parse(string input, string? outputDirectory)
    {
        if (HeroesToSkip.Count > 0)
        {
            logger.LogWarning("");
            logger.LogWarning("===================================");
            logger.LogWarning("");
            logger.LogWarning("Skipping following Heroes:");
            logger.LogWarning(string.Join(',', HeroesToSkip));
            logger.LogWarning("");
            logger.LogWarning("===================================");
            logger.LogWarning("");
        }

        logger.LogInformation("Starting parsing game design data.");
        var filename = "data.bin";
        var outputFilePath = filename;
        if (outputDirectory != null)
        {
            Directory.CreateDirectory(outputDirectory);
            outputFilePath = Path.Combine(outputDirectory, filename);
        }

        logger.LogInformation($"Input file: {input}. Output file: {outputFilePath}.");
        using var file = File.OpenRead(input);
        var container = GameDesignResponseDtoContainer.Parser.ParseFrom(file);
        var gdr = GameDesignResponseDTO.Parser.ParseFrom(container.Content.Value);
        var data = Parse(gdr);
        protobufSerializer.SerializeToFile(data, outputFilePath);
        logger.LogInformation("Completed parsing game design data.");
    }

    private static IList<BuildingCustomization> BuildingCustomizations(IMapper mapper, GameDesignResponseDTO gdr,
        IDictionary<string, Age> ages,
        IDictionary<string, Unit> units)
    {
        return mapper.Map<IList<BuildingCustomization>>(gdr.BuildingCustomizationDefinitions, opt =>
        {
            opt.Items.Add(ContextKeys.AGES, ages);
            opt.Items.Add(ContextKeys.UNITS, units);
            opt.Items.Add(ContextKeys.HERO_BUILDING_BOOST_COMPONENTS,
                gdr.HeroBuildingBoostComponents.ToDictionary(hbbc => hbbc.Id));
            opt.Items.Add(ContextKeys.HERO_ABILITY_TRAINING_COMPONENTS,
                gdr.HeroAbilityTrainingComponents.ToDictionary(hatc => hatc.Id));
        });
    }

    private static IMapper ConfigureAutoMapper()
    {
        var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile<MappingProfile>(); });

        return mappingConfig.CreateMapper();
    }

    private static IList<Building> CreateBuildings(IMapper mapper, GameDesignResponseDTO gdr,
        IDictionary<string, Age> ages,
        IDictionary<string, Unit> units)
    {
        var buildings = mapper.Map<IList<Building>>(gdr.BuildingDefinitions, opt =>
        {
            opt.Items.Add(ContextKeys.AGES, ages);
            opt.Items.Add(ContextKeys.UNITS, units);
            opt.Items.Add(ContextKeys.HERO_BUILDING_BOOST_COMPONENTS,
                gdr.HeroBuildingBoostComponents.ToDictionary(hbbc => hbbc.Id));
            opt.Items.Add(ContextKeys.HERO_ABILITY_TRAINING_COMPONENTS,
                gdr.HeroAbilityTrainingComponents.ToDictionary(hatc => hatc.Id));
            opt.Items.Add(ContextKeys.DYNAMIC_FLOAT_VALUE_DEFINITIONS, gdr.DynamicFloatValueDefinitions);
        });
        foreach (var building in buildings)
        {
            var upgradeComponent = building.Components.OfType<UpgradeComponent>()
                .FirstOrDefault(uc => uc.NextBuildingId != building.Id);
            if (upgradeComponent == null)
            {
                continue;
            }

            building.Components.Remove(upgradeComponent);
            var targetBuilding = buildings.FirstOrDefault(b => b.Id == upgradeComponent.NextBuildingId);
            if (targetBuilding != null)
            {
                targetBuilding.Components.Add(upgradeComponent);
            }
        }

        return buildings;
    }

    private static IList<CityDefinition> CreateCities(IMapper mapper, GameDesignResponseDTO gdr)
    {
        return mapper.Map<IList<CityDefinition>>(gdr.CityDefinitions);
    }

    private static IList<Expansion> CreateExpansions(IMapper mapper, GameDesignResponseDTO gdr)
    {
        return mapper.Map<IList<Expansion>>(gdr.ExpansionDefinitions);
    }

    private static IList<HeroAbility> CreateHeroAbilities(IMapper mapper, GameDesignResponseDTO gdr)
    {
        return mapper.Map<IList<HeroAbility>>(gdr.BattleAbilityDefinitions);
    }

    private static IList<HeroBattleAbilityComponent> CreateHeroAbilityComponents(IMapper mapper,
        GameDesignResponseDTO gdr)
    {
        return mapper.Map<IList<HeroBattleAbilityComponent>>(gdr.HeroBattleAbilityComponents);
    }

    private static IList<HeroAwakeningComponent> CreateHeroAwakeningComponents(IMapper mapper,
        GameDesignResponseDTO gdr)
    {
        return mapper.Map<IList<HeroAwakeningComponent>>(gdr.HeroAwakeningComponents);
    }

    private static IList<Technology> CreateTechnologies(IMapper mapper, GameDesignResponseDTO gdr,
        IDictionary<string, Age> ages)
    {
        return mapper.Map<IList<Technology>>(gdr.TechnologyDefinitions,
            opt => { opt.Items.Add(ContextKeys.AGES, ages); });
    }

    private static List<TreasureHuntDifficultyData> CreateTreasureHuntBattles(IMapper mapper, GameDesignResponseDTO gdr,
        IDictionary<string, Unit> units)
    {
        var treasureHuntBattleDefinitions = gdr.HeroBattleDefinitions
            .Where(hbd => hbd.Id.StartsWith("hero_battle.Encounter"))
            .OrderBy(hbd => hbd.Id);
        var battles = mapper.Map<IList<BattleDetails>>(treasureHuntBattleDefinitions, opt =>
        {
            opt.Items.Add(ContextKeys.BATTLE_WAVES_DEFINITIONS, gdr.HeroBattleWaveDefinitions);
            opt.Items.Add(ContextKeys.UNITS, units);
        });
        var difficulties = new List<TreasureHuntDifficultyData>();
        var difficultyLevels = battles
            .Select(bd =>
            {
                var parts = bd.Id.Split('_');
                return parts[^3];
            })
            .ToHashSet();
        foreach (var difficultyLevel in difficultyLevels)
        {
            var stages = new List<TreasureHuntStage>();
            var difficultyBattles = battles
                .Where(bd =>
                {
                    var parts = bd.Id.Split('_');
                    return parts[^3] == difficultyLevel;
                }).ToList();
            for (var i = 0; i < 4; i++)
            {
                var stageBattles = difficultyBattles
                    .Where(bd =>
                    {
                        var parts = bd.Id.Split('_');
                        return parts[^2] == i.ToString();
                    });
                stages.Add(new TreasureHuntStage()
                {
                    Index = i,
                    Battles = stageBattles.OrderBy(bd =>
                    {
                        var parts = bd.Id.Split('_');
                        return int.Parse(parts[^1]);
                    }).ToList(),
                });
            }

            difficulties.Add(new TreasureHuntDifficultyData()
            {
                Difficulty = int.Parse(difficultyLevel),
                Stages = stages,
            });
        }

        return difficulties;
    }

    private static IList<Wonder> CreateWonders(IMapper mapper, GameDesignResponseDTO gdr)
    {
        return mapper.Map<IList<Wonder>>(gdr.ReworkedWonderDefinitions);
    }

    private static IList<World> CreateWorlds(IMapper mapper, GameDesignResponseDTO gdr, IDictionary<string, Age> ages,
        IDictionary<string, Unit> units)
    {
        var encounters =
            mapper.Map<IList<Encounter>>(gdr.EncounterDefinitions, opt =>
            {
                opt.Items.Add(ContextKeys.BATTLES_DEFINITIONS, gdr.HeroBattleDefinitions);
                opt.Items.Add(ContextKeys.BATTLE_WAVES_DEFINITIONS, gdr.HeroBattleWaveDefinitions);
                opt.Items.Add(ContextKeys.UNITS, units);
            });
        var regions = mapper.Map<IList<Region>>(gdr.RegionDefinitions, opt =>
        {
            opt.Items.Add(ContextKeys.AGES, ages);
            opt.Items.Add(ContextKeys.CONTINENT_DEFINITIONS, gdr.ContinentDefinitions);
            opt.Items.Add(ContextKeys.ENCOUNTERS, encounters);
        });
        var continents = mapper.Map<IList<Continent>>(gdr.ContinentDefinitions, opt =>
        {
            opt.Items.Add(ContextKeys.WORLD_DEFINITIONS, gdr.WorldDefinitions);
            opt.Items.Add(ContextKeys.REGIONS, regions);
        });

        return mapper.Map<IList<World>>(gdr.WorldDefinitions,
            opt => { opt.Items.Add(ContextKeys.CONTINENTS, continents); });
        ;
    }

    private static Data Parse(GameDesignResponseDTO gdr)
    {
        var mapper = ConfigureAutoMapper();
        var ages = mapper.Map<IList<Age>>(gdr.AgeDefinitions).ToDictionary(a => a.Id);
        var resources = mapper
            .Map<IList<Resource>>(gdr.ResourceDefinitions, opt => opt.Items.Add(ContextKeys.AGES, ages));
        var units = mapper.Map<IList<Unit>>(gdr.HeroUnitDefinitions).ToDictionary(r => r.Id);
        var worlds = CreateWorlds(mapper, gdr, ages, units);
        var technologies = CreateTechnologies(mapper, gdr, ages);
        var buildings = CreateBuildings(mapper, gdr, ages, units);
        var heroAbilities = CreateHeroAbilities(mapper, gdr);
        var heroAbilityComponents = CreateHeroAbilityComponents(mapper, gdr);
        var treasureHuntBattles = CreateTreasureHuntBattles(mapper, gdr, units);
        var wonders = CreateWonders(mapper, gdr);
        var heroAwakeningComponents = CreateHeroAwakeningComponents(mapper, gdr);
        var cities = CreateCities(mapper, gdr);
        var buildingCustomizations = BuildingCustomizations(mapper, gdr, ages, units);

        var data = new Data
        {
            Worlds = worlds.AsReadOnly(),
            Buildings = buildings.AsReadOnly(),
            Units = units.Values,
            Heroes =
                mapper.Map<IReadOnlyCollection<Hero>>(gdr.HeroDefinitions.Where(h => !HeroesToSkip.Contains(h.Id))),
            ProgressionCosts = mapper.Map<IReadOnlyCollection<HeroProgressionCost>>(gdr.HeroProgressionCostDefinitions),
            AscensionCosts =
                mapper.Map<IReadOnlyCollection<HeroAscensionCost>>(gdr.HeroProgressionAscensionCostDefinitions),
            UnitBattleConstants = mapper.Map<UnitBattleConstants>(gdr.HeroBattleConstantsDefinition),
            UnitStatFormulaData =
                mapper.Map<IReadOnlyCollection<UnitStatFormulaData>>(gdr.HeroUnitStatFormulaDefinitions),
            TreasureHuntBattles = treasureHuntBattles.AsReadOnly(),
            HeroAbilities = heroAbilities.AsReadOnly(),
            HeroBattleAbilityComponents = heroAbilityComponents.AsReadOnly(),
            Wonders = wonders.AsReadOnly(),
            HeroAwakeningComponents = heroAwakeningComponents.AsReadOnly(),
            Expansions = CreateExpansions(mapper, gdr).AsReadOnly(),
            Technologies = technologies.AsReadOnly(),
            Ages = ages.Select(kvp => kvp.Value).ToList(),
            Cities = cities.ToList(),
            BuildingCustomizations = buildingCustomizations.ToList(),
            HeroUnitTypes = mapper.Map<IReadOnlyCollection<HeroUnitType>>(gdr.HeroUnitTypeDefinitions),
            Resources = resources.AsReadOnly(),
        };

        return data;
    }
}
