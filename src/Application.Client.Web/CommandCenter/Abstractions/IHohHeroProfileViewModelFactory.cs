using Ingweland.Fog.Application.Client.Web.CommandCenter.Models;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;

public interface IHohHeroProfileViewModelFactory
{
    HeroProfileViewModel CreateForCommandCenterProfile(HeroProfile profile, HeroDto hero);

    HeroProfileViewModel CreateForPlayground(HeroProfile profile, HeroDto hero,
        IReadOnlyCollection<BuildingDto> barracks);
}
