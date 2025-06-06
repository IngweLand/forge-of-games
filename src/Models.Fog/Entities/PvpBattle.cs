using System.Text.Json.Serialization;

namespace Ingweland.Fog.Models.Fog.Entities;

public class PvpBattle
{
    private PvpBattleKey? _key;
    public int Id { get; set; }

    public required byte[] InGameBattleId { get; set; }

    [JsonIgnore]
    public PvpBattleKey Key
    {
        get { return _key ??= new PvpBattleKey(WorldId, InGameBattleId); }
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