using AutoMapper;
using Ingweland.Fog.Application.Core.Enums.Hoh;
using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Shared;
using Ingweland.Fog.Shared.Helpers;
using Ingweland.Fog.Shared.Helpers.Interfaces;
using Ingweland.Fog.Shared.Localization;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.HohProtoParser;

public class LocalizationParser(IMapper mapper, IProtobufSerializer protobufSerializer, ILogger<LocalizationParser> logger)
{
    private static readonly IList<string> UsedLocalizationCategories = new List<string>()
    {
        HohLocalizationKeyBuilder.BuildKey(HohLocalizationCategory.Abilities, string.Empty),
        HohLocalizationKeyBuilder.BuildKey(HohLocalizationCategory.AbilityDescriptions, string.Empty),
        HohLocalizationKeyBuilder.BuildKey(HohLocalizationCategory.Ages, string.Empty),
        HohLocalizationKeyBuilder.BuildKey(HohLocalizationCategory.BuildingGroups, string.Empty),
        HohLocalizationKeyBuilder.BuildKey(HohLocalizationCategory.Buildings, string.Empty),
        HohLocalizationKeyBuilder.BuildKey(HohLocalizationCategory.BuildingTypes, string.Empty),
        HohLocalizationKeyBuilder.BuildKey(HohLocalizationCategory.BuildingCustomizations, string.Empty),
        HohLocalizationKeyBuilder.BuildKey(HohLocalizationCategory.Cities, string.Empty),
        HohLocalizationKeyBuilder.BuildKey(HohLocalizationCategory.Continents, string.Empty),
        HohLocalizationKeyBuilder.BuildKey(HohLocalizationCategory.Heroes, string.Empty),
        HohLocalizationKeyBuilder.BuildKey(HohLocalizationCategory.HeroClass, string.Empty),
        HohLocalizationKeyBuilder.BuildKey(HohLocalizationCategory.Regions, string.Empty),
        HohLocalizationKeyBuilder.BuildKey(HohLocalizationCategory.TreasureHuntDifficulties, string.Empty),
        HohLocalizationKeyBuilder.BuildKey(HohLocalizationCategory.TreasureHuntDifficultiesPanel, string.Empty),
        HohLocalizationKeyBuilder.BuildKey(HohLocalizationCategory.Units, string.Empty),
        HohLocalizationKeyBuilder.BuildKey(HohLocalizationCategory.UnitTypes, string.Empty),
        HohLocalizationKeyBuilder.BuildKey(HohLocalizationCategory.Wonders, string.Empty),
        HohLocalizationKeyBuilder.BuildKey(HohLocalizationCategory.Technologies, string.Empty),
        HohLocalizationKeyBuilder.BuildKey(HohLocalizationCategory.Difficulties, string.Empty),
    };

    public void Parse(string? inputDirectory, IList<string> outputDirectories)
    {
        logger.LogInformation($"Starting parsing localization files: {string.Join(", ", HohSupportedCultures.AllCultures)}");

        foreach (var localeCode in HohSupportedCultures.AllCultures)
        {
            var filePath = GetInputFilePath(inputDirectory, localeCode);
            if (!File.Exists(filePath))
            {
                logger.LogWarning($"Could not find file: {filePath}");
                continue;
            }

            try
            {
                using var localizationFile = File.OpenRead(filePath);
                var localizationContainer = LocaResponseContainer.Parser.ParseFrom(localizationFile);
                var data = mapper.Map<LocalizationData>(localizationContainer.Data);
                var filteredData = new LocalizationData()
                {
                    Entries = data.Entries.Where(kvp => UsedLocalizationCategories.Any(s => kvp.Key.StartsWith(s)))
                        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
                };
                foreach (var outDir in outputDirectories)
                {
                    Directory.CreateDirectory(outDir);
                    var outputFilePath = GetOutputFilePath(outDir, localeCode);
                    protobufSerializer.SerializeToFile(filteredData, outputFilePath);
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Failed parsing file: {filePath}");
            }
        }
    }

    private string GetInputFilePath(string? inputDirectory, string localeCode)
    {
        var fileName = $"loca_{localeCode}.bin";
        return string.IsNullOrWhiteSpace(inputDirectory)
            ? fileName
            : $"{inputDirectory}{Path.DirectorySeparatorChar}{fileName}";
    }

    private string GetOutputFilePath(string outputDirectory, string localeCode)
    {
        var fileName = $"loca_parsed_{localeCode}.bin";
        return $"{outputDirectory}{Path.DirectorySeparatorChar}{fileName}";
    }
}
