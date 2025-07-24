using System.Text.Json.Serialization;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Models.Fog.Entities;

public class PlayerRanking
{
    public int Id { get; set; }
    public required DateOnly CollectedAt { get; set; }
    public required int Points { get; set; }
    public required int Rank { get; set; }
    public required PlayerRankingType Type { get; set; }
    public int PlayerId { get; set; }
}
