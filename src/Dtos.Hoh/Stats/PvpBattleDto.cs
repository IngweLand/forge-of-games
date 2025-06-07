using Ingweland.Fog.Models.Hoh.Entities.Battle;

namespace Ingweland.Fog.Dtos.Hoh.Stats;

public class PvpBattleDto
{
    public required PlayerDto Loser { get; init; }
    public required IReadOnlyCollection<PvpUnit> LoserUnits { get; init; }
    public required PlayerDto Winner { get; init; }
    public required IReadOnlyCollection<PvpUnit> WinnerUnits { get; init; }
}