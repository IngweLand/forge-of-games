using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities;

namespace Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;

public interface IPlayerProfileService
{
    Task<PlayerProfile?> FetchProfile(PlayerKey playerKey);
    Task UpsertPlayer(PlayerProfile profile, string worldId);
}
