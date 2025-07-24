namespace Ingweland.Fog.Models.Fog.Entities;

public record HeroLevelRange(int From, int To)
{
    public override string ToString()
    {
        if (From == -1)
        {
            return $"< {To+1}";
        }

        if (To == -1)
        {
            return $"{From}+";
        }

        return $"{From}-{To}";
    }
}

