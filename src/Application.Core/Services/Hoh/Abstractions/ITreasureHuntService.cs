using Ingweland.Fog.Dtos.Hoh.Battle;

namespace Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;

public interface ITreasureHuntService
{
    Task<IReadOnlyCollection<TreasureHuntDifficultyDataBasicDto>> GetDifficultiesAsync();
    Task<TreasureHuntStageDto?> GetStageAsync(int difficulty, int stageIndex);
}
