using System.Text.Json.Serialization;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Models.Fog.Entities;

public class PlayerRanking
{
    public required string Age { get; set; }
    public string? AllianceName { get; set; }
    public required DateOnly CollectedAt { get; set; }
    public required int InGamePlayerId { get; set; }
    public required string Name { get; set; }
    public required int Points { get; set; }
    public required int Rank { get; set; }
    public required PlayerRankingType Type { get; set; }
    public required string WorldId { get; set; }
    
    private PlayerRankingKey? _key;

    [JsonIgnore]
    public PlayerRankingKey Key
    {
        get { return _key ??= new PlayerRankingKey(WorldId, InGamePlayerId, CollectedAt); }
    }
}
