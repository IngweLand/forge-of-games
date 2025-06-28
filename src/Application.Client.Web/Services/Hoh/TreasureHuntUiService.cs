using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Dtos.Hoh.Battle;

namespace Ingweland.Fog.Application.Client.Web.Services.Hoh;

public class TreasureHuntUiService(
    ITreasureHuntService treasureHuntService,
    ITreasureHuntStageViewModelFactory treasureHuntStageViewModelFactory,
    IMapper mapper)
    : ITreasureHuntUiService
{
    private TreasureHuntEncounterMapDto? _treasureHuntEncounterMap;

    private readonly Dictionary<(int difficulty, int stageIndex), TreasureHuntStageViewModel> _stages = new();
    private IReadOnlyCollection<TreasureHuntDifficultyBasicViewModel>? _difficulties;

    public async Task<IReadOnlyCollection<TreasureHuntDifficultyBasicViewModel>> GetDifficultiesAsync()
    {
        if (_difficulties != null)
        {
            return _difficulties;
        }

        var difficulties = await treasureHuntService.GetDifficultiesAsync();
        _difficulties = mapper.Map<IReadOnlyCollection<TreasureHuntDifficultyBasicViewModel>>(difficulties);
        return _difficulties;
    }

    public async Task<TreasureHuntStageViewModel?> GetStageAsync(int difficulty, int stageIndex)
    {
        var key = (difficulty, stageIndex);
        if (_stages.TryGetValue(key, out var cachedStage))
        {
            return cachedStage;
        }

        var stage = await treasureHuntService.GetStageAsync(difficulty, stageIndex);
        if (stage == null)
        {
            return null;
        }

        var stageViewModel = treasureHuntStageViewModelFactory.Create(stage);
        _stages[key] = stageViewModel;
        return stageViewModel;
    }

    public async Task<TreasureHuntEncounterMapDto> GetBattleEncounterToIndexMapAsync()
    {
        if (_treasureHuntEncounterMap != null)
        {
            return _treasureHuntEncounterMap;
        }

        _treasureHuntEncounterMap = await treasureHuntService.GetBattleEncounterToIndexMapAsync();
        return _treasureHuntEncounterMap;
    }
}
