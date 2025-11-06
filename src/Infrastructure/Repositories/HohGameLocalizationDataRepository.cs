using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Infrastructure.Repositories.Abstractions;
using Ingweland.Fog.Shared.Localization;

namespace Ingweland.Fog.Infrastructure.Repositories;

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
