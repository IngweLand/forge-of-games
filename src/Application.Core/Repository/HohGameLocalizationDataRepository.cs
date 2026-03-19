using Ingweland.Fog.Application.Core.Repository.Abstractions;
using Ingweland.Fog.Shared.Localization;

namespace Ingweland.Fog.Application.Core.Repository;

public class HohGameLocalizationDataRepository(IHohLocalizationDataProvider dataProvider)
    : IHohGameLocalizationDataRepository
{
    public IReadOnlyDictionary<string, IReadOnlyCollection<string>> GetEntries(string cultureCode)
    {
        var data = dataProvider.GetData();
        return data.TryGetValue(cultureCode, out var localizationData)
            ? localizationData.Entries
            : data[HohSupportedCultures.DefaultCulture].Entries;
    }
}
