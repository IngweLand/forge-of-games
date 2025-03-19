using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Entities.Battle;

namespace Ingweland.Fog.Application.Server.Factories;

public class TreasureHuntStageDtoFactory(IHohGameLocalizationService localizationService) : ITreasureHuntStageDtoFactory
{
    public TreasureHuntStageDto Create(TreasureHuntStage stage, int difficulty, IReadOnlyCollection<UnitDto> units,
        IReadOnlyCollection<HeroDto> heroes)
    {
        return new TreasureHuntStageDto()
        {
            Index = stage.Index,
            Difficulty = difficulty,
            Name = localizationService.GetTreasureHuntStageName(stage.Index),
            DifficultyName = localizationService.GetTreasureHuntDifficulty(difficulty),
            Battles = stage.Battles,
            Units = units,
            Heroes = heroes,
        };
    }
}
