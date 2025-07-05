namespace Ingweland.Fog.Application.Server.Interfaces;

public interface ICityExpansionsHasher
{
    ulong Compute(IEnumerable<string> unlockedExpansions);
}
