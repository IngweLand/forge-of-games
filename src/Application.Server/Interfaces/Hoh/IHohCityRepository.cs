using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Server.Interfaces.Hoh;

public interface IHohCityRepository
{
    Task<HohCity?> GetAsync(string cityId);
    Task<string> SaveAsync(HohCity city);
}
