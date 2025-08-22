using Ingweland.Fog.Application.Core.Factories.Interfaces;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Battle.Queries;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Models.Fog;
using Ingweland.Fog.Models.Hoh.Enums;
using MediatR;

namespace Ingweland.Fog.Application.Server.Services.Hoh;

public class BattleService(ISender sender, IBattleDefinitionIdFactory battleDefinitionIdFactory) : IBattleService
{
    public async Task<BattleSearchResult> SearchBattlesAsync(BattleSearchRequest request,
        CancellationToken ct = default)
    {
        var query = new BattleSearchQuery
        {
            BattleType = request.BattleType,
            BattleDefinitionId = battleDefinitionIdFactory.Create(request),
            UnitIds = request.UnitIds,
        };

        return await sender.Send(query, ct);
    }

    public async Task<BattleStatsDto?> GetBattleStatsAsync(int battleStatsId, CancellationToken ct = default)
    {
        var query = new GetBattleStatsQuery(battleStatsId);

        return await sender.Send(query, ct);
    }

    public Task<IReadOnlyCollection<UnitBattleDto>> GetUnitBattlesAsync(string unitId, BattleType battleType,
        CancellationToken ct = default)
    {
        var query = new GetUnitBattlesQuery(unitId, battleType);

        return sender.Send(query, ct);
    }

    public async Task<PaginatedList<BattleSummaryDto>> SearchBattlesAsync(UserBattleSearchRequest request,
        CancellationToken ct = default)
    {
        var query = new UserBattleSearchQuery
        {
            BattleType = request.BattleType,
            BattleDefinitionId = request.SearchRequest != null
                ? battleDefinitionIdFactory.Create(request.SearchRequest)
                : null,
            UnitIds = request.SearchRequest?.UnitIds ?? [],
            SubmissionId = request.SubmissionId,
            StartIndex = request.StartIndex,
            Count = request.Count,
        };

        return await sender.Send(query, ct);
    }

    public async Task<BattleDto?> GetBattleAsync(int battleId, CancellationToken ct = default)
    {
        var query = new GetBattleQuery(battleId);

        return await sender.Send(query, ct);
    }
}
