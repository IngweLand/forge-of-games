using AutoMapper;
using Ingweland.Fog.Application.Core.Extensions;
using Ingweland.Fog.Application.Server.Factories;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Entities.Equipment;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Helpers;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Server.Services.Hoh;

public class InGameStartupDataProcessingService(
    IMapper mapper,
    IHohCityFactory cityFactory,
    IHohCoreDataRepository coreDataRepository,
    IBarracksProfileFactory barracksProfileFactory,
    ICommandCenterProfileFactory commandCenterProfileFactory,
    ILogger<InGameStartupDataProcessingService> logger)
    : InGameDataProcessingServiceBase(logger), IInGameStartupDataProcessingService
{
    public async Task<InGameStartupData> ParseStartupData(string inputData)
    {
        var data = DecodeInternal(inputData);
        
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
        var equipment = await ImportEquipmentAsync(startupDto);
        return new InGameStartupData()
        {
            Cities = cities.ToList(),
            Profile = profile,
            Equipment = equipment.ToList(),
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
            // TODO: remove this check once we can do something with other cities
            if (cityDto.CityId is not (CityId.Capital or CityId.Mayas_Tikal or CityId.Mayas_ChichenItza
                or CityId.Mayas_SayilPalace or CityId.China))
            {
                continue;
            }

            try
            {
                var buildings = await coreDataRepository.GetBuildingsAsync(cityDto.CityId);
                var wonderIds = cityDto.CityId.GetWonders();
                var activeWonderDto = startupDto.Wonders?.Wonders.FirstOrDefault(src => src.IsActive);
                var wonderId = WonderId.Undefined;
                if (activeWonderDto != null)
                {
                    var activeWonderId = HohStringParser.ParseEnumFromString2<WonderId>(activeWonderDto.Id, '_');
                    if (wonderIds.Contains(activeWonderId))
                    {
                        wonderId = activeWonderId;
                    }
                }
               
                var city = cityFactory.Create(cityDto, buildings.ToDictionary(b => b.Id), wonderId,
                    activeWonderDto?.Level ?? 0);
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

    private async Task<IList<EquipmentItem>> ImportEquipmentAsync(StartupDto startupDto)
    {
        var equipmentItems = new List<EquipmentItem>();

        if (startupDto.Equipment == null)
        {
            return equipmentItems;
        }

        try
        {
            equipmentItems = mapper.Map<List<EquipmentItem>>(startupDto.Equipment.Equipments);
        }
        catch (Exception ex)
        {
            const string msg = "Failed to map equipment data";
            logger.LogError(ex, msg);
            throw new InvalidOperationException(msg, ex);
        }

        return equipmentItems;
    }
}
