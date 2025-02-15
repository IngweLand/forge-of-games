using Ingweland.Fog.Models.Hoh.Entities.Ranking;

namespace Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;

public interface IInGameDataParsingService
{
    PlayerRanks ParsePlayerRanking(string inputData);
    AllianceRanks ParseAllianceRankings(string inputData);
    byte[] Decode(string inputData);
}
