using System.Text.Json.Serialization;

namespace Ingweland.Fog.Models.Fog.Entities;

public class Alliance
{
    private AllianceKey? _key;
    public int AvatarBackgroundId { get; set; }
    public int AvatarIconId { get; set; }
    public int Id { get; set; }
    public required int InGameAllianceId { get; set; }

    [JsonIgnore]
    public AllianceKey Key
    {
        get { return _key ??= new AllianceKey(WorldId, InGameAllianceId); }
    }

    public Player? Leader { get; set; }
    public int? LeaderId { get; set; }
    public ICollection<Player> MemberHistory { get; set; } = new List<Player>();
    public ICollection<Player> Members { get; set; } = new List<Player>();
    public required string Name { get; set; }
    public ICollection<AllianceNameHistoryEntry> NameHistory { get; set; } = new List<AllianceNameHistoryEntry>();
    public int Rank { get; set; }
    public int RankingPoints { get; set; }
    public ICollection<AllianceRanking> Rankings { get; set; } = new List<AllianceRanking>();

    public DateTime? RegisteredAt { get; set; }
    public required DateOnly UpdatedAt { get; set; }
    public required string WorldId { get; set; }
}