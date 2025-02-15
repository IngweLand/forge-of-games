using Ingweland.Fog.Infrastructure.Entities;
using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ingweland.Fog.Infrastructure.EntityConfigurations;

public class PlayerRankingEntityTypeConfiguration : IEntityTypeConfiguration<PlayerRanking>
{
    public void Configure(EntityTypeBuilder<PlayerRanking> builder)
    {
        builder.ToTable("player_rankings");

        builder.HasKey(p => new {p.WorldId, p.InGamePlayerId, p.CollectedAt});
        
        builder.Ignore(p => p.Key);
        
        builder.Property(p => p.Name).IsRequired().HasMaxLength(500);
        builder.Property(p => p.Age).IsRequired().HasMaxLength(255);
        builder.Property(p => p.Type).IsRequired();
        builder.Property(p => p.Points).IsRequired();
        builder.Property(p => p.Rank).IsRequired();
        builder.Property(p => p.AllianceName).HasMaxLength(500);
        builder.Property(p => p.WorldId).IsRequired().HasMaxLength(48);
        builder.Property(p => p.CollectedAt).IsRequired();

        builder.HasIndex(p => new {p.WorldId, p.InGamePlayerId});
        builder.HasIndex(p => p.CollectedAt).IsDescending();
        builder.HasIndex(p => p.WorldId);
    }
}
