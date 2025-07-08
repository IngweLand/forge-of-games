using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Server.PlayerCity.Abstractions;

public interface IFailedPlayerCityFetchesCache
{
    bool IsFailedFetch(PlayerKey key);
    void AddFailedFetch(PlayerKey key);
    void RemoveFailedFetchAsync(PlayerKey key);
}
