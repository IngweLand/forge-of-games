using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Caching.Interfaces;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Application.Core.Extensions;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Constants;
using Ingweland.Fog.Shared.Helpers;

namespace Ingweland.Fog.Application.Client.Web.Factories;

public class BattleViewModelFactory(
    IMapper mapper,
    IHohHeroProfileFactory heroProfileFactory,
    IHohHeroProfileViewModelFactory heroProfileViewModelFactory,
    IHeroRelicViewModelFactory relicViewModelFactory,
    IHohHeroLevelSpecsProvider heroLevelSpecsProvider,
    IResourceLocalizationService resourceLocalizationService,
    IHohCoreDataCache coreDataCache) : IBattleViewModelFactory
{
    public PvpBattleViewModel CreatePvpBattle(PlayerViewModel player, PvpBattleDto pvpBattleDto,
        IReadOnlyCollection<HeroDto> heroes, IReadOnlyDictionary<string, AgeDto> ages,
        IReadOnlyDictionary<(string unitId, int unitLevel), BuildingDto> barracks,
        IReadOnlyDictionary<string, RelicDto> relics)
    {
        var heroesDic = heroes.ToDictionary(h => h.Unit.Id);

        var isVictory = pvpBattleDto.Winner.Id == player.Id;
        var winnerUnits = pvpBattleDto.WinnerUnits.Select(u => CreateBattleSquad(u, heroesDic, barracks, relics))
            .OrderBy(uvm => uvm.Identifier.HeroId)
            .ToList();
        var loserUnits = pvpBattleDto.LoserUnits.Select(u => CreateBattleSquad(u, heroesDic, barracks, relics))
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

    public async Task<IReadOnlyCollection<BattleSummaryViewModel>> CreateBattleSummaryViewModels(
        IReadOnlyCollection<BattleSummaryDto> battles, BattleType battleType)
    {
        var heroIds =
            battles.SelectMany(src => src.PlayerSquads.Select(s => s.Hero?.UnitId).Where(s => s != null));
        if (battleType == BattleType.Pvp)
        {
            heroIds = heroIds.Concat(battles.SelectMany(src =>
                src.EnemySquads.Select(s => s.Hero?.UnitId).Where(s => s != null)));
        }

        var heroes = (await coreDataCache.GetHeroes(heroIds.ToHashSet()!)).ToDictionary(h => h.Unit.Id);
        var relics = await coreDataCache.GetRelicsAsync();
        var barracks = await coreDataCache.GetBarracksByUnitMapAsync();

        return battles.Select(battle => new BattleSummaryViewModel
            {
                Id = battle.Id,
                ResultStatus = battle.ResultStatus,
                PlayerSquads = battle.PlayerSquads.Select(src => CreateBattleSquad(src, heroes, barracks, relics))
                    .ToList(),
                EnemySquads = battle.EnemySquads.Select(src => CreateBattleSquad(src, heroes, barracks, relics))
                    .ToList(),
                StatsId = battle.StatsId,
                BattleType = battle.BattleType,
                PerformedAt = battle.PerformedAt != DateOnly.MinValue ? battle.PerformedAt.ToString("d") : string.Empty,
                BattleDefinitionId = battle.BattleDefinitionId,
                Difficulty = battle.Difficulty,
            })
            .ToList();
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

    public IReadOnlyCollection<UnitBattleTypeViewModel> CreateUnitBattleTypes(IEnumerable<BattleType> unitBattleTypes)
    {
        return unitBattleTypes.ToHashSet().Select(battleType => new UnitBattleTypeViewModel
        {
            BattleTypeName = resourceLocalizationService.Localize(battleType),
            BattleType = battleType,
        }).ToList();
    }

    public async Task<BattleSummaryViewModel> CreateBattleSummaryViewModel(BattleSummaryDto battle)
    {
        var heroIds =
            battle.PlayerSquads.Select(s => s.Hero?.UnitId).Where(s => s != null);
        if (battle.BattleType == BattleType.Pvp)
        {
            heroIds = heroIds.Concat(battle.EnemySquads.Select(s => s.Hero?.UnitId).Where(s => s != null));
        }

        var heroes = (await coreDataCache.GetHeroes(heroIds.ToHashSet()!)).ToDictionary(h => h.Unit.Id);
        var relics = await coreDataCache.GetRelicsAsync();
        var barracks = await coreDataCache.GetBarracksByUnitMapAsync();

        return new BattleSummaryViewModel
        {
            Id = battle.Id,
            ResultStatus = battle.ResultStatus,
            PlayerSquads = battle.PlayerSquads.Select(src => CreateBattleSquad(src, heroes, barracks, relics))
                .ToList(),
            EnemySquads = battle.EnemySquads.Select(src => CreateBattleSquad(src, heroes, barracks, relics))
                .ToList(),
            StatsId = battle.StatsId,
            BattleType = battle.BattleType,
            PerformedAt = battle.PerformedAt != DateOnly.MinValue ? battle.PerformedAt.ToString("d") : string.Empty,
            BattleDefinitionId = battle.BattleDefinitionId,
            Difficulty = battle.Difficulty,
        };
    }

    private BattleSquadViewModel CreateBattleSquad(BattleSquadDto squad,
        IReadOnlyDictionary<string, HeroDto> heroes,
        IReadOnlyDictionary<(string unitId, int unitLevel), BuildingDto> barracks,
        IReadOnlyDictionary<string, RelicDto> relics)
    {
        var hero = heroes[squad.Hero!.UnitId];
        BuildingDto? concreteBarracks = null;
        if (squad.Unit != null)
        {
            barracks.TryGetValue((squad.Unit.UnitId, squad.Unit.Level), out concreteBarracks);
        }

        HeroRelicViewModel? relicVm = null;
        foreach (var ability in squad.Hero.Abilities)
        {
            var concreteId = HohStringParser.GetConcreteId(ability).Split('_')[0];
            if (relics.TryGetValue(concreteId, out var relic))
            {
                relicVm = relicViewModelFactory.Create(relic,
                    relic.LevelData.First(x => x.Abilities.Any(y => y.Id == ability)));
            }
        }

        var profile = heroProfileFactory.Create(squad.Hero!, hero, concreteBarracks);
        var profileVm = heroProfileViewModelFactory.Create(profile, hero, [], relicVm);
        string? finalHitPointsFormatted = null;
        var isDead = false;
        if (squad.Hero.FinalState.TryGetValue(UnitStatType.HitPoints, out var hp))
        {
            var finalHitPointsPercent = Math.Min(1, hp / profile.Stats[UnitStatType.MaxHitPoints]);
            finalHitPointsFormatted = finalHitPointsPercent.ToString("P1");
            isDead = hp <= 0;
        }

        return new BattleSquadViewModel(profileVm, finalHitPointsFormatted, isDead, squad.BattlefieldSlot);
    }
}
