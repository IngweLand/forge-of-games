using Ingweland.Fog.Models.Hoh.Entities;

namespace Ingweland.Fog.Application.Core.Repository.Abstractions;

public interface IHohDataProvider
{
    Guid Version { get; }
    Task<Data> GetDataAsync();
}
