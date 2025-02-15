using AutoMapper;
using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Server.Mapping.Hoh;
using Ingweland.Fog.Application.Server.Services.Hoh;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Ranking;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Infrastructure.IntegrationTests;

public class PlayerRankingServiceIntegrationTests : IAsyncLifetime
{
    private readonly DbContextOptions<FogDbContext> _dbOptions;
    private readonly ILogger<PlayerRankingService> _logger;
    private readonly IMapper _mapper;

    public PlayerRankingServiceIntegrationTests()
    {
        _dbOptions = new DbContextOptionsBuilder<FogDbContext>()
            .UseSqlServer(
                "Server=(localdb)\\MSSQLLocalDB;Database=forge_of_games_test;Trusted_Connection=True;MultipleActiveResultSets=true;")
            .Options;

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddMaps(typeof(Application.Server.DependencyInjection).Assembly);
        });
        _mapper = mapperConfig.CreateMapper();

        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        _logger = loggerFactory.CreateLogger<PlayerRankingService>();
    }

    public async Task InitializeAsync()
    {
        await using var context = new FogDbContext(_dbOptions);
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await using var context = new FogDbContext(_dbOptions);
        await context.Database.EnsureDeletedAsync();
    }

    [Fact]
    public async Task AddOrUpdateRangeAsync_Should_AddNewPlayers_When_EmptyDatabase()
    {
        await using var context = new FogDbContext(_dbOptions);
        var service = new PlayerRankingService(context, _mapper, _logger);

        var rankings = new List<PlayerRank>
        {
            new() {Id = 1, Name = "N1", Age = "A", Points = 999, Rank = 1},
            new() {Id = 2, Name = "N2", Age = "A", Points = 900, Rank = 2},
        };

        await service.AddOrUpdateRangeAsync(rankings, "world1", new DateOnly(2024, 2, 19),
            PlayerRankingType.RankingPoints);

        Assert.Equal(2, await context.Players.CountAsync());
        Assert.Equal(2, await context.PlayerRankings.CountAsync());
    }

    [Fact]
    public async Task AddOrUpdateRangeAsync_Should_AddNewPlayers_When_OtherPlayersExist()
    {
        await using var context = new FogDbContext(_dbOptions);
        context.Players.Add(new Player
        {
            InGamePlayerId = 99, Name = "ExistingPlayer", Age = "A", WorldId = "world1",
            Rankings = new List<PlayerRanking>(), UpdatedAt = new DateOnly(),
            Rank = 1, RankingPoints = 1,
        });
        await context.SaveChangesAsync();

        var service = new PlayerRankingService(context, _mapper, _logger);
        var rankings = new List<PlayerRank>
        {
            new() {Id = 1, Name = "Player1", Age = "Medieval", Points = 1500, Rank = 1},
        };

        await service.AddOrUpdateRangeAsync(rankings, "world1", new DateOnly(2024, 2, 19),
            PlayerRankingType.RankingPoints);

        Assert.Equal(2, await context.Players.CountAsync());
    }

    [Fact]
    public async Task AddOrUpdateRangeAsync_Should_UpdateExistingPlayers_When_PlayerExist()
    {
        const string worldId = "world1";
        await using var context = new FogDbContext(_dbOptions);
        context.Players.Add(new Player
        {
            InGamePlayerId = 1, Name = "N", Age = "A", WorldId = worldId, UpdatedAt = new DateOnly(),
            Rankings = new List<PlayerRanking>(), Rank = 1, RankingPoints = 1,
        });
        await context.SaveChangesAsync();

        var service = new PlayerRankingService(context, _mapper, _logger);
        var rankings = new List<PlayerRank>
        {
            new() {Id = 1, Name = "N", Age = "A", Points = 1100, Rank = 1},
        };

        await service.AddOrUpdateRangeAsync(rankings, worldId, new DateOnly(2024, 2, 19),
            PlayerRankingType.RankingPoints);

        var updatedPlayer = await context.Players.Include(player => player.Rankings)
            .FirstOrDefaultAsync(p => p.InGamePlayerId == 1);
        Assert.NotNull(updatedPlayer);
        Assert.Single(updatedPlayer.Rankings);
        Assert.Equal(1100, updatedPlayer.RankingPoints);
    }

    [Fact]
    public async Task
        AddOrUpdateRangeAsync_Should_UpdatePlayerAndAddRanking_When_PlayerAndRankingExist_But_NewRankingIsForDifferentDate()
    {
        const string worldId = "world1";
        await using var context = new FogDbContext(_dbOptions);
        var existingRanking = new PlayerRanking()
        {
            InGamePlayerId = 1, Name = "N", Age = "A", WorldId = worldId, CollectedAt = new DateOnly(), Points = 1,
            Rank = 1, Type = PlayerRankingType.RankingPoints,
        };
        context.Players.Add(new Player
        {
            InGamePlayerId = 1, Name = "N", Age = "A", WorldId = worldId, UpdatedAt = new DateOnly(),
            Rankings = new List<PlayerRanking>() {existingRanking}, Rank = 1, RankingPoints = 1,
        });
        await context.SaveChangesAsync();

        var service = new PlayerRankingService(context, _mapper, _logger);
        var rankings = new List<PlayerRank>
        {
            new() {Id = 1, Name = "N", Age = "A", Points = 1100, Rank = 1},
        };

        await service.AddOrUpdateRangeAsync(rankings, worldId, new DateOnly(2025, 1, 1),
            PlayerRankingType.RankingPoints);

        var updatedPlayer = await context.Players.Include(player => player.Rankings)
            .FirstOrDefaultAsync(p => p.InGamePlayerId == 1);
        Assert.NotNull(updatedPlayer);
        Assert.Equal(2, updatedPlayer.Rankings.Count);
        Assert.Equal(1100, updatedPlayer.RankingPoints);
        Assert.Equal(1, updatedPlayer.Rankings.OrderBy(pr => pr.CollectedAt).First().Points);
        Assert.Equal(1100, updatedPlayer.Rankings.OrderByDescending(pr => pr.CollectedAt).First().Points);
    }

    [Fact]
    public async Task AddOrUpdateRangeAsync_Should_UpdatePlayerAndRanking_When_PlayerAndRankingExist()
    {
        const string worldId = "world1";
        await using var context = new FogDbContext(_dbOptions);
        var existingRanking = new PlayerRanking()
        {
            InGamePlayerId = 1, Name = "N", Age = "A", WorldId = worldId, CollectedAt = new DateOnly(), Points = 1,
            Rank = 1, Type = PlayerRankingType.RankingPoints,
        };
        context.Players.Add(new Player
        {
            InGamePlayerId = 1, Name = "N", Age = "A", WorldId = worldId, UpdatedAt = new DateOnly(),
            Rankings = new List<PlayerRanking>() {existingRanking}, Rank = 1, RankingPoints = 1,
        });
        await context.SaveChangesAsync();

        var service = new PlayerRankingService(context, _mapper, _logger);
        var rankings = new List<PlayerRank>
        {
            new() {Id = 1, Name = "N", Age = "A", Points = 1100, Rank = 1},
        };

        await service.AddOrUpdateRangeAsync(rankings, worldId, new DateOnly(), PlayerRankingType.RankingPoints);

        var updatedPlayer = await context.Players.Include(player => player.Rankings)
            .FirstOrDefaultAsync(p => p.InGamePlayerId == 1);
        Assert.NotNull(updatedPlayer);
        Assert.Single(updatedPlayer.Rankings);
        Assert.Equal(1100, updatedPlayer.RankingPoints);
        Assert.Equal(1100, updatedPlayer.Rankings.First().Points);
    }

    [Fact]
    public async Task AddOrUpdateRankings_WhenMultipleRankingsForSamePlayerAreProvided_ShouldSaveHighestRanking()
    {
        await using var context = new FogDbContext(_dbOptions);
        var service = new PlayerRankingService(context, _mapper, _logger);

        var rankings = new List<PlayerRank>
        {
            new() {Id = 1, Name = "N", Age = "A", Points = 1000, Rank = 1},
            new() {Id = 1, Name = "N", Age = "A", Points = 1200, Rank = 1},
            new() {Id = 1, Name = "N", Age = "A", Points = 900, Rank = 1},
        };

        await service.AddOrUpdateRangeAsync(rankings, "world1", new DateOnly(2024, 2, 19),
            PlayerRankingType.RankingPoints);

        var player = await context.Players.Include(player => player.Rankings)
            .SingleOrDefaultAsync(p => p.InGamePlayerId == 1);
        Assert.NotNull(player);
        Assert.Single(player.Rankings);
        Assert.Equal(1200, player.Rankings.First().Points);
    }
    
    [Fact]
    public async Task AddOrUpdateRankings_AddingResearchRankings_Should_CalculatePlayerRankingPoints_Correctly()
    {
        await using var context = new FogDbContext(_dbOptions);
        var service = new PlayerRankingService(context, _mapper, _logger);
        const int researchPoints = 1000;
        var rankings = new List<PlayerRank>
        {
            new() {Id = 1, Name = "N", Age = "A", Points = researchPoints, Rank = 1},
        };

        await service.AddOrUpdateRangeAsync(rankings, "world1", new DateOnly(2024, 2, 19),
            PlayerRankingType.ResearchPoints);

        var player = await context.Players.SingleOrDefaultAsync(p => p.InGamePlayerId == 1);
        Assert.NotNull(player);
        Assert.Equal(researchPoints * HohConstants.RESEARCH_TO_PLAYER_RANKING_POINTS_FACTOR, player.RankingPoints);
    }
}