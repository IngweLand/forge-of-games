using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Relic;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;

public interface IHohHeroProfileViewModelFactory
{
    HeroProfileViewModel Create(HeroProfile profile, HeroDto hero, IEnumerable<BuildingDto> barracks,
        RelicViewModel? relic = null, bool withSupportUnit = true);

    HeroProfileBasicViewModel CreateBasic(ProfileSquadDto squad, HeroDto hero);
    BattleSquadBasicViewModel CreateBasic(BattleSquadDto squad, HeroDto hero, string? finalHitPointsFormatted, bool isDead);
}
