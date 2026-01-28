using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Dtos.Hoh;

namespace Ingweland.Fog.Application.Server.Services;

public class SharedCityStrategyService : ISharedCityStrategyService
{
    public Task<IReadOnlyCollection<CommunityCityStrategyDto>> GetAllAsync(CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
