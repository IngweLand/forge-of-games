using Ingweland.Fog.Dtos.Hoh.Units;

namespace Ingweland.Fog.Dtos.Hoh.Battle;

public class BattleSearchResult
{
    public static readonly BattleSearchResult Blank = new ();
    
    public IReadOnlyCollection<BattleSummaryDto> Battles { get; init; } = [];
}
