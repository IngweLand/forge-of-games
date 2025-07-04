using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.InnSdk.Hoh.Services.Abstractions;

public interface ICityService
{
    Task<byte[]> GetOtherCityRawDataAsync(GameWorldConfig world, int playerId, string cityId);
}
