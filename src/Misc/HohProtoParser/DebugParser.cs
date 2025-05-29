using System.Text;
using AutoMapper;
using HohProtoParser.Converters;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Newtonsoft.Json;

namespace Ingweland.Fog.HohProtoParser;

public class DebugParser
{
    private readonly IMapper _mapper;

    public DebugParser(string filepath, IMapper mapper)
    {
        _mapper = mapper;
        using var file = File.OpenRead(filepath);
        var container = GameDesignResponseDtoContainer.Parser.ParseFrom(file);
        var gdr = GameDesignResponseDTO.Parser.ParseFrom(container.Content.Value);
        PrintProperty(gdr.BuildingDefinitions, dto => dto.Type, "building_types.txt");
        PrintProperty(gdr.BuildingDefinitions, dto => dto.Subtype, "building_subtypes.txt");
        PrintProperty(gdr.ResourceDefinitions, dto => dto.Type, "resource_types.txt");
        ProcessBuildings(gdr.BuildingDefinitions);
        // ProcessStartupBin();
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            Formatting = Formatting.Indented,
        };
        // File.WriteAllText("worlds.json", JsonConvert.SerializeObject(worlds, settings));
        // File.WriteAllText("buildings.json", JsonConvert.SerializeObject(buildings, settings));

        // hero classes 
        var gameDesignReferences =
            gdr.HeroDefinitions.SelectMany(hd =>
                hd.PackedComponents.Where(any => any.Is(GameDesignReference.Descriptor)));
        var classes = new HashSet<string>();
        foreach (var any in gameDesignReferences)
        {
            var gameDesignReference = any.Unpack<GameDesignReference>();
            if (gameDesignReference.Type.EndsWith("HeroClassDefinitionDTO"))
            {
                classes.Add(gameDesignReference.Id);
            }
        }

        Console.Out.WriteLine("Hero classes:");
        foreach (var c in classes)
        {
            Console.Out.WriteLine(c);
        }

        Console.Out.WriteLine("");

        // battle waves
        foreach (var wave in gdr.HeroBattleWaveDefinitions)
        {
            foreach (var squad in wave.Squads)
            {
                switch (squad.SquadCase)
                {
                    case HeroBattleWaveSquadDto.SquadOneofCase.Hero:
                    {
                        break;
                    }
                    case HeroBattleWaveSquadDto.SquadOneofCase.Unit:
                    {
                        break;
                    }
                    case HeroBattleWaveSquadDto.SquadOneofCase.None:
                    default:
                    {
                        Console.Out.WriteLine($"No squad on {wave.Id}");
                        break;
                    }
                }
            }
        }
    }

    private static void PrintLootContainerReward(LootContainerRewardDTO reward, StringBuilder sb, string prefix = "")
    {
        foreach (var lcrPackedReward in reward.PackedRewards)
        {
            sb.AppendLine($"{prefix}{lcrPackedReward.TypeUrl}");
        }
    }

    private static void PrintMysteryChestReward(MysteryChestRewardDTO reward, StringBuilder sb, string prefix = "")
    {
        foreach (var mcrPackedReward in reward.PackedRewards)
        {
            sb.AppendLine($"{prefix}\t{mcrPackedReward.TypeUrl}");
            if (mcrPackedReward.Is(RewardDefinitionDTO.Descriptor))
            {
                PrintRewardDefinition(mcrPackedReward.Unpack<RewardDefinitionDTO>(), sb, $"{prefix}\t\t");
            }
            else if (mcrPackedReward.Is(MysteryChestRewardDTO.Descriptor))
            {
                PrintMysteryChestReward(mcrPackedReward.Unpack<MysteryChestRewardDTO>(), sb, $"{prefix}\t\t");
            }
            else if (mcrPackedReward.Is(LootContainerRewardDTO.Descriptor))
            {
                PrintLootContainerReward(mcrPackedReward.Unpack<LootContainerRewardDTO>(), sb, $"{prefix}\t\t");
            }
        }
    }

    private static void PrintProperty<T>(IEnumerable<T> src, Func<T, string> propertySelector,
        string fileName)
    {
        var s = string.Join("\n", src.Select(propertySelector).ToHashSet());
        File.WriteAllText(fileName, s);
    }

    private static void PrintRewardDefinition(RewardDefinitionDTO rewardDefinition, StringBuilder sb,
        string prefix = "")
    {
        if (string.IsNullOrEmpty(prefix))
        {
            sb.AppendLine($"{prefix}{rewardDefinition.Id}");
        }

        if (rewardDefinition.Id == "reward.TreasureHunt_Milestone_63000")
        {
            Console.Out.WriteLine("");
        }

        foreach (var packedReward in rewardDefinition.PackedRewards)
        {
            sb.AppendLine($"{prefix}\t{packedReward.TypeUrl}");
            if (packedReward.Is(RewardDefinitionDTO.Descriptor))
            {
                PrintRewardDefinition(packedReward.Unpack<RewardDefinitionDTO>(), sb, $"{prefix}\t");
            }
            else if (packedReward.Is(MysteryChestRewardDTO.Descriptor))
            {
                var mcr = packedReward.Unpack<MysteryChestRewardDTO>();
                PrintMysteryChestReward(mcr, sb, $"{prefix}\t");
            }
            else if (packedReward.Is(LootContainerRewardDTO.Descriptor))
            {
                PrintLootContainerReward(packedReward.Unpack<LootContainerRewardDTO>(), sb, $"{prefix}\t\t");
            }
        }
    }

    private static void ProcessBuildings(IList<BuildingDefinitionDTO> list)
    {
        var sb = new StringBuilder();
        var componentTypes = new HashSet<string>();
        foreach (var buildingDefinition in list)
        {
            sb.AppendLine(buildingDefinition.Id);
            if (buildingDefinition.BuffDetails is {Factor: > 0})
            {
                sb.AppendLine($"\tBuff modifier: {buildingDefinition.BuffDetails?.Factor}");
            }

            sb.AppendLine("\tComponents:");
            foreach (var any in buildingDefinition.PackedComponents)
            {
                sb.AppendLine($"\t\t{any.TypeUrl}");
                componentTypes.Add(any.TypeUrl);
                if (any.Is(ProductionComponentDTO.Descriptor))
                {
                    var pc = any.Unpack<ProductionComponentDTO>();
                    // if (pc.PackedWorkerBehaviour == null)
                    // {
                    //     Console.Out.WriteLine(pc.Id);
                    // }
                    // if (pc is {Cost: not null})
                    // {
                    //    Console.Out.WriteLine(pc.Id);
                    // }

                    // if (pc.Factor != 1.0D)
                    // {
                    //     Console.Out.WriteLine($"{pc.Id} - {pc.Factor}");
                    // }

                    // if (pc.Product.PackedRewards.Any(any => !any.Is(ResourceRewardDTO.Descriptor)))
                    // {
                    //     Console.Out.WriteLine(pc.Id);
                    // }
                }
                else if (any.Is(BuildingUnitProviderComponentDTO.Descriptor))
                {
                    var c = any.Unpack<BuildingUnitProviderComponentDTO>();
                    if (c.UnitCount != 5)
                    {
                        Console.Out.WriteLine(buildingDefinition.Id);
                    }
                }
            }
        }

        File.WriteAllText("building_definitions.txt", sb.ToString());

        foreach (var value in componentTypes)
        {
            Console.Out.WriteLine(value);
        }
    }

    private static void ProcessRewardDefinitions(IList<RewardDefinitionDTO> list)
    {
        var sb = new StringBuilder();
        foreach (var rewardDefinition in list)
        {
            PrintRewardDefinition(rewardDefinition, sb);
        }

        File.WriteAllText("resource_definitions.txt", sb.ToString());
    }

    private void ProcessStartupBin()
    {
        const string startupBin = @"D:\IngweLand\Projects\forge-of-games\resources\hoh\data\startup_10.01.25.bin";
        using var file = File.OpenRead(startupBin);
        var container = StartupDto.Parser.ParseFrom(file);
        var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile<StartupProfile>(); });
        var cities = _mapper.Map<IList<City>>(container.Cities);
    }
}
