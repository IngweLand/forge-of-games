using Ingweland.Fog.Models.Hoh.Entities;

namespace Ingweland.Fog.Infrastructure.Repositories.Abstractions;

public interface IHohDataProvider
{
    Guid Version { get; }
    Task<Data> GetDataAsync();
}
