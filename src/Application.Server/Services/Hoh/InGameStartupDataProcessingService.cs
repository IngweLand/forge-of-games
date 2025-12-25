using AutoMapper;
using Ingweland.Fog.Application.Core.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Core.Extensions;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.InnSdk.Hoh.Services.Abstractions;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Entities.Equipment;
using Ingweland.Fog.Models.Hoh.Entities.Research;
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
    IDataParsingService dataParsingService,
    ILogger<InGameStartupDataProcessingService> logger)
    : InGameDataProcessingServiceBase(logger), IInGameStartupDataProcessingService
{
    public async Task<InGameStartupData> ParseStartupData(string inputData)
    {
        var data = DecodeInternal(inputData);

        var communicationDto = dataParsingService.ParseCommunicationDto(data);
        communicationDto.LogIfFailed();
        if (communicationDto.IsFailed)
        {
            throw new InvalidOperationException("Failed to parse startup data");
        }

        var cities = await ImportInGameCities(communicationDto.Value);
        var profile = await ImportProfileAsync(communicationDto.Value, cities);
        var equipment = await ImportEquipmentAsync(communicationDto.Value);
        var researchState = await ImportResearchState(communicationDto.Value);
        return new InGameStartupData
        {
            Cities = cities.ToList(),
            Profile = profile,
            Equipment = equipment.ToList(),
            ResearchState = researchState,
        };
    }

    private async Task<IList<HohCity>> ImportInGameCities(CommunicationDto startupDto)
    {
        List<City> cityDtos;
        try
        {
            cityDtos = mapper.Map<List<City>>(startupDto.Cities);
        }
        catch (Exception ex)
        {
            const string msg = "Failed to map cities data";
            logger.LogError(ex, msg);
            return [];
        }

        var cities = new List<HohCity>();
        foreach (var cityDto in cityDtos)
        {
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

    private async Task<BasicCommandCenterProfile?> ImportProfileAsync(CommunicationDto startupDto,
        IEnumerable<HohCity> cities)
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

        IReadOnlyCollection<HeroProfileIdentifier> heroes;
        try
        {
            heroes = mapper.Map<IReadOnlyCollection<HeroProfileIdentifier>>(startupDto.HeroPush.Unlocked);

            return commandCenterProfileFactory.Create(heroes, barracksProfile);
        }
        catch (Exception ex)
        {
            const string msg = "Failed to map heroes data";
            logger.LogError(ex, msg);
        }

        return null;
    }

    private async Task<IList<EquipmentItem>> ImportEquipmentAsync(CommunicationDto startupDto)
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
        }

        return equipmentItems;
    }

    private async Task<IReadOnlyDictionary<CityId, IReadOnlyCollection<ResearchStateTechnology>>> ImportResearchState(
        CommunicationDto startupDto)
    {
        var researchItems = new Dictionary<CityId, IReadOnlyCollection<ResearchStateTechnology>>();
        if (startupDto.ResearchState == null)
        {
            return researchItems;
        }

        try
        {
            var items = mapper.Map<List<ResearchStateTechnology>>(startupDto.ResearchState.Technologies);
            foreach (var cityId in Enum.GetValues<CityId>())
            {
                if (cityId == CityId.Undefined)
                {
                    continue;
                }

                var technologies = (await coreDataRepository.GetTechnologiesAsync(cityId)).Select(x => x.Id)
                    .ToHashSet();
                if (technologies.Count == 0)
                {
                    continue;
                }

                researchItems[cityId] = items.Where(x => technologies.Contains(x.TechnologyId)).ToList();
            }
        }
        catch (Exception ex)
        {
            const string msg = "Failed to map research state data";
            logger.LogError(ex, msg);
        }

        return researchItems;
    }
}
