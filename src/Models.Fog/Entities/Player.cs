using System.Text.Json.Serialization;

namespace Ingweland.Fog.Models.Fog.Entities;

public class Player
{
    private PlayerKey? _key;
    public required string Age { get; set; }
    public ICollection<PlayerAgeHistoryEntry> AgeHistory { get; set; } = new List<PlayerAgeHistoryEntry>();
    public ICollection<Alliance> AllianceHistory { get; set; } = new List<Alliance>();

    public string? AllianceName { get; set; }

    public ICollection<PlayerAllianceNameHistoryEntry> AllianceNameHistory { get; set; } =
        new List<PlayerAllianceNameHistoryEntry>();

    public int AvatarId { get; set; }
    public ICollection<PlayerCitySnapshot> CitySnapshots { get; set; } = new List<PlayerCitySnapshot>();
    public Alliance? CurrentAlliance { get; set; }
    public int Id { get; set; }
    public required int InGamePlayerId { get; set; }

    public bool IsPresentInGame { get; set; } = true;

    [JsonIgnore]
    public PlayerKey Key
    {
        get { return _key ??= new PlayerKey(WorldId, InGamePlayerId); }
    }

    public Alliance? LedAlliance { get; set; }

    public required string Name { get; set; }
    public ICollection<PlayerNameHistoryEntry> NameHistory { get; set; } = new List<PlayerNameHistoryEntry>();
    public ICollection<PvpBattle> PvpLosses { get; set; } = new List<PvpBattle>();
    public ICollection<PvpRanking> PvpRankings { get; set; } = new List<PvpRanking>();
    public ICollection<PvpBattle> PvpWins { get; set; } = new List<PvpBattle>();
    public int? Rank { get; set; }
    public int? RankingPoints { get; set; }
    public ICollection<PlayerRanking> Rankings { get; set; } = new List<PlayerRanking>();
    public required DateOnly UpdatedAt { get; set; }
    public required string WorldId { get; set; }
}
