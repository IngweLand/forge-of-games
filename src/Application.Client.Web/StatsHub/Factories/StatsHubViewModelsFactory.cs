using AutoMapper;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.StatsHub.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Fog;
using Ingweland.Fog.Shared.Constants;

namespace Ingweland.Fog.Application.Client.Web.StatsHub.Factories;

public class StatsHubViewModelsFactory(
    IMapper mapper,
    IHohHeroProfileFactory heroProfileFactory,
    IHohHeroProfileViewModelFactory heroProfileViewModelFactory,
    IBattleViewModelFactory battleViewModelFactory) : IStatsHubViewModelsFactory
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

    public PlayerProfileViewModel CreatePlayerProfile(PlayerProfileDto playerProfile,
        IReadOnlyCollection<HeroDto> heroes, IReadOnlyDictionary<string, AgeDto> ages,
        IReadOnlyDictionary<(string unitId, int unitLevel), BuildingDto> barracks,
        TreasureHuntDifficultyBasicViewModel? treasureHuntDifficulty, int treasureHuntMaxPoints,
        IReadOnlyDictionary<string, RelicDto> relics)
    {
        var player = mapper.Map<PlayerViewModel>(playerProfile.Player,
            opt => { opt.Items[ResolutionContextKeys.AGES] = ages; });
        var battles = playerProfile.PvpBattles
            .Select(x => battleViewModelFactory.CreatePvpBattle(player, x, heroes, ages, barracks, relics))
            .ToList();
        var heroesDic = heroes.ToDictionary(h => h.Unit.Id);
        var currentAlliance = playerProfile.Alliances.FirstOrDefault(a => a.Id == playerProfile.Player.AllianceId);
        var previousAlliances = playerProfile.Alliances.Where(a => a.Id != playerProfile.Player.AllianceId)
            .OrderBy(a => a.IsDeleted).ThenByDescending(a => a.RankingPoints);
        return new PlayerProfileViewModel
        {
            Player = player,
            Ages = playerProfile.Ages.Select(a => new StatsTimedStringValue
                    {Date = a.Date, Value = ages.TryGetValue(a.Value, out var age) ? age.Name : a.Value})
                .ToList(),
            CurrentAlliance = currentAlliance != null ? mapper.Map<AllianceViewModel>(currentAlliance) : null,
            PreviousAlliances = mapper.Map<IReadOnlyCollection<AllianceViewModel>>(previousAlliances),
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
