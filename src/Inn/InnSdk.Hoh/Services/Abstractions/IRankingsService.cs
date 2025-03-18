using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;
using Ingweland.Fog.Models.Hoh.Entities.Ranking;

namespace Ingweland.Fog.InnSdk.Hoh.Services.Abstractions;

public interface IRankingsService
{
    Task<AllianceRanks> GetAllianceRankingAsync(GameWorldConfig world, AllianceRankingType rankingType);
    Task<PlayerRanks> GetPlayerRankingAsync(GameWorldConfig world, PlayerRankingType rankingType);
    Task<byte[]> GetAllianceRankingRawDataAsync(GameWorldConfig world, AllianceRankingType rankingType);
    Task<byte[]> GetPlayerRankingRawDataAsync(GameWorldConfig world, PlayerRankingType rankingType);
    Task<byte[]> GetPvpRankingRawDataAsync(GameWorldConfig world, int eventId);
}
