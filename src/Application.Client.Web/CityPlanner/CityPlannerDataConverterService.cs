using System.Drawing;
using System.Net;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Core.CityPlanner;
using Ingweland.Fog.Application.Core.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Core.Extensions;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Dtos.Hoh.CityPlanner;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Utils;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner;

public class CityPlannerDataConverterService(
    IHohCityFactory cityFactory,
    ICityService cityService,
    IPersistenceService persistenceService,
    ILogger<CityPlannerDataConverterService> logger) : ICityPlannerDataConverterService
{
    private static readonly IReadOnlyDictionary<CityId, Point> CoordinatesMap = new Dictionary<CityId, Point>
    {
        [CityId.Capital] = new(11, -51),
        [CityId.Egypt] = new(19, -51),
        [CityId.China] = new(11, -51),
        [CityId.Mayas_Tikal] = new(19, -51),
        [CityId.Mayas_ChichenItza] = new(19, -51),
        [CityId.Mayas_SayilPalace] = new(19, -51),
        [CityId.Vikings] = new(12, -48),
        [CityId.Arabia_CityOfBrass] = new(6, -51),
        [CityId.Arabia_NoriasOfHama] = new(6, -51),
        [CityId.Arabia_Petra] = new(6, -51),
    };

    private static readonly IReadOnlyDictionary<string, string> DefIdMap = new Dictionary<string, string>
    {
        ["mbcv"] = "Building_Capital_Barracks_Cavalry",
        ["mbhi"] = "Building_Capital_Barracks_HeavyInfantry",
        ["mbif"] = "Building_Capital_Barracks_Infantry",
        ["mbrg"] = "Building_Capital_Barracks_Ranged",
        ["mbsg"] = "Building_Capital_Barracks_Siege",
        ["mchl"] = "Building_Capital_City_CityHall",
        ["mamp"] = "Building_Capital_Collectable_Amphitheatre",
        ["marc"] = "Building_Capital_Collectable_ArchitectsStudioV2",
        ["mmin"] = "Building_Capital_Collectable_MinoanWatchtowerV2",
        ["msch"] = "Building_Capital_Collectable_SchoolV2",
        ["mcco"] = "Building_Capital_CultureSite_Compact",
        ["mcla"] = "Building_Capital_CultureSite_Large",
        ["mcli"] = "Building_Capital_CultureSite_Little",
        ["mcmo"] = "Building_Capital_CultureSite_Moderate",
        ["mcpr"] = "Building_Capital_CultureSite_Premium",
        ["mfoy"] = "Building_Capital_Evolving_FountainOfYouth",
        ["mggr"] = "Building_Capital_Evolving_GreatGarden",
        ["mfdo"] = "Building_Capital_Farm_Domestic",
        ["mfpr"] = "Building_Capital_Farm_Premium",
        ["mfru"] = "Building_Capital_Farm_Rural",
        ["mhav"] = "Building_Capital_Home_Average",
        ["mhpr"] = "Building_Capital_Home_Premium",
        ["mhsm"] = "Building_Capital_Home_Small",
        ["mfur"] = "Building_Capital_Special_Furnace",
        ["mhha"] = "Building_Capital_Special_HeroAcademy",
        ["macl"] = "Building_Capital_Workshop_Alchemist",
        ["mwar"] = "Building_Capital_Workshop_Artisan",
        ["mwca"] = "Building_Capital_Workshop_Carpenter",
        ["mgls"] = "Building_Capital_Workshop_Glassblower",
        ["mjwl"] = "Building_Capital_Workshop_Jeweler",
        ["mwsc"] = "Building_Capital_Workshop_Scribe",
        ["mwsm"] = "Building_Capital_Workshop_SpiceMerchant",
        ["mwst"] = "Building_Capital_Workshop_StoneMason",
        ["mwta"] = "Building_Capital_Workshop_Tailor",
        ["cchl"] = "Building_China_City_CityHall",
        ["cekq"] = "Building_China_ExtractionPoint_KaolinQuarry",
        ["cemg"] = "Building_China_ExtractionPoint_MothGlade",
        ["chav"] = "Building_China_Home_Average",
        ["chpr"] = "Building_China_Home_Premium",
        ["chsm"] = "Building_China_Home_Small",
        ["cebr"] = "Building_China_Manual_EasternBridge",
        ["csbr"] = "Building_China_Manual_SouthernBridge",
        ["crfa"] = "Building_China_RiceFarm_Average",
        ["crfp"] = "Building_China_RiceFarm_Premium",
        ["cwcp"] = "Building_China_Workshop_ClayProcessor",
        ["cwpw"] = "Building_China_Workshop_PorcelainWorkshop",
        ["cwsw"] = "Building_China_Workshop_SilkWorkshop",
        ["cwtp"] = "Building_China_Workshop_ThreadProcessor",
        ["echl"] = "Building_Egypt_City_CityHall",
        ["egma"] = "Building_Egypt_GoldMine_Average",
        ["egmp"] = "Building_Egypt_GoldMine_Premium",
        ["ehav"] = "Building_Egypt_Home_Average",
        ["ehpr"] = "Building_Egypt_Home_Premium",
        ["ehms"] = "Building_Egypt_Home_Small",
        ["eich"] = "Building_Egypt_Irrigation_Channel",
        ["eift"] = "Building_Egypt_Irrigation_Fountain",
        ["eist"] = "Building_Egypt_Irrigation_IrrigationStation",
        ["eioa"] = "Building_Egypt_Irrigation_Oasis",
        ["eisw"] = "Building_Egypt_Irrigation_SmallWell",
        ["eiwp"] = "Building_Egypt_Irrigation_WaterPump",
        ["enil"] = "Building_Egypt_Manual_NileBridge",
        ["epfa"] = "Building_Egypt_PapyrusField_Average",
        ["epfp"] = "Building_Egypt_PapyrusField_Premium",
        ["egds"] = "Building_Egypt_Workshop_Goldsmith",
        ["eppr"] = "Building_Egypt_Workshop_PapyrusPress",
        ["myaa"] = "Building_Mayas_Aviary_Average",
        ["myap"] = "Building_Mayas_Aviary_Premium",
        ["mych"] = "Building_Mayas_City_CityHall",
        ["myhg"] = "Building_Mayas_Home_Premium",
        ["myhp"] = "Building_Mayas_Home_Priest",
        ["myhw"] = "Building_Mayas_Home_Worker",
        ["myqj"] = "Building_Mayas_Quarry_Jade",
        ["myqo"] = "Building_Mayas_Quarry_Obsidian",
        ["myqp"] = "Building_Mayas_Quarry_Premium",
        ["myra"] = "Building_Mayas_RitualSite_Average",
        ["myrg"] = "Building_Mayas_RitualSite_Premium",
        ["mywc"] = "Building_Mayas_Workshop_CeremonyOutfitter",
        ["mywh"] = "Building_Mayas_Workshop_Chronicler",
        ["mywm"] = "Building_Mayas_Workshop_MaskSculptor",
        ["mywg"] = "Building_Mayas_Workshop_Premium",
        ["mywr"] = "Building_Mayas_Workshop_RitualCarver",
        ["vbha"] = "Building_Vikings_Beehive_Average",
        ["vchl"] = "Building_Vikings_City_CityHall",
        ["vepa"] = "Building_Vikings_ExpeditionPier_Average",
        ["vfpa"] = "Building_Vikings_FishingPier_Average",
        ["vfpp"] = "Building_Vikings_FishingPier_Premium",
        ["vhpr"] = "Building_Vikings_Home_Premium",
        ["vhsl"] = "Building_Vikings_Home_Sailor",
        ["vhwk"] = "Building_Vikings_Home_Worker",
        ["vmbr"] = "Building_Vikings_Manual_Bridge",
        ["vspa"] = "Building_Vikings_SailorPort_Average",
        ["vspp"] = "Building_Vikings_SailorPort_Premium",
        ["vtav"] = "Building_Vikings_Workshop_Tavern",
    };

    private static readonly IDictionary<string, CityId> CityIdMap = new Dictionary<string, CityId>
    {
        ["china"] = CityId.China,
        ["egypt"] = CityId.Egypt,
        ["mayas"] = CityId.Mayas_ChichenItza,
        ["vikings"] = CityId.Vikings,
        ["capital"] = CityId.Capital,
    };

    private static readonly IDictionary<string, WonderId> WonderIdMap = new Dictionary<string, WonderId>
    {
        ["wcfc"] = WonderId.China_ForbiddenCity,
        ["wcgw"] = WonderId.China_GreatWall,
        ["wcta"] = WonderId.China_TerracottaArmy,
        ["weas"] = WonderId.Egypt_AbuSimbel,
        ["wecp"] = WonderId.Egypt_CheopsPyramid,
        ["wegs"] = WonderId.Egypt_GreatSphinx,
        ["wmci"] = WonderId.Mayas_ChichenItza,
        ["wmsp"] = WonderId.Mayas_SayilPalace,
        ["wmtk"] = WonderId.Mayas_Tikal,
        ["wvde"] = WonderId.Vikings_DragonshipEllida,
        ["wvva"] = WonderId.Vikings_Valhalla,
        ["wvyg"] = WonderId.Vikings_Yggdrasil,
    };

    private readonly Dictionary<CityId, CityPlannerDataDto> _cityPlannerData = new();

    public async Task ConvertAsync(string compressedData, string cityName)
    {
        var decoded = WebUtility.UrlDecode(compressedData);
        var decompressed = CompressionUtils.DecompressFromLzString(decoded);

        logger.LogInformation($"Decompressed data: {decompressed}");

        var dataItems = decompressed.Split(';');

        var city = CreateBaseCity(dataItems[0], cityName);
        await AddExpansionsAsync(city, dataItems[1]);
        await AddCityMapEntitiesAsync(city, dataItems.Skip(2));

        await persistenceService.SaveCity(city);
    }

    private async Task<CityPlannerDataDto> GetCityPlannerDataAsync(CityId cityId)
    {
        if (_cityPlannerData.TryGetValue(cityId, out var data))
        {
            return data;
        }

        data = await cityService.GetCityPlannerDataAsync(cityId);
        if (data == null)
        {
            throw new Exception($"Failed to get city planner data for city id: {cityId}");
        }

        _cityPlannerData.Add(cityId, data);
        return data;
    }

    private HohCity CreateBaseCity(string metadata, string cityName)
    {
        var parts = metadata.Split('.');
        if (parts[0] != "v1")
        {
            throw new Exception($"Unrecognized metadata version: {parts[0]}");
        }

        if (!CityIdMap.TryGetValue(parts[1], out var cityId))
        {
            throw new Exception($"Unrecognized city id: {parts[1]}");
        }

        if (cityId == CityId.Capital)
        {
            throw new Exception("Capital city is not supported yet.");
        }

        var wonderLevel = 0;
        if (!WonderIdMap.TryGetValue(parts[2], out var wonderId))
        {
            throw new Exception($"Unrecognized wonder id: {parts[2]}");
        }

        if (!string.IsNullOrWhiteSpace(parts[3]) && !int.TryParse(parts[3], out wonderLevel))
        {
            throw new Exception($"Failed to parse wonder level: {parts[3]}");
        }

        if (wonderId != WonderId.Undefined)
        {
            cityId = wonderId.ToCity();
        }

        var cityRequest = new NewCityRequest
        {
            Name = cityName,
            CityId = cityId,
            WonderId = wonderId,
            WonderLevel = wonderLevel,
        };
        return cityFactory.Create(cityRequest, FogConstants.CITY_PLANNER_VERSION);
    }

    private async Task AddExpansionsAsync(HohCity city, string data)
    {
        var allExpansions = (await GetCityPlannerDataAsync(city.InGameCityId)).Expansions;
        var topLeftCoords = CoordinatesMap[city.InGameCityId];
        foreach (var dataItem in data.Split('.'))
        {
            var parts = dataItem.Split('-');
            if (!int.TryParse(parts[0], out var x))
            {
                continue;
            }

            if (!int.TryParse(parts[1], out var y))
            {
                continue;
            }

            var ex = allExpansions.FirstOrDefault(e => e.X == topLeftCoords.X + x && e.Y == topLeftCoords.Y + y);
            if (ex != null)
            {
                city.UnlockedExpansions.Add(ex.Id);
            }
        }
    }

    private async Task AddCityMapEntitiesAsync(HohCity city, IEnumerable<string> data)
    {
        var cityEntities = new List<HohCityMapEntity>();
        var topLeftCoords = CoordinatesMap[city.InGameCityId];
        var buildings = (await GetCityPlannerDataAsync(city.InGameCityId)).Buildings.ToDictionary(x => x.Id);
        var initLockedEntities = city.Entities.Where(x => x.IsLocked).ToList();
        foreach (var dataItem in data)
        {
            var parts = dataItem.Split('.');
            if (!DefIdMap.TryGetValue(parts[0], out var baseCityEntityId))
            {
                logger.LogWarning("Failed to find city entity id for def id: {Part}", parts[0]);
                continue;
            }

            if (!int.TryParse(parts[1], out var x))
            {
                logger.LogWarning("Failed to parse x coordinate {Part} for city entity id: {CityEntityId}", parts[1],
                    baseCityEntityId);
                continue;
            }

            if (!int.TryParse(parts[2], out var y))
            {
                logger.LogWarning("Failed to parse y coordinate {Part} for city entity id: {CityEntityId}", parts[2],
                    baseCityEntityId);
                continue;
            }

            var isRotated = !string.IsNullOrWhiteSpace(parts[3]) && parts[3] == "1";

            var level = 1;
            if (!string.IsNullOrWhiteSpace(parts[4]) && !int.TryParse(parts[4], out level) && level == 0)
            {
                logger.LogWarning("Failed to parse level {Part} for city entity id: {CityEntityId}", parts[4],
                    baseCityEntityId);
                continue;
            }

            var cityEntityId = $"{baseCityEntityId}_{level}";
            if (!buildings.TryGetValue(cityEntityId, out var building))
            {
                logger.LogWarning("Failed to find building for city entity id: {CityEntityId}", cityEntityId);
                continue;
            }

            var selectedProductIndex = 0;
            if (!string.IsNullOrWhiteSpace(parts[5]) && !int.TryParse(parts[5], out selectedProductIndex))
            {
                logger.LogWarning("Failed to parse selected product index {Part} for city entity id: {CityEntityId}",
                    parts[5], cityEntityId);
                continue;
            }

            string? selectedProduct = null;
            if (selectedProductIndex > 0)
            {
                var productionComponents = building.Components.OfType<ProductionComponent>().ToList();
                if (selectedProductIndex <= productionComponents.Count)
                {
                    selectedProduct = productionComponents[selectedProductIndex - 1].Id;
                }
                else
                {
                    logger.LogWarning(
                        "Failed to find production with index {selectedProductIndex} component for city entity id: {CityEntityId}",
                        selectedProductIndex, cityEntityId);
                }
            }

            var fogX = topLeftCoords.X + x;
            var fogY = topLeftCoords.Y + y;
            var initEntity = initLockedEntities.FirstOrDefault(ie => ie.X == fogX && ie.Y == fogY);
            if (initEntity == null)
            {
                cityEntities.Add(new HohCityMapEntity
                {
                    Id = -1,
                    CityEntityId = cityEntityId,
                    Level = level,
                    X = fogX,
                    Y = fogY,
                    IsRotated = isRotated,
                    SelectedProductId = selectedProduct,
                });
            }
            else
            {
                initEntity.CityEntityId = cityEntityId;
                initEntity.SelectedProductId = selectedProduct;
                if (!string.IsNullOrWhiteSpace(parts[4]))
                {
                    initEntity.Level = level;
                    initEntity.IsLocked = false;
                }
            }
        }

        city.Entities = cityEntities.Concat(initLockedEntities).ToList();
    }
}
