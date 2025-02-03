using Ingweland.Fog.Models.Hoh.Entities;

namespace Ingweland.Fog.Infrastructure.Repositories.Abstractions;

public interface IHohDataProvider
{
    Task<Data> GetDataAsync();
}
