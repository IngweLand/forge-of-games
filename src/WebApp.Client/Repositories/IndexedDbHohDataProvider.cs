using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Core.Repository.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Shared.Helpers.Interfaces;
using Ingweland.Fog.WebApp.Client.Repositories.Abstractions;

namespace Ingweland.Fog.WebApp.Client.Repositories;

public class IndexedDbHohDataProvider(
    IProtobufSerializer protobufSerializer,
    IHohDataService hohDataService,
    IFileCacheInteropService fileCacheInteropService,
    ILogger<IndexedDbHohDataProvider> logger)
    : HohDataProviderBase<Data>(logger), IHohDataProvider
{
    protected override async Task<Data> LoadAsync(string? version)
    {
        var response = await hohDataService.GetHohDataAsync(version);
        var data = response.Data;
        if (data != null)
        {
            var fileName = $"{response.CurrentVersion}/data.bin";
            await fileCacheInteropService.SaveAsync(fileName, data);
            await fileCacheInteropService.SaveVersionAsync(response.CurrentVersion);
        }
        else
        {
            var fileName = $"{version}/data.bin";
            data = await fileCacheInteropService.LoadAsync(fileName);
        }

        if (data == null)
        {
            throw new Exception("Could not load Hoh data");
        }

        return protobufSerializer.DeserializeFromBytes<Data>(data);
    }
}
