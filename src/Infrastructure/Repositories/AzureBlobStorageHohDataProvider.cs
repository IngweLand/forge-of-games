using Ingweland.Fog.Application.Server.Settings;
using Ingweland.Fog.Infrastructure.Repositories.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Shared.Helpers.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ingweland.Fog.Infrastructure.Repositories;

public class AzureBlobStorageHohDataProvider(
    IProtobufSerializer protobufSerializer,
    IOptionsMonitor<ResourceSettings> optionsMonitor,
    IHohCoreDataAzureContainerClient containerClient,
    ILogger<AzureBlobStorageHohDataProvider> logger)
    : ReloadableDataProviderBase<Data>(optionsMonitor, logger), IHohDataProvider
{
    protected override async Task<Data> LoadAsync(ResourceSettings options)
    {
        var blobClient = containerClient.Client.GetBlobClient($"{options.HohCoreDataVersion}/data.bin");
        var blobContent = await blobClient.DownloadContentAsync();
        var bytes = blobContent.Value.Content.ToArray();
        return protobufSerializer.DeserializeFromBytes<Data>(bytes);
    }
}
