using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Dtos.Hoh.Battle;

namespace Ingweland.Fog.Application.Client.Web.Factories;

public class TreasureHuntStageViewModelFactory(
    IHohTreasureHuntDifficultyIconUrlProvider difficultyIconUrlProvider,
    IBattleWaveSquadViewModelFactory battleWaveSquadViewModelFactory) : ITreasureHuntStageViewModelFactory
{
    public TreasureHuntStageViewModel Create(TreasureHuntStageDto stageDto)
    {
        var encounters = stageDto.Battles
            .Select((e, index) => new TreasureHuntEncounterViewModel
            {
                Title = (index + 1).ToString(),
                Waves = Enumerable.Select(e.Waves, (bw, bwi) =>
                    new BattleWaveViewModel
                    {
                        Title = $"~{index + 1}.{bwi + 1}",
                        Squads = bw.Squads.Select(bws =>
                                battleWaveSquadViewModelFactory.Create(bws, stageDto.Units, stageDto.Heroes))
                            .ToList().AsReadOnly(),
                        AggregatedSquads = bw.Squads.GroupBy(bws => bws.Unit.UnitId)
                            .SelectMany(g =>
                                battleWaveSquadViewModelFactory.Create(g.ToList(), stageDto.Units, stageDto.Heroes))
                            .ToList()
                    }).ToList().AsReadOnly()
            })
            .ToList().AsReadOnly();

        return new TreasureHuntStageViewModel
        {
            Name = stageDto.Name,
            DifficultyName = stageDto.DifficultyName,
            DifficultyIconUrl = difficultyIconUrlProvider.GetIconUrl(stageDto.Difficulty),
            Encounters = encounters,
        };
    }
}
