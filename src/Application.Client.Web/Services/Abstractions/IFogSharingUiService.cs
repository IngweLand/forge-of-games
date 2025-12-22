using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.Services.Abstractions;

public interface IFogSharingUiService
{
    Task<CreatedShareDto> ShareAsync(SharedDataDto data);
    SharedDataDto CreateSharedData(CityStrategy cityStrategy);
    Task<bool> LoadCityStrategyAsync(string shareId);
}
