using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Ranking;

namespace Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;

public interface IInGameDataParsingService
{
    PlayerRanks ParsePlayerRanking(string inputData);
    IReadOnlyCollection<PvpRank> ParsePvpRanking(string inputData);
    AllianceRanks ParseAllianceRankings(string inputData);
    byte[] Decode(string inputData);
    Wakeup ParseWakeup(string inputData);
}