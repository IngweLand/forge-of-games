using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Refit;

namespace Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;

public interface ITreasureHuntService
{
    [Get(FogUrlBuilder.ApiRoutes.TREASURE_HUNT_DIFFICULTIES_PATH)]
    Task<IReadOnlyCollection<TreasureHuntDifficultyDataBasicDto>> GetDifficultiesAsync();

    [Get(FogUrlBuilder.ApiRoutes.TREASURE_HUNT_STAGE_TEMPLATE_REFIT)]
    Task<TreasureHuntStageDto?> GetStageAsync(int difficulty, int stageIndex);
}