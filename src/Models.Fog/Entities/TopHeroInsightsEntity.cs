using Ingweland.Fog.Models.Fog.Enums;

namespace Ingweland.Fog.Models.Fog.Entities;

public class TopHeroInsightsEntity
{
    public string? AgeId { get; set; }
    public required DateOnly CreatedAt { get; set; }
    public int? FromLevel { get; set; }
    public required ISet<string> Heroes { get; set; }
    public int Id { get; set; }
    public required HeroInsightsMode Mode { get; set; }
    public int? ToLevel { get; set; }
}
