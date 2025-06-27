using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.Shared.Utils;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Application.Server.Services.Hoh;

public class BattleQueryService(IFogDbContext context) : IBattleQueryService
{
    public async Task<IReadOnlyDictionary<byte[], int>> GetExistingBattleStatsIdsAsync(IEnumerable<byte[]> battleIds,
        CancellationToken cancellationToken)
    {
        var battleIdsSet = battleIds.ToHashSet(StructuralByteArrayComparer.Instance);
        return await context.BattleStats.AsNoTracking()
            .Where(e => battleIdsSet.Contains(e.InGameBattleId))
            .ToDictionaryAsync(src => src.InGameBattleId, src => src.Id, StructuralByteArrayComparer.Instance,
                cancellationToken);
    }
}
