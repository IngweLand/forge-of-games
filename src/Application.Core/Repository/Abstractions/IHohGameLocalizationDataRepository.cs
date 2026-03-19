namespace Ingweland.Fog.Application.Core.Repository.Abstractions;

public interface IHohGameLocalizationDataRepository
{
    IReadOnlyDictionary<string, IReadOnlyCollection<string>> GetEntries(string cultureCode);
}
