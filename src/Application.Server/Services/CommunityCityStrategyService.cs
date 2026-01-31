using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Services.Queries;
using Ingweland.Fog.Dtos.Hoh;
using MediatR;

namespace Ingweland.Fog.Application.Server.Services;

public class CommunityCityStrategyService(ISender sender) : ICommunityCityStrategyService
{
    public Task<IReadOnlyCollection<CommunityCityStrategyDto>> GetStrategiesAsync(CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<CommunityCityGuideInfoDto>> GetGuidesAsync(CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public async Task<CommunityCityGuideDto?> GetGuideAsync(int id, CancellationToken ct = default)
    {
        var query = new GetCommunityCityGuideQuery(id);
        var result = await sender.Send(query, ct);
        return result.IsSuccess ? result.Value : null;
    }
}
