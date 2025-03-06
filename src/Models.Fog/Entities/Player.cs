using System.Text.Json.Serialization;

namespace Ingweland.Fog.Models.Fog.Entities;

public class Player
{
    private PlayerKey? _key;
    public required string Age { get; set; }
    public ICollection<PlayerAgeHistoryEntry> AgeHistory { get; set; } = new List<PlayerAgeHistoryEntry>();

    public ICollection<PlayerAllianceNameHistoryEntry> AllianceNameHistory { get; set; } =
        new List<PlayerAllianceNameHistoryEntry>();

    public string? AllianceName { get; set; }
    public int AvatarId { get; set; }
    public int Id { get; set; }
    public required int InGamePlayerId { get; set; }

    [JsonIgnore]
    public PlayerKey Key
    {
        get { return _key ??= new PlayerKey(WorldId, InGamePlayerId); }
    }

    public required string Name { get; set; }
    public ICollection<PlayerNameHistoryEntry> NameHistory { get; set; } = new List<PlayerNameHistoryEntry>();
    public ICollection<PvpRanking> PvpRankings { get; set; } = new List<PvpRanking>();
    public int? Rank { get; set; }
    public int? RankingPoints { get; set; }
    public ICollection<PlayerRanking> Rankings { get; set; } = new List<PlayerRanking>();
    public ICollection<Alliance> AllianceHistory { get; set; } = new List<Alliance>();
    public Alliance? CurrentAlliance { get; set; }
    public Alliance? LedAlliance { get; set; }
    public required DateOnly UpdatedAt { get; set; }
    public required string WorldId { get; set; }
}