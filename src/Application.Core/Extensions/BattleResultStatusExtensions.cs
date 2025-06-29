using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Core.Extensions;

public static class BattleResultStatusExtensions
{
    public static BattleResultStatus Reverse(this BattleResultStatus src)
    {
        return src switch
        {
            BattleResultStatus.Win => BattleResultStatus.Defeat,
            BattleResultStatus.Defeat => BattleResultStatus.Win,
            _ => BattleResultStatus.Undefined,
        };
    }
}
