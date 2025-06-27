namespace Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;

public interface IBattleQueryService
{
    Task<IReadOnlyDictionary<byte[], int>> GetExistingBattleStatsIdsAsync(IEnumerable<byte[]> battleIds,
        CancellationToken cancellationToken);
}
