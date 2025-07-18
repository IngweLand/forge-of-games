using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Server.Factories.Interfaces;

public interface ICommandCenterProfileFactory
{
    BasicCommandCenterProfile Create(IReadOnlyCollection<HeroProfileIdentifier> heroes,
        BarracksProfile barracksProfile);
}
