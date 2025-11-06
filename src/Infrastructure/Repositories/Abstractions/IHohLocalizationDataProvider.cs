using Ingweland.Fog.Models.Hoh.Entities;

namespace Ingweland.Fog.Infrastructure.Repositories.Abstractions;

public interface IHohLocalizationDataProvider
{
    IDictionary<string, LocalizationData> GetData();
}
