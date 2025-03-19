using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Entities.Battle;

namespace Ingweland.Fog.Application.Server.Factories.Interfaces;

public interface ITreasureHuntStageDtoFactory
{
    TreasureHuntStageDto Create(TreasureHuntStage stage, int difficulty, IReadOnlyCollection<UnitDto> units,
        IReadOnlyCollection<HeroDto> heroes);
}