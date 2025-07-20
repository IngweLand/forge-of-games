using Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.CommandCenter.Factories;

public class HeroProfileIdentifierFactory : IHeroProfileIdentifierFactory
{
    public HeroProfileIdentifier Create(string heroId)
    {
        return new HeroProfileIdentifier
        {
            HeroId = heroId,
        };
    }

    public HeroProfileIdentifier Create(string heroId, int barracksLevel)
    {
        return new HeroProfileIdentifier
        {
            HeroId = heroId,
            BarracksLevel = barracksLevel,
        };
    }

    public HeroProfileIdentifier Create(string heroId, BattleUnitDto battleUnit, int barracksLevel)
    {
        return new HeroProfileIdentifier
        {
            HeroId = heroId,
            Level = battleUnit.Level,
            AscensionLevel = battleUnit.AscensionLevel,
            AbilityLevel = battleUnit.AbilityLevel,
            AwakeningLevel = 0,
            BarracksLevel = barracksLevel,
        };
    }
    
    public HeroProfileIdentifier Create(HeroProfileIdentifier identifier, HeroLevelSpecs levelSpecs)
    {
        return new HeroProfileIdentifier
        {
            HeroId = identifier.HeroId,
            Level = levelSpecs.Level,
            AscensionLevel = levelSpecs.AscensionLevel,
            AbilityLevel = identifier.AbilityLevel,
            AwakeningLevel = identifier.AwakeningLevel,
            BarracksLevel = identifier.BarracksLevel,
        };
    }
}
