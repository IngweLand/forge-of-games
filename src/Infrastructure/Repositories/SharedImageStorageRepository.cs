using System.Net.Mime;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FluentResults;
using FluentResults.Extensions;
using Ingweland.Fog.Application.Server.Errors;
using Ingweland.Fog.Application.Server.Interfaces;

namespace Ingweland.Fog.Infrastructure.Repositories;

public class SharedImageStorageRepository(string connectionString, string containerName) : ISharedImageStorageRepository
{
    private const int DATA_SIZE_LIMIT = 200 * 1024;

    private static readonly IReadOnlyDictionary<string, string> ContentTypeToExtensionMap =
        new Dictionary<string, string>
        {
            [MediaTypeNames.Image.Jpeg] = ".jpg",
            [MediaTypeNames.Image.Png] = ".png",
        };

    private readonly Lazy<BlobContainerClient> _blobContainerClient =
        new(() => new BlobContainerClient(connectionString, containerName));

    public Task<Result<string>> SaveAsync(string id, string contentType, byte[] data)
    {
        var container = _blobContainerClient.Value;
        var extension = ContentTypeToExtensionMap.GetValueOrDefault(contentType, string.Empty);
        var fileName = $"{id}{extension}";
        var blobClient = container.GetBlobClient(fileName);
        if (data.Length > DATA_SIZE_LIMIT)
        {
            return Task.FromResult(Result.Fail<string>(new DataTooLargeError(data.Length)));
        }

        var blobHttpHeaders = new BlobHttpHeaders
        {
            ContentType = contentType,
            CacheControl = "public, max-age=31536000, immutable", // 1 year cache
        };
        return Result.Try(() => blobClient.UploadAsync(new BinaryData(data), new BlobUploadOptions
                {
                    HttpHeaders = blobHttpHeaders,
                    Conditions = new BlobRequestConditions {IfNoneMatch = new ETag("*")}, // disable override
                }),
                ex => new Error("An unexpected error occurred while saving data").CausedBy(ex))
            .Bind(_ => Result.Ok(blobClient.Uri.ToString()));
    }
}
