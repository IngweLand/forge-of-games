using AutoMapper;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Shared.Helpers.Interfaces;
using Ingweland.Fog.Shared.Localization;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.HohCoreDataParserSdk;

public class LocalizationParser(
    IMapper mapper,
    IProtobufSerializer protobufSerializer,
    ILogger<LocalizationParser> logger)
{
    public IReadOnlyDictionary<string, byte[]> Parse(IReadOnlyDictionary<string, byte[]> source)
    {
        logger.LogInformation($"Starting parsing localization files: {
            string.Join(", ", HohSupportedCultures.AllCultures)}");

        var results = new Dictionary<string, byte[]>();
        foreach (var localeCode in HohSupportedCultures.AllCultures)
        {
            if (!source.TryGetValue(localeCode, out var localizationData))
            {
                logger.LogWarning("Missing localization data for {locale}", localeCode);
                continue;
            }

            try
            {
                var localizationContainer = CommunicationDto.Parser.ParseFrom(localizationData);
                var data = mapper.Map<LocalizationData>(localizationContainer.LocaResponse);
                var filteredData = new LocalizationData
                {
                    Entries = data.Entries,
                };
                results.Add($"loca_parsed_{localeCode}.bin", protobufSerializer.SerializeToBytes(filteredData));
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed parsing localization {locale}", localeCode);
            }
        }

        return results;
    }
}
