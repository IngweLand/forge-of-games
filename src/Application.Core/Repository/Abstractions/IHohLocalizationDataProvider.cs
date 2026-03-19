using Ingweland.Fog.Models.Hoh.Entities;

namespace Ingweland.Fog.Application.Core.Repository.Abstractions;

public interface IHohLocalizationDataProvider
{
    IDictionary<string, LocalizationData> GetData();
}
