using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Core.Extensions;

public static class BattleTypeExtensions
{
    public static BattleType ToBattleType(this string src)
    {
        if (string.IsNullOrWhiteSpace(src))
        {
            return BattleType.Undefined;
        }

        var lowerSrc = src.ToLowerInvariant();

        if (lowerSrc.StartsWith(nameof(RegionId.SiegeOfOrleans), StringComparison.InvariantCultureIgnoreCase))
        {
            return BattleType.HistoricBattle;
        }

        if (lowerSrc.StartsWith("encounter_", StringComparison.InvariantCultureIgnoreCase))
        {
            return BattleType.TreasureHunt;
        }

        if (lowerSrc.StartsWith("teslastorm", StringComparison.InvariantCultureIgnoreCase))
        {
            return BattleType.TeslaStorm;
        }

        if (lowerSrc.StartsWith("pvp", StringComparison.InvariantCultureIgnoreCase))
        {
            return BattleType.Pvp;
        }

        return BattleType.Campaign;
    }
}
