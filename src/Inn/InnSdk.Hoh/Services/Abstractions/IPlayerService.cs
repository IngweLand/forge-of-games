using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;
using Ingweland.Fog.Models.Hoh.Entities;

namespace Ingweland.Fog.InnSdk.Hoh.Services.Abstractions;

public interface IPlayerService
{
    Task<byte[]> GetPlayerProfileRawDataAsync(GameWorldConfig world, int playerId);
    Task<PlayerProfile> GetPlayerProfileAsync(GameWorldConfig world, int playerId);
}
