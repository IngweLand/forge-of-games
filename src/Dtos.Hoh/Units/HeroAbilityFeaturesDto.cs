namespace Ingweland.Fog.Dtos.Hoh.Units;

public class HeroAbilityFeaturesDto
{
    public required string HeroId { get; init; }
    public required ISet<string> Tags { get; init; }
}
