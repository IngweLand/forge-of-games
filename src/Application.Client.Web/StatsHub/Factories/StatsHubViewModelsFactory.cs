using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Extensions;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.StatsHub.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Fog;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Ingweland.Fog.Shared.Constants;

namespace Ingweland.Fog.Application.Client.Web.StatsHub.Factories;

public class StatsHubViewModelsFactory(
    IMapper mapper,
    IAssetUrlProvider assetUrlProvider,
    IHohHeroLevelSpecsProvider heroLevelSpecsProvider) : IStatsHubViewModelsFactory
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
        IReadOnlyDictionary<string, AgeDto> ages)
    {
        return new TopStatsViewModel
        {
            MainWorldPlayers = mapper.Map<IReadOnlyCollection<PlayerViewModel>>(mainPlayers,
                opt => { opt.Items[ResolutionContextKeys.AGES] = ages; }),
            BetaWorldPlayers = mapper.Map<IReadOnlyCollection<PlayerViewModel>>(betaPlayers,
                opt => { opt.Items[ResolutionContextKeys.AGES] = ages; }),
            MainWorldAlliances = mapper.Map<IReadOnlyCollection<AllianceViewModel>>(mainAlliances),
            BetaWorldAlliances = mapper.Map<IReadOnlyCollection<AllianceViewModel>>(betaAlliances),
        };
    }

    public PlayerWithRankingsViewModel CreatePlayer(PlayerWithRankings playerWithRankings,
        IReadOnlyDictionary<string, AgeDto> ages)
    {
        var battles = new List<PvpBattleViewModel>();
        var player = mapper.Map<PlayerViewModel>(playerWithRankings.Player,
            opt => { opt.Items[ResolutionContextKeys.AGES] = ages; });
        var heroes = playerWithRankings.Heroes.ToDictionary(h => h.Unit.Id);
        foreach (var pvpBattleDto in playerWithRankings.PvpBattles)
        {
            var isVictory = pvpBattleDto.Winner.Id == playerWithRankings.Player.Id;
            var winnerUnits = pvpBattleDto.WinnerUnits.Select(u =>
                {
                    var hero = heroes[u.Hero.Id];
                    return CreatePvpUnit(u, hero);
                })
                .OrderBy(uvm => uvm.Id)
                .ToList();
            var loserUnits = pvpBattleDto.LoserUnits.Select(u =>
                {
                    var hero = heroes[u.Hero.Id];
                    return CreatePvpUnit(u, hero);
                })
                .OrderBy(uvm => uvm.Id)
                .ToList();
            battles.Add(new PvpBattleViewModel
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
            });
        }

        return new PlayerWithRankingsViewModel
        {
            Player = player,
            Ages = playerWithRankings.Ages.Select(a => new StatsTimedStringValue
                    {Date = a.Date, Value = ages[a.Value].Name})
                .ToList(),
            Alliances = playerWithRankings.Alliances,
            Names = playerWithRankings.Names,
            PvpRankingPoints = playerWithRankings.PvpRankingPoints,
            RankingPoints = playerWithRankings.RankingPoints,
            PvpBattles = battles,
        };
    }

    public BattleSummaryViewModel CreateBattleSummaryViewModel(BattleSummaryDto summaryDto,
        IReadOnlyDictionary<string, HeroDto> heroes)
    {
        return new BattleSummaryViewModel()
        {
            Id = summaryDto.Id,
            ResultStatus = summaryDto.ResultStatus,
            PlayerSquads = summaryDto.PlayerSquads.Select(src => CreateBattleViewModel(src, heroes[src.UnitId]))
                .ToList(),
            StatsId = summaryDto.StatsId,
        };
    }

    private BattleHeroViewModel CreateBattleViewModel(BattleSquadDto squad, HeroDto hero)
    {
        var levelSpecs = heroLevelSpecsProvider.Get(hero.ProgressionCosts.Count);
        var level = levelSpecs.First(ls =>
            ls.Level == squad.Level && ls.AscensionLevel == squad.AscensionLevel);

        var pvpUnitViewModel = new BattleHeroViewModel
        {
            Id = hero.Id,
            Name = hero.Unit.Name,
            Level = level,
            AbilityLevel = squad.AbilityLevel,
            PortraitUrl = assetUrlProvider.GetHohUnitPortraitUrl(hero.Unit.AssetId),
            StarCount = hero.StarClass.ToStarCount(),
            UnitColor = hero.Unit.Color.ToCssColor(),
            UnitClassIconUrl = assetUrlProvider.GetHohIconUrl(hero.ClassId.GetClassIconId()),
            UnitTypeIconUrl =
                assetUrlProvider.GetHohIconUrl(hero.Unit.Type.GetTypeIconId()),
        };

        return pvpUnitViewModel;
    }

    private BattleHeroViewModel CreatePvpUnit(PvpUnit unit, HeroDto hero)
    {
        var levelSpecs = heroLevelSpecsProvider.Get(hero.ProgressionCosts.Count);
        var level = levelSpecs.First(ls =>
            ls.Level == unit.Hero.Level && ls.AscensionLevel == unit.Hero.AscensionLevel);

        var pvpUnitViewModel = new BattleHeroViewModel
        {
            Id = hero.Id,
            Name = hero.Unit.Name,
            Level = level,
            AbilityLevel = unit.Hero.AbilityLevel,
            PortraitUrl = assetUrlProvider.GetHohUnitPortraitUrl(hero.Unit.AssetId),
            StarCount = hero.StarClass.ToStarCount(),
            UnitColor = hero.Unit.Color.ToCssColor(),
            UnitClassIconUrl = assetUrlProvider.GetHohIconUrl(hero.ClassId.GetClassIconId()),
            UnitTypeIconUrl =
                assetUrlProvider.GetHohIconUrl(hero.Unit.Type.GetTypeIconId()),
        };

        return pvpUnitViewModel;
    }
}
