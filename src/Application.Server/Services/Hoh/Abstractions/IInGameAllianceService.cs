using FluentResults;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Alliance;

namespace Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;

public interface IInGameAllianceService
{
    Task<Result<IReadOnlyCollection<AllianceMember>>> GetMembersAsync(AllianceKey allianceKey);

    Task<Result<IReadOnlyCollection<AllianceWithLeader>>> SearchAlliancesAsync(string worldId,
        string searchString);

    Task<Result<AllianceWithLeader>> GetAllianceAsync(AllianceKey allianceKey);
}
