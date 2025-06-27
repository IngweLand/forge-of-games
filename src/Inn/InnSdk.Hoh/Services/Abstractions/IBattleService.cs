using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;
using Ingweland.Fog.Models.Hoh.Entities.Battle;

namespace Ingweland.Fog.InnSdk.Hoh.Services.Abstractions;

public interface IBattleService
{
    Task<BattleStats> GetBattleStatsAsync(GameWorldConfig world, byte[] battleId);
    Task<byte[]> GetBattleStatsRawDataAsync(GameWorldConfig world, byte[] battleId);
}
