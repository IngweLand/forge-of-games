using System.Collections.Concurrent;
using Ingweland.Fog.Application.Core.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Core.CityPlanner.Stats;
using Ingweland.Fog.Application.Core.Extensions;
using Ingweland.Fog.Application.Core.Interfaces;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Application.Server.PlayerCity.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Providers;
using Ingweland.Fog.InnSdk.Hoh.Services.Abstractions;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Entities.Rewards;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Server.PlayerCity;

public class PlayerCityService : IPlayerCityService
{
    private readonly ConcurrentDictionary<CityId, List<Building>> _buildingsCache = new();
    private readonly ICityExpansionsHasher _cityExpansionsHasher;
    private readonly IHohCityFactory _cityFactory;
    private readonly ICityStatsCalculator _cityStatsCalculator;
    private readonly IFogDbContext _context;
    private readonly IHohCoreDataRepository _coreDataRepository;
    private readonly IDataParsingService _dataParsingService;
    private readonly IGameWorldsProvider _gameWorldsProvider;
    private readonly IInnSdkClient _innSdkClient;
    private readonly Lazy<Task<HashSet<string>>> _listOfGoodsLazy;
    private readonly ILogger<PlayerCityService> _logger;

    public PlayerCityService(IFogDbContext context,
        IGameWorldsProvider gameWorldsProvider,
        IDataParsingService dataParsingService,
        IHohCoreDataRepository coreDataRepository,
        IHohCityFactory cityFactory,
        IInnSdkClient innSdkClient,
        ICityStatsCalculator cityStatsCalculator,
        ICityExpansionsHasher cityExpansionsHasher,
        ILogger<PlayerCityService> logger)
    {
        _context = context;
        _gameWorldsProvider = gameWorldsProvider;
        _dataParsingService = dataParsingService;
        _coreDataRepository = coreDataRepository;
        _cityFactory = cityFactory;
        _innSdkClient = innSdkClient;
        _cityStatsCalculator = cityStatsCalculator;
        _cityExpansionsHasher = cityExpansionsHasher;
        _logger = logger;

        _listOfGoodsLazy = new Lazy<Task<HashSet<string>>>(CreateListOfGoodsAsync);
    }

    private static DateOnly Today => DateTime.UtcNow.ToDateOnly();

    public async Task<byte[]?> FetchCityAsync(string gameWorldId, int inGamePlayerId, CityId cityId = CityId.Capital)
    {
        var gameWorld = _gameWorldsProvider.GetGameWorlds().FirstOrDefault(config => config.Id == gameWorldId);
        if (gameWorld == null)
        {
            _logger.LogWarning("GameWorld with ID {GameWorldId} not found.", gameWorldId);
            return null;
        }

        byte[] data;
        try
        {
            data = await _innSdkClient.CityService.GetOtherCityRawDataAsync(gameWorld, inGamePlayerId,
                cityId.ToInGameId());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error fetching city: {@Data}",
                new {GameWorldId = gameWorld.Id, playerId = inGamePlayerId});
            return null;
        }

        return TryParseCity(data, gameWorld.Id, inGamePlayerId) == null ? null : data;
    }

    public async Task<PlayerCitySnapshot?> SaveCityAsync(int playerId, byte[] data)
    {
        var otherCity = _dataParsingService.ParseOtherCity(data);

        var existing = await GetCityAsync(playerId, otherCity.CityId, Today);

        if (existing != null)
        {
            return existing;
        }

        var city = await CreateCity(otherCity, string.Empty);
        if (city == null)
        {
            return null;
        }

        var cityStats = await _cityStatsCalculator.Calculate(city);

        var listOfGoods = await _listOfGoodsLazy.Value;

        ConsolidatedTimedProductionValues? coins;
        ConsolidatedTimedProductionValues? food;
        switch (city.InGameCityId)
        {
            case CityId.Arabia_Petra:
            case CityId.Arabia_CityOfBrass:
            case CityId.Arabia_NoriasOfHama:
            {
                cityStats.Products.TryGetValue("resource.dirham", out coins);
                cityStats.Products.TryGetValue("resource.gold_fal", out  food);
                break;
            }
            
            default:
            {
                cityStats.Products.TryGetValue("resource.coins", out coins);
                cityStats.Products.TryGetValue("resource.food", out  food);
                break;
            }
        }
        
        
        var goods = cityStats.Products.Where(kvp => listOfGoods.Contains(kvp.Key)).Sum(kvp => kvp.Value.Default);
        var goods1H = cityStats.Products.Where(kvp => listOfGoods.Contains(kvp.Key)).Sum(kvp => kvp.Value.OneHour);
        var goods24H = cityStats.Products.Where(kvp => listOfGoods.Contains(kvp.Key)).Sum(kvp => kvp.Value.OneDay);
        var citySnapshot = new PlayerCitySnapshot
        {
            PlayerId = playerId,
            CityId = city.InGameCityId,
            AgeId = city.AgeId,
            CollectedAt = Today,
            Data = new PlayerCitySnapshotDataEntity()
            {
                Data = data,
            },
            Coins = coins?.Default ?? 0,
            Coins1H = coins?.OneHour ?? 0,
            Coins24H = coins?.OneDay ?? 0,
            CoinsPerArea = (coins?.Default ?? 0) / cityStats.TotalArea,
            Coins1HPerArea = (coins?.OneHour ?? 0) / cityStats.TotalArea,
            Coins24HPerArea = (coins?.OneDay ?? 0) / cityStats.TotalArea,
            Food = food?.Default ?? 0,
            Food1H = food?.OneHour ?? 0,
            Food24H = food?.OneDay ?? 0,
            FoodPerArea = (food?.Default ?? 0) / cityStats.TotalArea,
            Food1HPerArea = (food?.OneHour ?? 0) / cityStats.TotalArea,
            Food24HPerArea = (food?.OneDay ?? 0) / cityStats.TotalArea,
            Goods = goods,
            Goods1H = goods1H,
            Goods24H = goods24H,
            GoodsPerArea = goods / cityStats.TotalArea,
            Goods1HPerArea = goods1H / cityStats.TotalArea,
            Goods24HPerArea = goods24H / cityStats.TotalArea,
            HappinessUsageRatio = cityStats.HappinessUsageRatio,
            OpenedExpansionsHash = _cityExpansionsHasher.Compute(city.UnlockedExpansions),
            HasPremiumBuildings = city.Entities.Any(x =>
                x.CityEntityId.Contains("premium", StringComparison.InvariantCultureIgnoreCase)),
            TotalArea = cityStats.TotalArea,
        };

        _context.PlayerCitySnapshots.Add(citySnapshot);
        await _context.SaveChangesAsync();

        return citySnapshot;
    }

    public Task<PlayerCitySnapshot?> GetCityAsync(int playerId, CityId cityId, DateOnly date)
    {
        return _context.PlayerCitySnapshots
            .Include(x => x.Data)
            .FirstOrDefaultAsync(x => x.PlayerId == playerId && x.CityId == cityId && x.CollectedAt == date);
    }

    private async Task<List<Building>> GetBuildingsWithCacheAsync(CityId cityId)
    {
        if (_buildingsCache.TryGetValue(cityId, out var cachedBuildings))
        {
            return cachedBuildings;
        }

        var buildings = await _coreDataRepository.GetBuildingsAsync(cityId);
        var buildingsList = buildings.ToList();

        _buildingsCache.TryAdd(cityId, buildingsList);
        return buildingsList;
    }

    private async Task<HashSet<string>> CreateListOfGoodsAsync()
    {
        var capitalBuildings = await GetBuildingsWithCacheAsync(CityId.Capital);
        var arabiaBuildings = await GetBuildingsWithCacheAsync(CityId.Arabia_Petra);

        var set = new HashSet<string>();
        foreach (var building in capitalBuildings.Concat(arabiaBuildings))
        {
            var productionComponents = building.Components.OfType<ProductionComponent>().ToList();
            foreach (var productionComponent in productionComponents)
            {
                var products = productionComponent.Products.OfType<ResourceReward>().Select(x => x.ResourceId);
                set.UnionWith(products);
            }
        }

        var resourcesToIgnore = new HashSet<string>
        {
            "resource.coins", "resource.food", "resource.mastery_points", "resource.hero_xp", "resource.embers",
            "resource.dirham", "resource.gold_fal",
        };
        set.ExceptWith(resourcesToIgnore);
        return set;
    }

    private async Task<HohCity?> CreateCity(OtherCity cityDto, string playerName)
    {
        try
        {
            var buildings = await GetBuildingsWithCacheAsync(cityDto.CityId);

            var cityName = $"{playerName} - {cityDto.CityId} - {DateTime.UtcNow:d}";
            return _cityFactory.Create(cityDto, buildings.ToDictionary(b => b.Id), WonderId.Undefined, 0, cityName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create HohCity for cityId: {CityId}", cityDto.CityId);
        }

        return null;
    }

    private OtherCity? TryParseCity(byte[] data, string gameWorldId, int playerId)
    {
        try
        {
            return _dataParsingService.ParseOtherCity(data);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error parsing city raw data: {@Data}", new {Date = Today, gameWorldId, playerId});
            return null;
        }
    }
}
