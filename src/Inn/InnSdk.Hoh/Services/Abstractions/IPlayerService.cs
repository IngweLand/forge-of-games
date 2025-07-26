using FluentResults;
using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;
using Ingweland.Fog.Models.Hoh.Entities;

namespace Ingweland.Fog.InnSdk.Hoh.Services.Abstractions;

public interface IPlayerService
{
    Task<Result<byte[]>> GetPlayerProfileRawDataAsync(GameWorldConfig world, int playerId);
    Task<Result<PlayerProfile>> GetPlayerProfileAsync(GameWorldConfig world, int playerId);
}
