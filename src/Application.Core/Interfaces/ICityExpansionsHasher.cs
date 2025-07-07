namespace Ingweland.Fog.Application.Core.Interfaces;

public interface ICityExpansionsHasher
{
    string Compute(IEnumerable<string> unlockedExpansions);
}
