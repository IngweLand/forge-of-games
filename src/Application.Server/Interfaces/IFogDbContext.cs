using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Application.Server.Interfaces;

public interface IFogDbContext
{
    DbSet<AllianceRanking> AllianceRankings { get; set; }
    DbSet<Alliance> Alliances { get; set; }
    DbSet<BattleSummaryEntity> Battles { get; set; }
    DbSet<BattleStatsEntity> BattleStats { get; set; }
    DbSet<BattleUnitEntity> BattleUnits { get; set; }
    DbSet<PlayerCitySnapshot> PlayerCitySnapshots { get; set; }
    DbSet<PlayerAllianceNameHistoryEntry> PlayerAllianceNameHistory { get; set; }
    DbSet<PlayerRanking> PlayerRankings { get; set; }
    DbSet<Player> Players { get; set; }
    DbSet<PvpBattle> PvpBattles { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
