using FluentResults;

namespace Ingweland.Fog.Application.Server.Interfaces;

public interface IBinaryDataStorageRepository
{
    Task<Result<string>> GetAsStringAsync(string id);
    Task<Result> SaveAsync(string id, string data);
    Task<Result<bool>> DeleteAsync(string id);
    Task<Result<bool>> ExistsAsync(string id);
}
