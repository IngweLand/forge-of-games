using System.Text.Json.Serialization;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Models.Fog.Entities;

public class AllianceRanking
{
    public required DateOnly CollectedAt { get; set; }
    public required int InGameAllianceId { get; set; }
    public required string Name { get; set; }
    public required int Points { get; set; }
    public required int Rank { get; set; }
    public required AllianceRankingType Type { get; set; }
    public required string WorldId { get; set; }
    public required int MemberCount { get; set; }
    
    private AllianceRankingKey? _key;

    [JsonIgnore]
    public AllianceRankingKey Key
    {
        get { return _key ??= new AllianceRankingKey(WorldId, InGameAllianceId, CollectedAt); }
    }
}