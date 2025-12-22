using Azure;
using Azure.Storage.Blobs;
using FluentResults;
using FluentResults.Extensions;
using Ingweland.Fog.Application.Server.Errors;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Shared.Utils;

namespace Ingweland.Fog.Infrastructure.Repositories;

public class BinaryDataStorageRepository(string connectionString, string containerName) : IBinaryDataStorageRepository
{
    private const int DATA_SIZE_LIMIT = 500 * 1024;

    private readonly Lazy<BlobContainerClient> _blobContainerClient =
        new(() => new BlobContainerClient(connectionString, containerName));

    public Task<Result<string>> GetAsStringAsync(string id)
    {
        var blobClient = _blobContainerClient.Value.GetBlobClient(id);

        return Result.Try(() => blobClient.DownloadContentAsync(),
                ex =>
                {
                    if (ex is RequestFailedException {Status: 404})
                    {
                        return new ResourceNotFoundError(id, containerName);
                    }

                    return new Error("An unexpected error occurred while retrieving data").CausedBy(ex);
                })
            .Bind(response =>
            {
                var compressed = response.Value.Content.ToArray();
                return Result.Ok(CompressionUtils.DecompressToString(compressed));
            });
    }

    public Task<Result> SaveAsync(string id, string data)
    {
        var container = _blobContainerClient.Value;

        var blobClient = container.GetBlobClient(id);
        var compressed = CompressionUtils.CompressString(data);
        if (compressed.Length > DATA_SIZE_LIMIT)
        {
            return Task.FromResult(Result.Fail(new DataTooLargeError(compressed.Length)));
        }

        return Result.Try(() => blobClient.UploadAsync(new BinaryData(compressed), false),
                ex => new Error("An unexpected error occurred while saving data").CausedBy(ex))
            .Bind(_ => Result.Ok());
    }

    public Task<Result<bool>> DeleteAsync(string id)
    {
        var blobClient = _blobContainerClient.Value.GetBlobClient(id);

        return Result.Try(() => blobClient.DeleteIfExistsAsync(),
                ex => new Error($"Failed to delete resource '{id}'").CausedBy(ex))
            .Bind(response => Result.Ok(response.Value));
    }

    public Task<Result<bool>> ExistsAsync(string id)
    {
        var blobClient = _blobContainerClient.Value.GetBlobClient(id);

        return Result.Try(() => blobClient.ExistsAsync(),
                ex => new Error($"Failed to check existence for resource '{id}'").CausedBy(ex))
            .Bind(response => Result.Ok(response.Value));
    }
}
