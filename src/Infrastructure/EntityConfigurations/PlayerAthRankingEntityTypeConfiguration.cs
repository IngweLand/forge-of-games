using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ingweland.Fog.Infrastructure.EntityConfigurations;

public class PlayerAthRankingEntityTypeConfiguration : IEntityTypeConfiguration<PlayerAthRanking>
{
    public void Configure(EntityTypeBuilder<PlayerAthRanking> builder)
    {
        builder.ToTable("player_ath_rankings");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Points).IsRequired();
        builder.Property(p => p.UpdatedAt).IsRequired();
        builder.Property(p => p.InGameEventId).IsRequired();

        builder.HasIndex(p => p.Points).IsDescending();
        builder.HasIndex(p => p.InGameEventId);
        builder.HasIndex(p => new {p.PlayerId, p.InGameEventId}).IsUnique();
    }
}
