namespace Ingweland.Fog.Application.Server.Interfaces.Hoh;

public interface IHohGameLocalizationDataRepository
{
    IReadOnlyDictionary<string, IReadOnlyCollection<string>> GetEntries(string cultureCode);
}
