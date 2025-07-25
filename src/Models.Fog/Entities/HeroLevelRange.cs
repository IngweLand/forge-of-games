namespace Ingweland.Fog.Models.Fog.Entities;

public record HeroLevelRange(int? From, int? To)
{
    public override string ToString()
    {
        if (!From.HasValue)
        {
            return $"< {To+1}";
        }

        if (!To.HasValue)
        {
            return $"{From}+";
        }

        return $"{From}-{To}";
    }
}

