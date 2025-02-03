using Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;
using Ingweland.Fog.Dtos.Hoh.CommandCenter;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.CommandCenter.Factories;

public class HeroProfileDtoFactory : IHohHeroProfileDtoFactory
{
    public BasicHeroProfile Create(string heroId)
    {
        return new BasicHeroProfile()
        {
            Id = Guid.NewGuid().ToString("N"),
            HeroId = heroId,
            Level = 1,
            AscensionLevel = 0,
            AbilityLevel = 1,
            AwakeningLevel = 0,
        };
    }

    public HeroPlaygroundProfile Create(string id, string heroId, int barracksLevel)
    {
        return new HeroPlaygroundProfile()
        {
            Id = id,
            HeroId = heroId,
            Level = 1,
            AscensionLevel = 0,
            AbilityLevel = 1,
            AwakeningLevel = 0,
            BarracksLevel = barracksLevel,
        };
    }
}
