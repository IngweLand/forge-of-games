using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Battle.Queries;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Dtos.Hoh.Battle;
using MediatR;

namespace Ingweland.Fog.Application.Server.Services.Hoh;

public class BattleService(ISender sender, IBattleDefinitionIdFactory battleDefinitionIdFactory) : IBattleService
{
    public async Task<BattleSearchResult> SearchBattlesAsync(BattleSearchRequest request,
        CancellationToken ct = default)
    {
        var query = new BattleSearchQuery
        {
            BattleDefinitionId = await battleDefinitionIdFactory.Create(request),
            UnitIds = request.UnitIds,
        };

        return await sender.Send(query, ct);
    }

    public async Task<BattleStatsDto?> GetBattleStatsAsync(int battleStatsId, CancellationToken ct = default)
    {
        var query = new GetBattleStatsQuery(battleStatsId);

        return await sender.Send(query, ct);
    }
}
