using Azure.Storage.Blobs;
using Ingweland.Fog.Application.Server.Settings;
using Ingweland.Fog.Infrastructure.Repositories.Abstractions;

namespace Ingweland.Fog.Infrastructure.Repositories;

public class HohRawCoreDataAzureContainerClient(StorageSettings storageSettings) : IHohRawCoreDataAzureContainerClient
{
    public BlobContainerClient Client { get; } =
        new(storageSettings.ConnectionString, storageSettings.HohRawCoreDataContainer);
}
