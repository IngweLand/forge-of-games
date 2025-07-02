using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;

public interface IPlayerCityService
{
    Task<HohCity?> GetCityAsync(string gameWorldId, int playerId, string playerName);
}
