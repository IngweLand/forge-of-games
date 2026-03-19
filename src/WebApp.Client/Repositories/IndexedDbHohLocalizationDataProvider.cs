using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Core.Repository.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Shared.Helpers.Interfaces;
using Ingweland.Fog.WebApp.Client.Repositories.Abstractions;
using Ingweland.Fog.WebApp.Client.Services.Abstractions;

namespace Ingweland.Fog.WebApp.Client.Repositories;

public class IndexedDbHohLocalizationDataProvider(
    IProtobufSerializer protobufSerializer,
    IHohDataService hohDataService,
    IFileCacheInteropService fileCacheInteropService,
    IClientLocaleService clientLocaleService,
    ILogger<IndexedDbHohLocalizationDataProvider> logger)
    : HohDataProviderBase<IDictionary<string, LocalizationData>>(logger),
        IHohLocalizationDataProvider
{
    protected override async Task<IDictionary<string, LocalizationData>> LoadAsync(string? version)
    {
        var culture = await clientLocaleService.GetCurrentLocaleAsync();
        var response = await hohDataService.GetHohLocalizationDataAsync(version);
        var data = response.Data;
        if (data != null)
        {
            var fileName = $"{response.CurrentVersion}/loca_parsed_{culture.Code}.bin";
            await fileCacheInteropService.SaveAsync(fileName, data);
            await fileCacheInteropService.SaveVersionAsync(response.CurrentVersion);
        }
        else
        {
            var fileName = $"{version}/loca_parsed_{culture.Code}.bin";
            data = await fileCacheInteropService.LoadAsync(fileName);
        }

        if (data == null)
        {
            throw new Exception("Could not load Hoh localization data");
        }

        return new Dictionary<string, LocalizationData>
            {{culture.Code, protobufSerializer.DeserializeFromBytes<LocalizationData>(data)}};
    }
}
