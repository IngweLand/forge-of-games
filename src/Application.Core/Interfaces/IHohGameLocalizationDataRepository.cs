namespace Ingweland.Fog.Application.Core.Interfaces;

public interface IHohGameLocalizationDataRepository : IDisposable
{
    IReadOnlyDictionary<string, IReadOnlyCollection<string>> GetEntries(string cultureCode);
    Task InitializeAsync();
}
