using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;
using Ingweland.Fog.Models.Hoh.Entities.Battle;

namespace Ingweland.Fog.InnSdk.Hoh.Services.Abstractions;

public interface IBattleService
{
    Task<BattleStats> GetBattleStats(GameWorldConfig world, byte[] battleId);
}
