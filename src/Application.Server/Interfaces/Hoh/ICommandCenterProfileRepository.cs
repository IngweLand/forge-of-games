using Ingweland.Fog.Dtos.Hoh.CommandCenter;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Server.Interfaces.Hoh;

public interface ICommandCenterProfileRepository
{
    Task<BasicCommandCenterProfile?> GetAsync(string profileId);
    Task<string> SaveAsync(BasicCommandCenterProfile profile);
}
