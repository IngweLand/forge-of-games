using System.Collections.ObjectModel;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;

namespace Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;

public interface ITreasureHuntUiService
{
    Task<IReadOnlyCollection<TreasureHuntDifficultyBasicViewModel>> GetDifficultiesAsync();
    Task<TreasureHuntStageViewModel?> GetStageAsync(int difficulty, int stageIndex);
    Task<IReadOnlyDictionary<(int difficulty, int stage), ReadOnlyDictionary<int, int>>> GetBattleEncounterToIndexMapAsync();
}
