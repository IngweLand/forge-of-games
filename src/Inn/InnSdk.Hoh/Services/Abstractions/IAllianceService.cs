using FluentResults;
using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;
using Ingweland.Fog.Models.Hoh.Entities.Alliance;

namespace Ingweland.Fog.InnSdk.Hoh.Services.Abstractions;

public interface IAllianceService
{
    Task<Result<IReadOnlyCollection<AllianceSearchResult>>> SearchAlliancesAsync(GameWorldConfig world,
        string searchString);

    Task<Result<IReadOnlyCollection<AllianceMember>>> GetMembersAsync(GameWorldConfig world, int allianceId);
    Task<Result<byte[]>> GetMembersRawDataAsync(GameWorldConfig world, int allianceId);

    Task<Result<byte[]>> GetAllianceRawDataAsync(GameWorldConfig world, int allianceId);
}
