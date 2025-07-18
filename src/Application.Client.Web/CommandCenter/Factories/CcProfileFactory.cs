using AutoMapper;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Core.Extensions;
using Ingweland.Fog.Application.Core.Factories.Interfaces;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.CommandCenter;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.CommandCenter.Factories;

public class CcProfileFactory(IHohHeroProfileFactory heroProfileFactory, IMapper mapper)
    : IHohCommandCenterProfileFactory
{
    public CommandCenterProfile Create(BasicCommandCenterProfile profileDto,
        IReadOnlyDictionary<string, HeroDto> heroes, IReadOnlyCollection<BuildingDto> barracks)
    {
        var profile = mapper.Map<CommandCenterProfile>(profileDto);

        profile.Heroes = profileDto.Heroes.Select(src =>
        {
            var hero = heroes[src.HeroId];
            var group = hero.Unit.Type.ToBuildingGroup();
            var heroBarracks = barracks.First(b =>
                b.Group == group && b.Level == profile.BarracksProfile.Levels[group]);
            return heroProfileFactory.Create(src, hero, heroBarracks);
        }).ToDictionary(hp => hp.Identifier.HeroId);

        return profile;
    }

    public CommandCenterProfile Create(string profileName)
    {
        return new CommandCenterProfile()
        {
            Id = Guid.NewGuid().ToString("N"),
            Name = profileName,
            BarracksProfile = new BarracksProfile(),
        };
    }
}
