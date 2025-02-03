namespace Ingweland.Fog.Application.Server.Interfaces.Hoh;

public interface IHohGameLocalizationDataRepository : IDisposable
{
    IReadOnlyDictionary<string, IReadOnlyCollection<string>> GetEntries(string cultureCode);
}
