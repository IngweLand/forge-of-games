using System.Text.Json.Serialization;
using Ingweland.Fog.Models.Fog.Enums;

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

    public ICollection<Player> MemberHistory { get; set; } = new List<Player>();
    public ICollection<AllianceMemberEntity> Members { get; set; } = new List<AllianceMemberEntity>();
    public DateTime MembersUpdatedAt { get; set; }
    public required string Name { get; set; }
    public ICollection<AllianceNameHistoryEntry> NameHistory { get; set; } = new List<AllianceNameHistoryEntry>();
    public int Rank { get; set; }
    public int RankingPoints { get; set; }
    public ICollection<AllianceRanking> Rankings { get; set; } = new List<AllianceRanking>();

    public DateTime? RegisteredAt { get; set; }
    public InGameEntityStatus Status { get; set; } = InGameEntityStatus.Active;
    public DateOnly UpdatedAt { get; set; }
    public required string WorldId { get; set; }
}
