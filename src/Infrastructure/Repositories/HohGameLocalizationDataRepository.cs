using Ingweland.Fog.Application.Core.Interfaces;
using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Application.Server.Settings;
using Ingweland.Fog.Infrastructure.Repositories.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Shared;
using Ingweland.Fog.Shared.Helpers;
using Ingweland.Fog.Shared.Helpers.Interfaces;
using Ingweland.Fog.Shared.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ingweland.Fog.Infrastructure.Repositories;

public class HohGameLocalizationDataRepository(
    IProtobufSerializer protobufSerializer,
    IOptionsMonitor<ResourceSettings> optionsMonitor,
    ILogger<HohGameLocalizationDataRepository> logger)
    :
        HohCoreDataRepositoryBase<HohGameLocalizationDataRepository>(optionsMonitor, logger),
        IHohGameLocalizationDataRepository
{
    private IDictionary<string, LocalizationData> _data = null!;

    public IReadOnlyDictionary<string, IReadOnlyCollection<string>> GetEntries(string cultureCode)
    {
        return _data.TryGetValue(cultureCode, out var localizationData)
            ? localizationData.Entries
            : _data[HohSupportedCultures.DefaultCulture].Entries;
    }

    public Task InitializeAsync()
    {
        throw new NotImplementedException();
    }

    private string GetDataFilePath(ResourceSettings options, string culture)
    {
        return $"{options.BaseUrl}/{options.HohLocalizationsDirectory}/loca_parsed_{culture}.bin";
    }

    protected override void Load(ResourceSettings options)
    {
        _data = new Dictionary<string, LocalizationData>();
        foreach (var culture in HohSupportedCultures.AllCultures)
        {
            var path = GetDataFilePath(options, culture);
            if (!File.Exists(path))
            {
                Logger.LogWarning($"Could not find localization file for the culture: {culture} at path: {path}");
                continue;
            }

            try
            {
                var bytes = File.ReadAllBytes(path);
                _data.Add(culture, protobufSerializer.DeserializeFromBytes<LocalizationData>(bytes));
                Logger.LogInformation($"Loaded localization for the culture: {culture}");
            }
            catch (Exception e)
            {
                Logger.LogError(e, $"Could not load localization for the culture: {culture} at path: {path}");
            }
        }

        if (!_data.ContainsKey(HohSupportedCultures.DefaultCulture))
        {
            throw new Exception($"Could not find default localization {HohSupportedCultures.DefaultCulture
            } after loading all files.");
        }
    }
}
