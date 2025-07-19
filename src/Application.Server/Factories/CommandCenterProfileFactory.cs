using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Dtos.Hoh.CommandCenter;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Server.Factories;

public class CommandCenterProfileFactory : ICommandCenterProfileFactory
{
    public BasicCommandCenterProfile Create(IReadOnlyCollection<HeroProfileIdentifier> heroes,
        BarracksProfile barracksProfile)
    {
        return new BasicCommandCenterProfile()
        {
            Id = Guid.NewGuid().ToString("N"),
            Name = $"Import - {DateTime.Now:g}",
            Heroes = heroes.ToList(),
            BarracksProfile = barracksProfile,
            SchemaVersion = FogConstants.CC_UI_PROFILE_SCHEME_VERSION,
            CommandCenterVersion = FogConstants.COMMAND_CENTER_VERSION,
        };
    }
}
