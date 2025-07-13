namespace Ingweland.Fog.Models.Hoh.Entities.Battle;

public class PvpBattle
{
    public required byte[] Id { get; init; }
    public required HohPlayer Loser { get; init; }
    public required IReadOnlyCollection<BattleSquad> LoserUnits { get; init; }
    public required DateTime PerformedAt { get; init; }
    public required HohPlayer Winner { get; init; }
    public required IReadOnlyCollection<BattleSquad> WinnerUnits { get; init; }
}