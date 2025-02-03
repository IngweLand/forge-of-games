using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;

public interface IHohCommandCenterTeamFactory
{
    CommandCenterProfileTeam Create(string teamName);
}
