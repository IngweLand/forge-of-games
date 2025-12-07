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
    private readonly ConcurrentDictionary<CityId, IReadOnlyDictionary<string, Building>> _buildingsCache = new();
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

        var citySnapshot = await CreateSnapshot(playerId, city, data);

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

    public async Task RecalculateStatsAsync(IEnumerable<int> citySnapshotIds)
    {
        var set = citySnapshotIds.ToHashSet();
        var snapshots = await _context.PlayerCitySnapshots.Include(x => x.Data).Where(x => set.Contains(x.Id))
            .ToListAsync();
        foreach (var snapshot in snapshots)
        {
            var otherCity = _dataParsingService.ParseOtherCity(snapshot.Data.Data);

            var city = await CreateCity(otherCity, string.Empty);
            if (city == null)
            {
                continue;
            }

            var newCitySnapshot = await CreateSnapshot(snapshot.PlayerId, city, snapshot.Data.Data);
            snapshot.TotalArea = newCitySnapshot.TotalArea;
            snapshot.HappinessUsageRatio = newCitySnapshot.HappinessUsageRatio;
            snapshot.HasPremiumHomeBuildings = newCitySnapshot.HasPremiumHomeBuildings;
            snapshot.HasPremiumFarmBuildings = newCitySnapshot.HasPremiumFarmBuildings;
            snapshot.HasPremiumCultureBuildings = newCitySnapshot.HasPremiumCultureBuildings;

            snapshot.Coins = newCitySnapshot.Coins;
            snapshot.Coins1H = newCitySnapshot.Coins1H;
            snapshot.Coins24H = newCitySnapshot.Coins24H;
            snapshot.CoinsPerArea = newCitySnapshot.CoinsPerArea;
            snapshot.Coins1HPerArea = newCitySnapshot.Coins1HPerArea;
            snapshot.Coins24HPerArea = newCitySnapshot.Coins24HPerArea;

            snapshot.Food = newCitySnapshot.Food;
            snapshot.Food1H = newCitySnapshot.Food1H;
            snapshot.Food24H = newCitySnapshot.Food24H;
            snapshot.FoodPerArea = newCitySnapshot.FoodPerArea;
            snapshot.Food1HPerArea = newCitySnapshot.Food1HPerArea;
            snapshot.Food24HPerArea = newCitySnapshot.Food24HPerArea;

            snapshot.Goods = newCitySnapshot.Goods;
            snapshot.Goods1H = newCitySnapshot.Goods1H;
            snapshot.Goods24H = newCitySnapshot.Goods24H;
            snapshot.GoodsPerArea = newCitySnapshot.GoodsPerArea;
            snapshot.Goods1HPerArea = newCitySnapshot.Goods1HPerArea;
            snapshot.Goods24HPerArea = newCitySnapshot.Goods24HPerArea;
        }

        await _context.SaveChangesAsync();
    }

    private async Task<PlayerCitySnapshot> CreateSnapshot(int playerId, HohCity city, byte[] data)
    {
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
                cityStats.Products.TryGetValue("resource.gold_fal", out food);
                break;
            }

            default:
            {
                cityStats.Products.TryGetValue("resource.coins", out coins);
                cityStats.Products.TryGetValue("resource.food", out food);
                break;
            }
        }

        var goods = cityStats.Products.Where(kvp => listOfGoods.Contains(kvp.Key)).Sum(kvp => kvp.Value.Default);
        var goods1H = cityStats.Products.Where(kvp => listOfGoods.Contains(kvp.Key)).Sum(kvp => kvp.Value.OneHour);
        var goods24H = cityStats.Products.Where(kvp => listOfGoods.Contains(kvp.Key)).Sum(kvp => kvp.Value.OneDay);
        return new PlayerCitySnapshot
        {
            PlayerId = playerId,
            CityId = city.InGameCityId,
            AgeId = city.AgeId,
            CollectedAt = Today,
            Data = new PlayerCitySnapshotDataEntity
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
            HasPremiumHomeBuildings =
                await HasPremiumBuildings(city.InGameCityId, BuildingGroup.PremiumHome, city.Entities),
            HasPremiumFarmBuildings =
                await HasPremiumBuildings(city.InGameCityId, BuildingGroup.PremiumFarm, city.Entities),
            HasPremiumCultureBuildings =
                await HasPremiumBuildings(city.InGameCityId, BuildingGroup.PremiumCulture, city.Entities),
            TotalArea = cityStats.TotalArea,
        };
    }

    private async Task<bool> HasPremiumBuildings(CityId cityId, BuildingGroup bg,
        IEnumerable<HohCityMapEntity> cityMapEntities)
    {
        var buildings = await GetBuildingsWithCacheAsync(cityId);
        foreach (var entity in cityMapEntities)
        {
            if (buildings.TryGetValue(entity.CityEntityId, out var building) && building.Group == bg)
            {
                return true;
            }
        }

        return false;
    }

    private async Task<IReadOnlyDictionary<string, Building>> GetBuildingsWithCacheAsync(CityId cityId)
    {
        if (_buildingsCache.TryGetValue(cityId, out var cachedBuildings))
        {
            return cachedBuildings;
        }

        var buildings = await _coreDataRepository.GetBuildingsAsync(cityId);
        var buildingsDict = buildings.ToDictionary(x => x.Id);

        _buildingsCache.TryAdd(cityId, buildingsDict);
        return buildingsDict;
    }

    private async Task<HashSet<string>> CreateListOfGoodsAsync()
    {
        var capitalBuildings = await GetBuildingsWithCacheAsync(CityId.Capital);
        var arabiaBuildings = await GetBuildingsWithCacheAsync(CityId.Arabia_Petra);

        var set = new HashSet<string>();
        foreach (var building in capitalBuildings.Values.Concat(arabiaBuildings.Values))
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
            return _cityFactory.Create(cityDto, buildings, WonderId.Undefined, 0, cityName);
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
