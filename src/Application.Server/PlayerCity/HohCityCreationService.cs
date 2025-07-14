using Ingweland.Fog.Application.Core.Interfaces;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Application.Server.PlayerCity.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Services.Abstractions;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Server.PlayerCity;

public class HohCityCreationService(
    IHohCoreDataRepository coreDataRepository,
    IDataParsingService dataParsingService,
    IHohCityFactory cityFactory) : IHohCityCreationService
{
    public async Task<HohCity> Create(PlayerCitySnapshot citySnapshot, string playerName)
    {
        var otherCity = dataParsingService.ParseOtherCity(citySnapshot.Data);
        var buildings = await coreDataRepository.GetBuildingsAsync(CityId.Capital);

        var cityName = $"💡 {playerName} - {otherCity.CityId}";
        return cityFactory.Create(otherCity, buildings.ToDictionary(b => b.Id), WonderId.Undefined, 0, cityName);
    }
}
