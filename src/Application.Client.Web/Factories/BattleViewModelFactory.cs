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
using Ingweland.Fog.Models.Fog.Enums;
using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
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
    IHohCoreDataCache coreDataCache,
    IUnitStatFactory unitStatFactory) : IBattleViewModelFactory
{
    private static readonly List<string> ExcludedUnitIds = ["spawner", "teslaboss"];

    public PvpBattleViewModel CreatePvpBattle(PlayerViewModel player, PvpBattleDto pvpBattleDto,
        IReadOnlyCollection<HeroDto> heroes, IReadOnlyDictionary<string, AgeDto> ages,
        IReadOnlyDictionary<(string unitId, int unitLevel), BuildingDto> barracks,
        IReadOnlyDictionary<string, RelicDto> relics)
    {
        var heroesDic = heroes.ToDictionary(h => h.Unit.Id);

        var isVictory = pvpBattleDto.Winner.Id == player.Id;
        var winnerUnits = pvpBattleDto.WinnerUnits.Select(u => CreateBasicBattleSquad(u, heroesDic, barracks))
            .OrderBy(uvm => uvm.BattlefieldSlot)
            .ToList();
        var loserUnits = pvpBattleDto.LoserUnits.Select(u => CreateBasicBattleSquad(u, heroesDic, barracks))
            .OrderBy(uvm => uvm.BattlefieldSlot)
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
                PlayerSquads = battle.PlayerSquads.Select(src => CreateBasicBattleSquad(src, heroes, barracks))
                    .ToList(),
                EnemySquads = battle.EnemySquads.Select(src => CreateBasicBattleSquad(src, heroes, barracks))
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

    public async Task<BattleViewModel> CreateBattleViewModel(BattleDto battle)
    {
        var battleSummary = battle.Summary;
        var heroIds = battleSummary.PlayerSquads.Select(s => s.Hero?.UnitId).Where(s => s != null);
        heroIds = heroIds.Concat(battleSummary.EnemySquads.Select(s => s.Hero?.UnitId).Where(s => s != null));

        var heroes =
            (await coreDataCache.GetHeroes(heroIds.Where(x =>
                ExcludedUnitIds.All(y => !x!.Contains(y, StringComparison.InvariantCultureIgnoreCase))).ToHashSet()!))
            .ToDictionary(h => h.Unit.Id);
        var relics = await coreDataCache.GetRelicsAsync();
        var barracks = await coreDataCache.GetBarracksByUnitMapAsync();

        List<BattleSquadBasicViewModel> enemySquads = [];
        if (battleSummary.BattleType == BattleType.Pvp)
        {
            enemySquads = battleSummary.EnemySquads.Select(src => CreateBasicBattleSquad(src, heroes, barracks))
                .ToList();
        }

        var summary = new BattleSummaryViewModel
        {
            Id = battleSummary.Id,
            ResultStatus = battleSummary.ResultStatus,
            PlayerSquads = battleSummary.PlayerSquads.Select(src => CreateBasicBattleSquad(src, heroes, barracks))
                .ToList(),
            EnemySquads = enemySquads,
            StatsId = battleSummary.StatsId,
            BattleType = battleSummary.BattleType,
            PerformedAt = battleSummary.PerformedAt != DateOnly.MinValue
                ? battleSummary.PerformedAt.ToString("d")
                : string.Empty,
            BattleDefinitionId = battleSummary.BattleDefinitionId,
            Difficulty = battleSummary.Difficulty,
        };

        var timeline = new List<BattleTimelineGroupViewModel>();
        foreach (var group in battle.Timeline.GroupBy(x => x.TimeSeconds))
        {
            var entries = new List<BattleTimelineEntryViewModel>();
            foreach (var entry in group)
            {
                var squad = battleSummary.PlayerSquads.FirstOrDefault(x =>
                    x.Hero?.UnitInBattleId == entry.UnitInBattleId);
                var side = BattleSquadSide.Player;
                if (squad == null)
                {
                    squad =
                        battleSummary.EnemySquads.FirstOrDefault(x => x.Hero?.UnitInBattleId == entry.UnitInBattleId);

                    if (squad != null)
                    {
                        side = BattleSquadSide.Enemy;
                    }
                }

                if (ExcludedUnitIds.Any(x =>
                        squad?.Hero?.UnitId.Contains(x, StringComparison.InvariantCultureIgnoreCase) == true))
                {
                    continue;
                }

                if (squad != null)
                {
                    var squadVm = CreateBattleSquad(squad, heroes, barracks, relics, side == BattleSquadSide.Player);
                    var concreteId = HohStringParser.GetConcreteId(entry.AbilityId).Split('_')[0];
                    string title;
                    string iconUrl;
                    var type = BattleTimelineEntryType.Ability;
                    if (relics.TryGetValue(concreteId, out _))
                    {
                        title = squadVm.Relic!.Name;
                        iconUrl = squadVm.Relic!.IconUrl;
                        type = BattleTimelineEntryType.Relic;
                    }
                    else
                    {
                        title = squadVm.Ability.Name;
                        iconUrl = squadVm.Ability.IconUrl;
                    }

                    entries.Add(new BattleTimelineEntryViewModel
                    {
                        Squad = squadVm,
                        Side = side,
                        Title = title,
                        AbilityIconUrl = iconUrl,
                        Type = type,
                    });
                }
            }

            if (entries.Count > 0)
            {
                var ts = TimeSpan.FromSeconds(group.Key);
                timeline.Add(new BattleTimelineGroupViewModel
                {
                    Time = ts.ToString(@"mm\:ss"),
                    Entries = entries,
                });
            }
        }

        return new BattleViewModel
        {
            Summary = summary,
            Timeline = timeline,
        };
    }

    public async Task<HeroProfileViewModel> CreateHeroProfileAsync(IBattleUnitProperties hero,
        IBattleUnitProperties? supportUnit)
    {
        var heroDto = await coreDataCache.GetHeroAsync(hero.UnitId);
        var relics = await coreDataCache.GetRelicsAsync();
        var barracks = await coreDataCache.GetBarracksByUnitMapAsync();

        BuildingDto? concreteBarracks = null;
        if (supportUnit != null)
        {
            barracks.TryGetValue((supportUnit.UnitId, supportUnit.Level), out concreteBarracks);
        }

        HeroRelicViewModel? relicVm = null;
        foreach (var ability in hero.Abilities)
        {
            var concreteId = HohStringParser.GetConcreteId(ability).Split('_')[0];
            if (relics.TryGetValue(concreteId, out var relic))
            {
                relicVm = relicViewModelFactory.Create(relic,
                    relic.LevelData.First(x => x.Abilities.Any(y => y.Id == ability)));
            }
        }

        var fullProfile = heroProfileFactory.Create(hero, heroDto!, concreteBarracks);
        return heroProfileViewModelFactory.Create(fullProfile, heroDto!, [], relicVm);
    }

    private BattleSquadViewModel CreateBattleSquad(BattleSquadDto squad,
        IReadOnlyDictionary<string, HeroDto> heroes,
        IReadOnlyDictionary<(string unitId, int unitLevel), BuildingDto> barracks,
        IReadOnlyDictionary<string, RelicDto> relics, bool withSupportUnit = true)
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
        var profileVm = heroProfileViewModelFactory.Create(profile, hero, [], relicVm, withSupportUnit);
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

    private BattleSquadBasicViewModel CreateBasicBattleSquad(BattleSquadDto squad,
        IReadOnlyDictionary<string, HeroDto> heroes,
        IReadOnlyDictionary<(string unitId, int unitLevel), BuildingDto> barracks)
    {
        var hero = heroes[squad.Hero!.UnitId];

        BuildingDto? concreteBarracks = null;
        if (squad.Unit != null)
        {
            barracks.TryGetValue((squad.Unit.UnitId, squad.Unit.Level), out concreteBarracks);
        }

        var hpUnitStats = hero.Unit.Stats.FirstOrDefault(x => x.Type == UnitStatType.MaxHitPoints);
        var stats = unitStatFactory.CreateHeroStats(hpUnitStats!, hero, squad.Hero!.Level, squad.Hero!.AscensionLevel,
            squad.Hero!.StatBoosts, concreteBarracks);
        string? finalHitPointsFormatted = null;
        var isDead = false;
        if (squad.Hero.FinalState.TryGetValue(UnitStatType.HitPoints, out var hp))
        {
            var statTotals = stats.Sum(kvp2 => kvp2.Value);
            var finalHitPointsPercent = Math.Min(1, hp / statTotals);
            finalHitPointsFormatted = finalHitPointsPercent.ToString("P1");
            isDead = hp <= 0;
        }

        var profileVm = heroProfileViewModelFactory.CreateBasic(squad, hero, finalHitPointsFormatted, isDead);
        return profileVm;
    }
}
