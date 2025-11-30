using FluentResults;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;
using Ingweland.Fog.Models.Hoh.Entities.Ranking;
using PlayerRankingType = Ingweland.Fog.Models.Hoh.Enums.PlayerRankingType;

namespace Ingweland.Fog.InnSdk.Hoh.Services.Abstractions;

public interface IRankingsService
{
    Task<AllianceRanks> GetAllianceRankingAsync(GameWorldConfig world, AllianceRankingType rankingType);
    Task<Result<PlayerRanks>> GetPlayerRankingAsync(GameWorldConfig world, PlayerRankingType rankingType);
    Task<byte[]> GetAllianceRankingRawDataAsync(GameWorldConfig world, AllianceRankingType rankingType);
    Task<Result<byte[]>> GetPlayerRankingRawDataAsync(GameWorldConfig world, PlayerRankingType rankingType);
}
