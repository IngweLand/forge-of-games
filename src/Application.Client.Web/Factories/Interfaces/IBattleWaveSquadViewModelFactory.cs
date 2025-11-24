using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities.Battle;

namespace Ingweland.Fog.Application.Client.Web.Factories.Interfaces;

public interface IBattleWaveSquadViewModelFactory
{
    BattleWaveSquadViewModel Create(BattleWaveSquad squad, IReadOnlyCollection<UnitDto> units,
        IReadOnlyCollection<HeroDto> heroes);

    IEnumerable<BattleWaveSquadViewModel> Create(IReadOnlyCollection<BattleWaveSquad> squads,
        IReadOnlyCollection<UnitDto> units, IReadOnlyCollection<HeroDto> heroes);
}