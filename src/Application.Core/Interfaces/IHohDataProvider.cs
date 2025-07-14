using Ingweland.Fog.Models.Hoh.Entities;

namespace Ingweland.Fog.Application.Core.Interfaces;

public interface IHohDataProvider
{
    Task<Data> GetDataAsync();
}
