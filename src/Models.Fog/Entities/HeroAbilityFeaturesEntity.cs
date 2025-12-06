namespace Ingweland.Fog.Models.Fog.Entities;

public class HeroAbilityFeaturesEntity
{
    public int Id { get; set; }
    public required string HeroId { get; set; }
    public required string Locale { get; set; }
    public required ISet<string> Tags { get; set; }
    public required ISet<string> Attributes { get; set; }
}
