using System.Text.Json.Serialization;
using AutoMapper.Configuration.Annotations;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Functions.Data;

public class AllianceAggregate
{
    public int? AvatarBackgroundId { get; set; }
    public int? AvatarIconId { get; set; }
    public required int InGameAllianceId { get; set; } = -1;
    public required string Name { get; set; }
    public int? Rank { get; set; }
    public int? RankingPoints { get; set; }
    public int? AthRank { get; set; }
    public int? AthRankingPoints { get; set; }
    public int? AthEventId { get; set; }
    public required DateTime CollectedAt { get; set; }
    public required string WorldId { get; set; }
    public int? MemberInGameId { get; set; }
    public int? LeaderInGameId { get; set; }

    private AllianceKey? _key;

    [JsonIgnore]
    [Ignore]
    public AllianceKey Key
    {
        get { return _key ??= new AllianceKey(WorldId, InGameAllianceId); }
    }
    
    public AllianceRankingType? AllianceRankingType { get; set; }

    public bool CanBeConvertedToAlliance()
    {
        return HasRequiredPropertiesSet();
    }
    
    public bool CanBeConvertedToAllianceRanking()
    {
        return HasRequiredPropertiesSet() && RankingPoints != null && Rank != null && AllianceRankingType != null;
    }
    
    public bool CanBeConvertedToAthAllianceRanking()
    {
        return HasRequiredPropertiesSet() && AthRank != null && AthRankingPoints != null && AthEventId != null;
    }
    
    public bool CanBeConvertedToMember()
    {
        return HasRequiredPropertiesSet() && MemberInGameId != null;
    }
    
    public bool CanBeConvertedToLeader()
    {
        return HasRequiredPropertiesSet() && LeaderInGameId != null;
    }

    private bool HasRequiredPropertiesSet()
    {
        return InGameAllianceId >= 0 && !string.IsNullOrWhiteSpace(Name) && CollectedAt != default && !string.IsNullOrWhiteSpace(WorldId);
    }
}