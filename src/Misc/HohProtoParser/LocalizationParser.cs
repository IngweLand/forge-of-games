using AutoMapper;
using Ingweland.Fog.Application.Server.Enums.Hoh;
using Ingweland.Fog.Application.Server.Helpers;
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
                    Entries = data.Entries,
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
