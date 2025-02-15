using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Ingweland.Fog.Application.Server.Interfaces;

public interface IFogDbContext
{
    DbSet<PlayerRanking> PlayerRankings { get; set; }
    DbSet<Player> Players { get; set; }
    DbSet<Alliance> Alliances { get; set; }
    DbSet<AllianceRanking> AllianceRankings { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
