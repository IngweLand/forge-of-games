using System.Text.Json.Serialization;

namespace Ingweland.Fog.Models.Fog.Entities;

public class Alliance
{
    public int AvatarBackgroundId { get; init; }
    public int AvatarIconId { get; init; }
    public required int InGameAllianceId { get; set; }
    public required int MemberCount { get; set; }
    public required string Name { get; set; }
    public required int Rank { get; set; }
    public required int RankingPoints { get; set; }
    public required DateOnly UpdatedAt { get; set; }
    public required string WorldId { get; set; }
    public ICollection<AllianceRanking> Rankings { get; set; } = new List<AllianceRanking>();

    private AllianceKey? _key;

    [JsonIgnore]
    public AllianceKey Key
    {
        get { return _key ??= new AllianceKey(WorldId, InGameAllianceId); }
    }
}