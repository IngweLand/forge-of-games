using Ingweland.Fog.Application.Client.Web.CommandCenter.Models;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;

public interface IHohHeroProfileViewModelFactory
{
    HeroProfileViewModel Create(HeroProfile profile, HeroDto hero, BuildingLevelRange barracksRanges);
}
