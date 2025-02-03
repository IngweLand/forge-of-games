using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Factories;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;

namespace Ingweland.Fog.WebApp.Services;

public class TreasureHuntUiServerService(
    ITreasureHuntService treasureHuntService,
    ITreasureHuntStageViewModelFactory treasureHuntStageViewModelFactory,
    IMapper mapper)
    : ITreasureHuntUiService
{
    public async Task<IReadOnlyCollection<TreasureHuntDifficultyBasicViewModel>> GetDifficultiesAsync()
    {
        var difficulties = await treasureHuntService.GetDifficultiesAsync();
        return mapper.Map<IReadOnlyCollection<TreasureHuntDifficultyBasicViewModel>>(difficulties);
    }

    public async Task<TreasureHuntStageViewModel?> GetStageAsync(int difficulty, int stageIndex)
    {
        var stage = await treasureHuntService.GetStageAsync(difficulty, stageIndex);
        return stage == null ? null : treasureHuntStageViewModelFactory.Create(stage);
    }
}
