namespace Ingweland.Fog.Models.Fog.Entities;

public class PvpBattleTeams
{
    public int Id { get; set; }
    public required string LoserTeam { get; set; }
    public PvpBattle PvpBattle { get; set; } = null!;

    public int PvpBattleId { get; set; }
    public required string WinnerTeam { get; set; }
}
