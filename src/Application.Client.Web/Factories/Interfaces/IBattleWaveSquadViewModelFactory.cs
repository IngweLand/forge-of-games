using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Entities.Abstractions;

namespace Ingweland.Fog.Application.Client.Web.Factories.Interfaces;

public interface IBattleWaveSquadViewModelFactory
{
    BattleWaveSquadViewModel Create(BattleWaveSquadBase squad, IReadOnlyCollection<UnitDto> units,
        IReadOnlyCollection<HeroDto> heroes);

    IEnumerable<BattleWaveSquadViewModel> Create(IReadOnlyCollection<BattleWaveSquadBase> squads,
        IReadOnlyCollection<UnitDto> units, IReadOnlyCollection<HeroDto> heroes);
}