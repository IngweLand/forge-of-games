using Ingweland.Fog.Dtos.Hoh.Battle;

namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;

public class TreasureHuntDifficultyBasicViewModel
{
    public int Difficulty { get; init; }
    public required string IconUrl { get; init; }
    public required string Name { get; init; }

    public required IReadOnlyCollection<TreasureHuntStageBasicDto> Stages { get; init; } =
        new List<TreasureHuntStageBasicDto>();
}
