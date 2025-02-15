using Ingweland.Fog.Models.Hoh.Entities.Ranking;

namespace Ingweland.Fog.InnSdk.Hoh.Services.Abstractions;

public interface IDataParsingService
{
    PlayerRanks ParsePlayerRankings(byte[] data);
    AllianceRanks ParseAllianceRankings(byte[] data);
}