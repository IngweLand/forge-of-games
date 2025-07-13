using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Models.Hoh.Entities.Battle;

namespace Ingweland.Fog.Dtos.Hoh.Stats;

public class PvpBattleDto
{
    public required PlayerDto Loser { get; init; }
    public required IReadOnlyCollection<BattleSquadDto> LoserUnits { get; init; }
    public required PlayerDto Winner { get; init; }
    public required IReadOnlyCollection<BattleSquadDto> WinnerUnits { get; init; }
    public int? StatsId { get; set; }
}