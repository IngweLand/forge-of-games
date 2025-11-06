using Azure.Storage.Blobs;

namespace Ingweland.Fog.Infrastructure.Repositories.Abstractions;

public interface IHohRawCoreDataAzureContainerClient
{
    BlobContainerClient Client { get; }
}
