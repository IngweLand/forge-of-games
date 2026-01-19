using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Server.Services.Hoh;

public class PlayerCityStrategyService:IPlayerCityStrategyService
{
    public Task<CityStrategy> GetPlayerCityStrategyAsync(int strategyId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
