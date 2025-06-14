using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Ingweland.Fog.Models.Hoh.Entities.Ranking;

namespace Ingweland.Fog.InnSdk.Hoh.Services.Abstractions;

public interface IDataParsingService
{
    PlayerRanks ParsePlayerRankings(byte[] data);
    AllianceRanks ParseAllianceRankings(byte[] data);
    IReadOnlyCollection<PvpRank> ParsePvpRankings(byte[] data);
    IReadOnlyCollection<PvpBattle> ParsePvpBattles(byte[] data);
    Wakeup ParseWakeup(byte[] data);
}