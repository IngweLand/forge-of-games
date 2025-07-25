using AutoMapper;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Models;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Application.Core.Extensions;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Fog;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Constants;

namespace Ingweland.Fog.Application.Client.Web.StatsHub.Factories;

public class StatsHubViewModelsFactory(
    IMapper mapper,
    IHohHeroLevelSpecsProvider heroLevelSpecsProvider,
    IResourceLocalizationService resourceLocalizationService,
    IHohHeroProfileFactory heroProfileFactory,
    IHohHeroProfileViewModelFactory heroProfileViewModelFactory) : IStatsHubViewModelsFactory
{
    public PaginatedList<PlayerViewModel> CreatePlayers(PaginatedList<PlayerDto> players,
        IReadOnlyDictionary<string, AgeDto> ages)
    {
        return mapper.Map<PaginatedList<PlayerViewModel>>(players,
            opt => { opt.Items[ResolutionContextKeys.AGES] = ages; });
    }

    public AllianceWithRankingsViewModel CreateAlliance(AllianceWithRankings alliance,
        IReadOnlyDictionary<string, AgeDto> ages)
    {
        return mapper.Map<AllianceWithRankingsViewModel>(alliance,
            opt => { opt.Items[ResolutionContextKeys.AGES] = ages; });
    }

    public PaginatedList<AllianceViewModel> CreateAlliances(PaginatedList<AllianceDto> players)
    {
        return mapper.Map<PaginatedList<AllianceViewModel>>(players);
    }

    public TopStatsViewModel CreateTopStats(IReadOnlyCollection<PlayerDto> mainPlayers,
        IReadOnlyCollection<PlayerDto> betaPlayers,
        IReadOnlyCollection<AllianceDto> mainAlliances, IReadOnlyCollection<AllianceDto> betaAlliances,
        IReadOnlyCollection<string> topHeroes,
        IReadOnlyDictionary<string, AgeDto> ages,
        IReadOnlyCollection<HeroBasicViewModel> heroes)
    {
        var heroesDic = heroes.ToDictionary(h => h.UnitId);
        return new TopStatsViewModel
        {
            MainWorldPlayers = mapper.Map<IReadOnlyCollection<PlayerViewModel>>(mainPlayers,
                opt => { opt.Items[ResolutionContextKeys.AGES] = ages; }),
            BetaWorldPlayers = mapper.Map<IReadOnlyCollection<PlayerViewModel>>(betaPlayers,
                opt => { opt.Items[ResolutionContextKeys.AGES] = ages; }),
            MainWorldAlliances = mapper.Map<IReadOnlyCollection<AllianceViewModel>>(mainAlliances),
            BetaWorldAlliances = mapper.Map<IReadOnlyCollection<AllianceViewModel>>(betaAlliances),
            Heroes = topHeroes.Select(x => heroesDic[x]).ToList(),
        };
    }

    public PlayerProfileViewModel CreatePlayerProfile(PlayerProfile playerProfile,
        IReadOnlyCollection<HeroDto> heroes, IReadOnlyDictionary<string, AgeDto> ages,
        IReadOnlyDictionary<(string unitId, int unitLevel), BuildingDto> barracks,
        TreasureHuntDifficultyBasicViewModel? treasureHuntDifficulty, int treasureHuntMaxPoints)
    {
        var player = mapper.Map<PlayerViewModel>(playerProfile.Player,
            opt => { opt.Items[ResolutionContextKeys.AGES] = ages; });
        var battles = playerProfile.PvpBattles
            .Select(x => CreatePvpBattle(player, x, heroes, ages, barracks)).ToList();
        var heroesDic = heroes.ToDictionary(h => h.Unit.Id);
        return new PlayerProfileViewModel
        {
            Player = player,
            Ages = playerProfile.Ages.Select(a => new StatsTimedStringValue
                    {Date = a.Date, Value = ages[a.Value].Name})
                .ToList(),
            Alliances = mapper.Map<IReadOnlyCollection<AllianceViewModel>>(playerProfile.Alliances),
            Names = playerProfile.Names.Count > 1
                ? string.Join(", ", playerProfile.Names.Select(name => $"\"{name}\""))
                : null,
            PvpRankingPoints = playerProfile.PvpRankingPoints,
            RankingPoints = playerProfile.RankingPoints,
            PvpBattles = battles,
            TreasureHuntDifficulty = treasureHuntDifficulty,
            TreasureHuntMaxPoints = treasureHuntMaxPoints,
            Squads = playerProfile.Squads.Select(src => CreateProfileSquad(src, heroesDic, barracks)).ToList(),
        };
    }

    public IReadOnlyCollection<UnitBattleViewModel> CreateUnitBattleViewModels(
        IReadOnlyCollection<UnitBattleDto> unitBattles)
    {
        var levelSpecs = heroLevelSpecsProvider.Get(500);

        return unitBattles.Select(src =>
            {
                var level = levelSpecs.First(ls =>
                    ls.Level == src.Unit.Level && ls.AscensionLevel == src.Unit.AscensionLevel);
                var battleType = src.BattleDefinitionId.ToBattleType();
                return new UnitBattleViewModel
                {
                    UnitId = src.Unit.UnitId,
                    Level = level,
                    AbilityLevel = src.Unit.AbilityLevel,
                    AttackValue = (src.BattleStats?.Attack)?.ToString("N0") ?? string.Empty,
                    DefenseValue = (src.BattleStats?.Defense)?.ToString("N0") ?? string.Empty,
                    HealValue = (src.BattleStats?.Heal)?.ToString("N0") ?? string.Empty,
                    BattleTypeName = resourceLocalizationService.Localize(battleType),
                    BattleDefinitionId = src.BattleDefinitionId,
                    BattleType = battleType,
                    Difficulty = src.Difficulty,
                };
            })
            .OrderBy(src => src.Level)
            .ToList();
    }

    public BattleSummaryViewModel CreateBattleSummaryViewModel(BattleSummaryDto summaryDto,
        IReadOnlyDictionary<string, HeroDto> heroes,
        IReadOnlyDictionary<(string unitId, int unitLevel), BuildingDto> barracks)
    {
        return new BattleSummaryViewModel
        {
            Id = summaryDto.Id,
            ResultStatus = summaryDto.ResultStatus,
            PlayerSquads = summaryDto.PlayerSquads.Select(src => CreateBattleSquad(src, heroes, barracks))
                .ToList(),
            EnemySquads = summaryDto.EnemySquads.Select(src => CreateBattleSquad(src, heroes, barracks))
                .ToList(),
            StatsId = summaryDto.StatsId,
            BattleType = summaryDto.BattleType,
        };
    }

    public IReadOnlyCollection<UnitBattleTypeViewModel> CreateUnitBattleTypes(IEnumerable<BattleType> unitBattleTypes)
    {
        return unitBattleTypes.ToHashSet().Select(battleType => new UnitBattleTypeViewModel
        {
            BattleTypeName = resourceLocalizationService.Localize(battleType),
            BattleType = battleType,
        }).ToList();
    }

    public PvpBattleViewModel CreatePvpBattle(PlayerViewModel player, PvpBattleDto pvpBattleDto,
        IReadOnlyCollection<HeroDto> heroes, IReadOnlyDictionary<string, AgeDto> ages,
        IReadOnlyDictionary<(string unitId, int unitLevel), BuildingDto> barracks)
    {
        var heroesDic = heroes.ToDictionary(h => h.Unit.Id);

        var isVictory = pvpBattleDto.Winner.Id == player.Id;
        var winnerUnits = pvpBattleDto.WinnerUnits.Select(u => CreateBattleSquad(u, heroesDic, barracks))
            .OrderBy(uvm => uvm.Identifier.HeroId)
            .ToList();
        var loserUnits = pvpBattleDto.LoserUnits.Select(u => CreateBattleSquad(u, heroesDic, barracks))
            .OrderBy(uvm => uvm.Identifier.HeroId)
            .ToList();

        return new PvpBattleViewModel
        {
            Player = player,
            Opponent = isVictory
                ? mapper.Map<PlayerViewModel>(pvpBattleDto.Loser,
                    opt => { opt.Items[ResolutionContextKeys.AGES] = ages; })
                : mapper.Map<PlayerViewModel>(pvpBattleDto.Winner,
                    opt => { opt.Items[ResolutionContextKeys.AGES] = ages; }),
            IsVictory = isVictory,
            PlayerUnits = isVictory ? winnerUnits : loserUnits,
            OpponentUnits = isVictory ? loserUnits : winnerUnits,
            StatsId = pvpBattleDto.StatsId,
        };
    }

    private BattleSquadViewModel CreateBattleSquad(BattleSquadDto squad,
        IReadOnlyDictionary<string, HeroDto> heroes,
        IReadOnlyDictionary<(string unitId, int unitLevel), BuildingDto> barracks)
    {
        var hero = heroes[squad.Hero!.UnitId];
        BuildingDto? concreteBarracks = null;
        if (squad.Unit != null)
        {
            barracks.TryGetValue((squad.Unit.UnitId, squad.Unit.Level), out concreteBarracks);
        }

        var profile = heroProfileFactory.Create(squad.Hero!, hero, concreteBarracks);
        var profileVm = heroProfileViewModelFactory.Create(profile, hero, []);
        string? finalHitPointsPercent = null;
        var isDead = false;
        if (squad.Hero.FinalState.TryGetValue(UnitStatType.HitPoints, out var hp))
        {
            finalHitPointsPercent = (hp / profile.Stats[UnitStatType.MaxHitPoints]).ToString("P1");
            isDead = hp <= 0;
        }

        return new BattleSquadViewModel(profileVm, finalHitPointsPercent, isDead);
    }

    private HeroProfileViewModel CreateProfileSquad(ProfileSquadDto squad,
        IReadOnlyDictionary<string, HeroDto> heroes,
        IReadOnlyDictionary<(string unitId, int unitLevel), BuildingDto> barracks)
    {
        var hero = heroes[squad.Hero.UnitId];
        barracks.TryGetValue((squad.SupportUnit.UnitId, squad.SupportUnit.Level), out var concreteBarracks);

        var profile = heroProfileFactory.Create(squad.Hero!, hero, concreteBarracks);
        return heroProfileViewModelFactory.Create(profile, hero, []);
    }
}
