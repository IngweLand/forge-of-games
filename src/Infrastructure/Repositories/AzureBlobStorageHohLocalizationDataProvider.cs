using Ingweland.Fog.Application.Server.Settings;
using Ingweland.Fog.Infrastructure.Repositories.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Shared.Helpers.Interfaces;
using Ingweland.Fog.Shared.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ingweland.Fog.Infrastructure.Repositories;

public class AzureBlobStorageHohLocalizationDataProvider(
    IProtobufSerializer protobufSerializer,
    IOptionsMonitor<ResourceSettings> optionsMonitor,
    IHohCoreDataAzureContainerClient containerClient,
    ILogger<AzureBlobStorageHohLocalizationDataProvider> logger)
    : ReloadableDataProviderBase<IDictionary<string, LocalizationData>>(optionsMonitor, logger),
        IHohLocalizationDataProvider
{
    protected override async Task<IDictionary<string, LocalizationData>> LoadAsync(ResourceSettings options)
    {
        var data = new Dictionary<string, LocalizationData>();
        foreach (var culture in HohSupportedCultures.AllCultures)
        {
            try
            {
                var blobClient =
                    containerClient.Client.GetBlobClient($"{options.HohCoreDataVersion}/loca_parsed_{culture}.bin");
                var blobContent = await blobClient.DownloadContentAsync();
                var bytes = blobContent.Value.Content.ToArray();
                data.Add(culture, protobufSerializer.DeserializeFromBytes<LocalizationData>(bytes));
                Logger.LogInformation("Loaded localization: version {version}, culture: {Culture}",
                    options.HohCoreDataVersion, culture);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Could not load localization: version {version}, culture: {Culture}",
                    options.HohCoreDataVersion, culture);
            }
        }

        if (!data.ContainsKey(HohSupportedCultures.DefaultCulture))
        {
            throw new Exception($"Could not find default localization {HohSupportedCultures.DefaultCulture
            } after loading all files.");
        }

        return data;
    }
}
