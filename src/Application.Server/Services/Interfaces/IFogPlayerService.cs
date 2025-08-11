using FluentResults;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Fog.Enums;
using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Alliance;

namespace Ingweland.Fog.Application.Server.Services.Interfaces;

public interface IFogPlayerService
{
    Task UpdateStatusAsync(IEnumerable<int> playerIds, InGameEntityStatus status, CancellationToken cancellationToken);
    Task UpdateStatusAsync(int playerId, InGameEntityStatus status, CancellationToken cancellationToken);
    Task UpsertPlayerAsync(PlayerProfile profile, string worldId);
    Task<Result<Player>> UpsertPlayerAsync(string worldId, HohPlayer player, int rankingPoints, DateTime lastOnline);

    Task<Result<IReadOnlyCollection<Player>>> UpsertPlayersAsync(string worldId,
        IReadOnlyCollection<AllianceMember> allianceMembers);
}
