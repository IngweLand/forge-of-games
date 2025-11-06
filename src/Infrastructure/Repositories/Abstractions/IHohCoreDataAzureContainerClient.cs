using Azure.Storage.Blobs;

namespace Ingweland.Fog.Infrastructure.Repositories.Abstractions;

public interface IHohCoreDataAzureContainerClient
{
    BlobContainerClient Client { get; }
}
