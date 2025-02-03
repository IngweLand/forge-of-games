using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Dtos.Hoh.Battle;

namespace Ingweland.Fog.Application.Client.Web.Factories.Interfaces;

public interface ITreasureHuntStageViewModelFactory
{
    TreasureHuntStageViewModel Create(TreasureHuntStageDto stageDto);
}
