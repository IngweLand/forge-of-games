using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Server.PlayerCity.Abstractions;

public interface IPlayerCityService
{
    Task<byte[]?> FetchCityAsync(string gameWorldId, int inGamePlayerId, CityId cityId = CityId.Capital);
    Task<PlayerCitySnapshot?> SaveCityAsync(int playerId, byte[] data);
    Task<PlayerCitySnapshot?> GetCityAsync(int playerId, CityId cityId, DateOnly date);
}
