using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Server.PlayerCity.Abstractions;

public interface IHohCityCreationService
{
    Task<HohCity> Create(PlayerCitySnapshot citySnapshot, string playerName);
    Task<HohCity> Create(byte[] data, string playerName);
}
