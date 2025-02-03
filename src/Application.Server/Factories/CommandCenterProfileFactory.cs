using Ingweland.Fog.Dtos.Hoh.CommandCenter;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Server.Factories;

public class CommandCenterProfileFactory : ICommandCenterProfileFactory
{
    public BasicCommandCenterProfile Create(IReadOnlyCollection<BasicHeroProfile> heroes,
        BarracksProfile barracksProfile)
    {
        return new BasicCommandCenterProfile()
        {
            Id = Guid.NewGuid().ToString("N"),
            Name = $"Import - {DateTime.Now:g}",
            Heroes = heroes.ToList(),
            BarracksProfile = barracksProfile,
        };
    }
}
