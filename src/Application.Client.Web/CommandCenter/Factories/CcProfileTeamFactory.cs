using Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.CommandCenter.Factories;

public class CcProfileTeamFactory : IHohCommandCenterTeamFactory
{
    public CommandCenterProfileTeam Create(string teamName)
    {
        return new CommandCenterProfileTeam()
        {
            Id = Guid.NewGuid().ToString("N"),
            Name = teamName,
        };
    }
}
