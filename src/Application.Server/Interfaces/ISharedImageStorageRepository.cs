using FluentResults;

namespace Ingweland.Fog.Application.Server.Interfaces;

public interface ISharedImageStorageRepository
{
    Task<Result<string>> SaveAsync(string id, string contentType, byte[] data);
}
