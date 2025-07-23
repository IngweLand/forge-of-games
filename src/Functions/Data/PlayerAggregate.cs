using System.Text.Json.Serialization;
using AutoMapper.Configuration.Annotations;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Functions.Data;

public class PlayerAggregate
{
    private PlayerKey? _key;
    public string? Age { get; set; }
    public string? AllianceName { get; set; }
    public int? AvatarId { get; set; }
    public required DateTime CollectedAt { get; set; }
    public int? InGameAllianceId { get; set; }
    public required int InGamePlayerId { get; set; } = -1;

    [JsonIgnore]
    [Ignore]
    public PlayerKey Key
    {
        get { return _key ??= new PlayerKey(WorldId, InGamePlayerId); }
    }

    public required string Name { get; set; }
    public PlayerRankingType? PlayerRankingType { get; set; }
    public IReadOnlyCollection<ProfileSquad> ProfileSquads { get; set; } = [];
    public int? PvpRank { get; set; }
    public int? PvpRankingPoints { get; set; }
    public string? PvpTier { get; set; }
    public int? Rank { get; set; }
    public int? RankingPoints { get; set; }
    public int? TreasureHuntDifficulty { get; set; }
    public required string WorldId { get; set; }

    public bool CanBeConvertedToPlayer()
    {
        return HasRequiredPropertiesSet() && Age != null;
    }

    public bool CanBeConvertedToPlayerRanking()
    {
        return HasRequiredPropertiesSet() && RankingPoints != null && Rank != null && PlayerRankingType != null;
    }

    public bool CanBeConvertedToPvpRanking()
    {
        return HasRequiredPropertiesSet() && PvpRank != null && PvpRankingPoints != null;
    }

    public bool CanUpdateHeroes()
    {
        return HasRequiredPropertiesSet() && ProfileSquads.Count > 0;
    }

    public bool HasRequiredPropertiesSet()
    {
        return InGamePlayerId > -1 && !string.IsNullOrWhiteSpace(Name) && CollectedAt != default &&
            !string.IsNullOrWhiteSpace(WorldId);
    }
}
