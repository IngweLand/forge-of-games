using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;

namespace Ingweland.Fog.InnSdk.Hoh.Services.Abstractions;

public interface ICityService
{
    Task<byte[]> GetOtherCityRawDataAsync(GameWorldConfig world, int playerId);
}
