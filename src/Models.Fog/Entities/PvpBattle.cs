using System.Text.Json.Serialization;

namespace Ingweland.Fog.Models.Fog.Entities;

public class PvpBattle
{
    private BattleKey? _key;
    public int Id { get; set; }

    public required byte[] InGameBattleId { get; set; }

    [JsonIgnore]
    public BattleKey Key
    {
        get { return _key ??= new BattleKey(WorldId, InGameBattleId); }
    }

    public Player Loser { get; init; }
    public required string LoserUnits { get; init; }

    public required DateTime PerformedAt { get; set; }
    public Player Winner { get; init; }

    public required string WinnerUnits { get; init; }
    public required string WorldId { get; set; }
    public int WinnerId { get; init; }
    public int LoserId { get; init; }
}