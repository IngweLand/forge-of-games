using Ingweland.Fog.Models.Fog.Enums;
using Ingweland.Fog.Models.Hoh.Entities;

namespace Ingweland.Fog.Application.Server.Services.Interfaces;

public interface IFogPlayerService
{
    Task UpdateStatusAsync(IEnumerable<int> playerIds, PlayerStatus status, CancellationToken cancellationToken);
    Task UpdateStatusAsync(int playerId, PlayerStatus status, CancellationToken cancellationToken);
    Task UpsertPlayer(PlayerProfile profile, string worldId);
}
