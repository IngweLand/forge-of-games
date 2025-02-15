using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Infrastructure.Entities;
using Ingweland.Fog.Infrastructure.EntityConfigurations;
using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Infrastructure;

public class FogDbContext : DbContext, IFogDbContext
{
    public FogDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<PlayerRanking> PlayerRankings { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<Alliance> Alliances { get; set; }
    public DbSet<AllianceRanking> AllianceRankings { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new PlayerEntityTypeConfiguration());
        builder.ApplyConfiguration(new PlayerRankingEntityTypeConfiguration());
        builder.ApplyConfiguration(new AllianceEntityTypeConfiguration());
        builder.ApplyConfiguration(new AllianceRankingEntityTypeConfiguration());
    }
}
