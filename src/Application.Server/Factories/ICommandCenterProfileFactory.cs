using Ingweland.Fog.Dtos.Hoh.CommandCenter;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Server.Factories;

public interface ICommandCenterProfileFactory
{
    BasicCommandCenterProfile Create(IReadOnlyCollection<BasicHeroProfile> heroes, BarracksProfile barracksProfile);
}
