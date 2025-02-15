using System.Text.Json.Serialization;

namespace Ingweland.Fog.Models.Fog.Entities;

public class Player
{
    public required string Age { get; set; }
    public string? AllianceName { get; init; }
    public int AvatarId { get; set; }
    public required int InGamePlayerId { get; set; }
    public required string Name { get; set; }
    public required int Rank { get; set; }
    public required int RankingPoints { get; set; }
    public required DateOnly UpdatedAt { get; set; }
    public required string WorldId { get; set; }
    public ICollection<PlayerRanking> Rankings { get; set; } = new List<PlayerRanking>();

    private PlayerKey? _key;

    [JsonIgnore]
    public PlayerKey Key
    {
        get { return _key ??= new PlayerKey(WorldId, InGamePlayerId); }
    }
}
