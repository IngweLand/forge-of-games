using AutoMapper;
using Ingweland.Fog.Application.Server.Factories;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Server.Services.Hoh;

public class InGameStartupDataProcessingService(
    IMapper mapper,
    IHohCityFactory cityFactory,
    IHohCoreDataRepository coreDataRepository,
    IBarracksProfileFactory barracksProfileFactory,
    ICommandCenterProfileFactory commandCenterProfileFactory,
    ILogger<InGameStartupDataProcessingService> logger) : IInGameStartupDataProcessingService
{
    public async Task<InGameStartupData> ParseStartupData(string inputData)
    {
        byte[] data;
        try
        {
            data = Convert.FromBase64String(inputData);
        }
        catch (Exception ex)
        {
            const string msg = "Failed to decode Base64 string of startup data";
            logger.LogError(ex, msg);
            throw new InvalidOperationException(msg, ex);
        }

        if (data == null || data.Length == 0)
        {
            const string msg = "Startup data cannot be null or empty";
            logger.LogError(msg);
            throw new ArgumentException(msg, nameof(data));
        }

        StartupDto startupDto;
        try
        {
            startupDto = StartupDto.Parser.ParseFrom(data);
        }
        catch (Exception ex)
        {
            const string msg = "Failed to parse startup data";
            logger.LogError(ex, msg);
            throw new InvalidOperationException(msg, ex);
        }

        var cities = await ImportInGameCities(startupDto);
        var profile = await ImportProfileAsync(startupDto, cities);
        return new InGameStartupData()
        {
            Cities = cities.ToList(),
            Profile = profile,
        };
    }

    private async Task<IList<HohCity>> ImportInGameCities(StartupDto startupDto)
    {
        var cityDtos = new List<City>();
        try
        {
            cityDtos = mapper.Map<List<City>>(startupDto.Cities);
        }
        catch (Exception ex)
        {
            const string msg = "Failed to map cities data";
            logger.LogError(ex, msg);
            throw new InvalidOperationException(msg, ex);
        }

        var cities = new List<HohCity>();
        foreach (var cityDto in cityDtos)
        {
            //TODO: remove this check once we can do something with non-capital cities
            if (cityDto.CityId != CityId.Capital)
            {
                continue;
            }
            try
            {
                var buildings = await coreDataRepository.GetBuildingsAsync(cityDto.CityId);
                var city = cityFactory.Create(cityDto, buildings.ToDictionary(b => b.Id));
                cities.Add(city);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to create HohCity for cityId: {CityId}", cityDto.CityId);
            }
        }

        return cities;
    }

    private async Task<BasicCommandCenterProfile> ImportProfileAsync(StartupDto startupDto, IEnumerable<HohCity> cities)
    {
        var barracksProfile = new BarracksProfile();
        var capital = cities.FirstOrDefault(c => c.InGameCityId == CityId.Capital);
        if (capital != null)
        {
            var buildings = await coreDataRepository.GetBuildingsAsync(CityId.Capital);
            barracksProfile = barracksProfileFactory.Create(capital.Entities, buildings);
        }
        else
        {
            logger.LogError("Could not find the capital city during heroes import");
        }

        IReadOnlyCollection<BasicHeroProfile> heroes;
        try
        {
            heroes = mapper.Map<IReadOnlyCollection<BasicHeroProfile>>(startupDto.HeroPush.Unlocked);

            var profile = commandCenterProfileFactory.Create(heroes, barracksProfile);
            return profile;
        }
        catch (Exception ex)
        {
            const string msg = "Failed to map heroes data";
            logger.LogError(ex, msg);
            throw new InvalidOperationException(msg, ex);
        }
    }
}
