using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;

public interface IBattleQueryService
{
    Task<IReadOnlyDictionary<byte[], int>> GetExistingBattleStatsIdsAsync(IEnumerable<byte[]> battleIds,
        CancellationToken cancellationToken);

    Task<IReadOnlyDictionary<byte[], BattleStatsEntity>> GetExistingBattleStatsAsync(IEnumerable<byte[]> battleIds,
        CancellationToken cancellationToken);
}
